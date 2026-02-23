using AuthEC.API.Data;
using AuthEC.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .InjectDBContext(builder.Configuration)
    .AddIdentityHandlersAndStores()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration);

builder.Services.AddSwaggerExplorer();

var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCORS(builder.Configuration)
    .AddIdentityMiddleware();

app.UseHttpsRedirection();
app.MapControllers();


app.MapGroup("/api").MapIdentityApi<AppUser>();

app.MapPost("/api/signup", async (
    UserManager<AppUser> userManager,
    [FromBody] UserRegistrationDTO userRegistrationDTO) =>
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
});

app.MapPost("/api/signin", async (
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    [FromBody] UserLoginDTO userLoginDTO) =>
{
    var user = await userManager.FindByEmailAsync(userLoginDTO.Email!);
    if (user == null)
        return Results.BadRequest("Invalid email or password.");
    var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password!, false);
    if (!result.Succeeded)
        return Results.BadRequest("Invalid email or password.");
    // Generate JWT token
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim("UserId",user.Id.ToString())
        }),
        Expires = DateTime.UtcNow.AddMinutes(int.TryParse(builder.Configuration["Jwt:ExpiryMinutes"], out var m) ? m : 60),
        Issuer = builder.Configuration["JWT:Issuer"],
        Audience = builder.Configuration["JWT:Audience"],
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWTSecret"]!)), SecurityAlgorithms.HmacSha256Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var jwtToken = tokenHandler.WriteToken(token);
    return Results.Ok(new { Token = jwtToken });
});

app.MapGet("/api/users", async (UserManager<AppUser> userManager) =>
{
    var users = await userManager.Users
        .Select(u => new UserDTO
        {
            FullName = u.FullName,
            Email = u.Email
        })
        .ToListAsync();

    return Results.Ok(users);
});

app.Run();

public class UserRegistrationDTO
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? FullName { get; set; }
    [Required]
    public string? Password { get; set; }
}

public class UserLoginDTO
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}

public class UserDTO
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
}