using AuthEC.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthEC.API.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("UserProfile", GetUserProfile);
            return app;
        }

        [Authorize]
        private static async Task<IResult> GetUserProfile(ClaimsPrincipal claims, UserManager<AppUser> userManager)
        {
            string userId= claims.Claims.First(x=>x.Type== "UserId").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            return Results.Ok(new
            {
                Email= userDetails?.Email,
                FullName= userDetails?.FullName
            });
        }
    }
}
