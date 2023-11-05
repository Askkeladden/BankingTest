using BankingSystemTest.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemTest.User
{
    public static class UserEndpoints
    {
        public static void RegisterUserEndpoints(this IEndpointRouteBuilder app)
        {
            var userendpoints = app.MapGroup("/api/user");

            userendpoints.MapGet("/", async (BankingDatabase db) =>
                await db.users.ToListAsync())
            .WithName("GetUsers")
            .WithOpenApi();

            userendpoints.MapGet("/{id}", async (int userid, BankingDatabase db) =>
                await db.users.FindAsync(userid))
            .WithName("GetUser")
            .WithOpenApi();

            userendpoints.MapPost("/", async (User user, BankingDatabase db) =>
            {
                db.users.Add(user);
                await db.SaveChangesAsync();

                return TypedResults.Created($"/{user.UserID}", user);
            })
            .WithName("AddUser")
            .WithOpenApi();

            userendpoints.MapPut("/{id}", async (int userid, User inputUser, BankingDatabase db) =>
            {
                var user = await db.users.FindAsync(userid);

                if (user is null) return Results.NotFound();

                user.UserName = inputUser.UserName;

                await db.SaveChangesAsync();

                return TypedResults.NoContent();
            })
            .WithName("ChangeUser")
            .WithOpenApi();

            userendpoints.MapDelete("/{id}", async (int userid, BankingDatabase db) =>
            {
                if (await db.users.FindAsync(userid) is User user)
                {
                    db.users.Remove(user);
                    await db.SaveChangesAsync();
                    return TypedResults.NoContent();
                }

                return Results.NotFound();
            })
            .WithName("DeleteUser")
            .WithOpenApi(); 
        }
    }
}
