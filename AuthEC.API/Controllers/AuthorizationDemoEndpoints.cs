using AuthEC.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AuthEC.API.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("AdminOnly", AdminOnly);

            app.MapGet("AdminOrTeacher", [Authorize(Roles = "Admin,Teacher")] () => {
                return Results.Ok("This endpoint is accessible to users with either the Admin or Teacher role.");
            });

            app.MapGet("LibraryMemberOnly", [Authorize(Policy = "HasLibraryId")] () =>
            {
                return Results.Ok("This endpoint is only accessible to users with a LibraryId claim.");
            });

            app.MapGet("ApplyForMaternityLeave", [Authorize(Policy = "Female")] () =>
            {
                return Results.Ok("Your maternity leave is approved!");
            });
           
            app.MapGet("User10Only", [Authorize(Policy = "User10")] () =>
            {
                return Results.Ok("This endpoint is only accessible to users whose DOB claim indicates they are older than 10 years.");
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