using AuthEC.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AuthEC.API.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("AdminOnly", AdminOnly);

            app.MapGet("AdminOrTeacher", [Authorize(Roles ="Admin,Teacher")] ()=>{
                return Results.Ok("This endpoint is accessible to users with either the Admin or Teacher role.");
            });

            return app;
        }

        [Authorize(Roles = "Admin")]
        private static IResult AdminOnly()
        {
            return Results.Ok("This endpoint is only accessible to users with the Admin role.");
        }
    }
}