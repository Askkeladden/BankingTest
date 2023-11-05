using System.ComponentModel.DataAnnotations;

namespace BankingSystemTest.UserAccount
{
    public class UserAccountDTO
    {
        [Key]
        public int UserID { get; set; }
        [Key]
        public int AccountID { get; set; }
        public double AccountBalance { get; set; }
    }
}
