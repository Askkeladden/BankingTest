using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace BankingSystemTest.UserAccount
{
    [PrimaryKey(nameof(AccountID), nameof(UserID))]
    public class UserAccount : IUserAccount
    {
        private const double _MaximumDepositLimit = 10000;

        private const double _MaxWithdrawPercentage = 90;

        private const double _MinimumLimitAccountBalance = 100;

        public int UserID { get; set; }
        public int AccountID { get; set; }
        public double AccountBalance { get; set; }

        public double MinimumAccountBalance
        {
            get { return _MinimumLimitAccountBalance; } 
        }

        public double MaximumDepositAmount
        {
            get { return _MaximumDepositLimit; }
        }

        public double MaximumWithdrawlLimit
        {
            get { return AccountBalance * (_MaxWithdrawPercentage / 100); }
        }

        public UserAccount()
        {
            
        }

        public UserAccount(int inputAccountID, int inputUserId)
        {
            UserID = inputUserId;
            AccountID = inputAccountID;   
        }

        public bool CheckMinimum(double amount)
        {
            if (amount < _MinimumLimitAccountBalance)
            {
                return false;
            }

            return true;
        }

        public bool CheckDepositMaximum(double amount)
        {
            if (amount > _MaximumDepositLimit)
            {
                return false;
            }

            return true;
        }

        public bool CheckWithdrawMaxAmount(double amount)
        {
            if (amount > MaximumWithdrawlLimit)
            {
                return false;
            }

            return true;
        }

        public bool CheckWithdrawMinimum(double amount)
        {
            if ((AccountBalance - amount) < _MinimumLimitAccountBalance)
            {
                return false;
            }

            return true;
        }
    }
}
