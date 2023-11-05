using System.ComponentModel.DataAnnotations;

namespace BankingSystemTest.User
{
    public class User : IUser
    {
        [Key]
        public int UserID { get; set; }
        public string? UserName { get; set; }

    }
}
