namespace BankingSystemTest.UserAccount
{
    public interface IUserAccount
    {
        int UserID { get; set; }
        int AccountID { get; set; }
        double AccountBalance { get; set; }
        double MaximumWithdrawlLimit { get; }
        double MinimumAccountBalance { get; }
        double MaximumDepositAmount { get; }

        public bool CheckMinimum(double amount);
        public bool CheckDepositMaximum(double amount);
        public bool CheckWithdrawMaxAmount(double amount);
        public bool CheckWithdrawMinimum(double amount);

    }
}
