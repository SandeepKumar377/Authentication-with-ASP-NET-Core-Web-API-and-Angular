using AuthEC.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        private static string GetUserProfile()
        {
            return "This is a protected endpoint. You are authenticated.";
        }
    }
}
