using AuthEC.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 5;
});

builder.Services.AddDbContext<AppDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region JWT configuration (minimal / dev-friendly default)
var jwtKey = builder.Configuration["JWT:JWTSecret"];
var jwtIssuer = builder.Configuration["JWT:Issuer"];
var jwtAudience = builder.Configuration["JWT:Audience"];
int expiryMinutes = int.TryParse(builder.Configuration["Jwt:ExpiryMinutes"], out var m) ? m : 60;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("JwtBearer", options =>
{
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});
#endregion

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

#region CORS
app.UseCors(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.WithOrigins("http://localhost:4200");
});
#endregion

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
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
        Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
        Issuer = jwtIssuer,
        Audience = jwtAudience,
        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
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