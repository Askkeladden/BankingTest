
using BankingSystemTest.User;
using BankingSystemTest.UserAccount;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemTest.Database
{
    public class BankingDatabase : DbContext
    {
        public BankingDatabase(DbContextOptions<BankingDatabase> options) 
            : base(options) { }

        public DbSet<User.User> users => Set<User.User>();

        public DbSet<UserAccount.UserAccount> accounts => Set<UserAccount.UserAccount>();

    }
}
