using AuthEC.API.Data;
using AuthEC.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthEC.API.Controllers
{
    public static class IdentityUserEndPoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("signup", CreateUser);
            app.MapPost("signin", SigninUser);

            app.MapGet("users", async (UserManager<AppUser> userManager) =>
            {
                var users = await userManager.Users.Select(u => new UserDTO
                {
                    FullName = u.FullName,
                    Email = u.Email
                }).ToListAsync();
                return Results.Ok(users);
            });
            return app;
        }

        private static async Task<IResult> CreateUser(
            UserManager<AppUser> userManager,
            [FromBody] UserRegistrationDTO userRegistrationDTO)
            {
            AppUser user = new()
            {
                UserName = userRegistrationDTO.Email,
                Email = userRegistrationDTO.Email,
                FullName = userRegistrationDTO.FullName
            };
            var result = await userManager.CreateAsync(user, userRegistrationDTO.Password!);
            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);
        }
        private static async Task<IResult> SigninUser(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            [FromBody] UserLoginDTO userLoginDTO,
            IOptions<AppSettings> appSettings)
            {
            var user = await userManager.FindByEmailAsync(userLoginDTO.Email!);
            if (user == null)
                return Results.BadRequest("Invalid email or password.");
            var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password!, false);
            if (!result.Succeeded)
                return Results.BadRequest("Invalid email or password.");
            
            return Results.Ok(new { Token = GenerateToken(user.Id,appSettings) });
        }

        public static string GenerateToken(string userId, IOptions<AppSettings> appSettings)
        {
            // Generate JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("UserId",userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(appSettings.Value.ExpiryMinutes),
                Issuer = appSettings.Value.Issuer,
                Audience = appSettings.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret!)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
