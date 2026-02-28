using AuthEC.API.Data;
using AuthEC.API.DTOs;
using Microsoft.AspNetCore.Authorization;
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
            app.MapPost("signup", SignupUser);
            app.MapPost("signin", SigninUser);

            app.MapGet("users", async (UserManager<AppUser> userManager) =>
            {
                //(DateTime.Today).Year - (DateTime.Parse(context.User.Claims.First(x => x.Type == "DOB").Value)).Year
                var users = await userManager.Users.Select(u => new UserDTO
                {
                    FullName = u.FullName,
                    Email = u.Email,
                    DOB = u.DOB.ToString(),
                    Age = (DateTime.Today).Year - (DateTime.Parse(u.DOB.ToString())).Year,
                    Gender= u.Gender
                }).ToListAsync();
                return Results.Ok(users);
            });
            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> SignupUser([FromBody] UserRegistrationDTO userRegistrationDTO, UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                UserName = userRegistrationDTO.Email,
                Email = userRegistrationDTO.Email,
                FullName = userRegistrationDTO.FullName,
                Gender = userRegistrationDTO.Gender,
                DOB = userRegistrationDTO.DOB,
                LibraryId = userRegistrationDTO.LibraryId
            };
            var result = await userManager.CreateAsync(user, userRegistrationDTO.Password!);
            if(userRegistrationDTO.Role !=null)
                await userManager.AddToRoleAsync(user, userRegistrationDTO.Role!);
            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);
        }

        [AllowAnonymous]
        private static async Task<IResult> SigninUser([FromBody] UserLoginDTO userLoginDTO, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IOptions<AppSettings> appSettings)
        {
            var user = await userManager.FindByEmailAsync(userLoginDTO.Email!);
            if (user == null)
                return Results.BadRequest("Invalid email or password.");
            var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password!, false);
            if (!result.Succeeded)
                return Results.BadRequest("Invalid email or password.");
            var roles= await userManager.GetRolesAsync(user);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId",user.Id.ToString()),
                new Claim("Gender", user.Gender!.ToString()),
                new Claim("DOB", user.DOB.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",",roles))
            });
            if(user.LibraryId != null)
                claimsIdentity.AddClaim(new Claim("libraryId", user.LibraryId.ToString()!));

            return Results.Ok(new { Token = GenerateToken(user.Id, claimsIdentity, appSettings) });
        }

        public static string GenerateToken(string userId, ClaimsIdentity claimsIdentity , IOptions<AppSettings> appSettings)
        {
            // Generate JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(appSettings.Value.ExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret!)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
