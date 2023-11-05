using BankingSystemTest.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemTest.UserAccount
{
    public static class UserAccountEndpoints
    {
        public static void RegisterUserAccountEndpoints(this IEndpointRouteBuilder app)
        {
            var accountendpoints = app.MapGroup("/api/useraccount");

            accountendpoints.MapGet("/{userid}", async (int userid, BankingDatabase db) =>
                await db.accounts.Where(x => x.UserID == userid).ToListAsync())
            .WithName("GetAllUsersAccountsOfUser")
            .WithOpenApi();

            accountendpoints.MapGet("/{userid}/{accountid}", async (int userid, int accountid, BankingDatabase db) =>
                await db.accounts.FindAsync(accountid, userid))
            .WithName("GetUserAccount")
            .WithOpenApi();

            accountendpoints.MapPost("/", async (UserAccountDTO inputaccount, BankingDatabase db, CancellationToken cancellationToken) =>
            {
                var useraccount = new UserAccount(inputaccount.AccountID, inputaccount.UserID);
                if (!useraccount.CheckMinimum(inputaccount.AccountBalance))
                {
                    return TypedResults.BadRequest($"Minimum deposit to create an accoun not reached {useraccount.MinimumAccountBalance}$");
                }

                if (!useraccount.CheckDepositMaximum(inputaccount.AccountBalance))
                {
                    return TypedResults.BadRequest($"Maximum deposit amount of {useraccount.MaximumDepositAmount}$ exceeded");
                }

                if (db.accounts.Where(x => x.AccountID == inputaccount.AccountID && 
                                           x.UserID == inputaccount.UserID).Any())
                {
                    return TypedResults.BadRequest($"Account {inputaccount.AccountID} for User {inputaccount.UserID} already exists");
                }
                useraccount.AccountBalance = inputaccount.AccountBalance;
                db.accounts.Add(useraccount);
                await db.SaveChangesAsync();

                return Results.Created($"/{inputaccount.AccountID}", inputaccount);
            })
            .WithName("CreateAccount")
            .WithOpenApi();
            
            accountendpoints.MapPut("/Deposit/{userid}/{accountid}/{DepositAmount}", async (int userid, int accountid, double depositamount, BankingDatabase db) =>
            {
                if (await db.accounts.FindAsync(accountid, userid) is UserAccount useraccount)
                {
                    if (depositamount < 0)
                    {
                        return TypedResults.BadRequest($"Can't deposit negative numbers");
                    }
                    if (!useraccount.CheckDepositMaximum(depositamount)) 
                    { 
                        return TypedResults.BadRequest($"Maximum deposit amount of {useraccount.MaximumDepositAmount}$ exceeded"); 
                    }
                    useraccount.AccountBalance += depositamount;
                    await db.SaveChangesAsync();
                    return TypedResults.Ok();
                }

                return Results.BadRequest("Account not found");
            })
            .WithName("Deposit")
            .WithOpenApi();

            accountendpoints.MapPut("/Withdraw/{userid}/{accountid}/{WithdrawAmount}", async (int userid, int accountid, double withdrawamount, BankingDatabase db) =>
            {
                if (await db.accounts.FindAsync(accountid, userid) is UserAccount useraccount)
                {
                    if (withdrawamount < 0)
                    {
                        return TypedResults.BadRequest($"Can't withdraw negative numbers");
                    }
                    if (!useraccount.CheckWithdrawMaxAmount(withdrawamount))
                    {
                        return TypedResults.BadRequest($"Maximum withdrawl amount of {useraccount.MaximumWithdrawlLimit}$ exceeded");
                    }
                    if (!useraccount.CheckWithdrawMinimum(withdrawamount))
                    {
                        return TypedResults.BadRequest($"Withdrawl not possible account balance would fall beneath the minimum of {useraccount.MinimumAccountBalance}");
                    }
                    useraccount.AccountBalance -= withdrawamount;
                    await db.SaveChangesAsync();
                    return TypedResults.Ok();
                }

                return Results.BadRequest("Account not found");
            })
            .WithName("Withdraw")
            .WithOpenApi();

            accountendpoints.MapDelete("/{userid}/{accountid}", async (int userid, int accountid, BankingDatabase db) =>
            {
                if (await db.accounts.FindAsync(accountid, userid) is UserAccount useraccount)
                {
                    db.accounts.Remove(useraccount);
                    await db.SaveChangesAsync();
                    return TypedResults.NoContent();
                }

                return Results.NotFound();
            })
            .WithName("DeleteAccount")
            .WithOpenApi();
        }


    }
}
