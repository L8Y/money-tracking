using money_management.Models;

namespace money_management.Interface
{
    public interface IBank_detailsService
    {
        public int AddBankAccount(string Account_Name, string Initial_Balance, int UserId);
        public IEnumerable<Bank_details> getAccounts(int UserId);

        public int updateInitialBalance(int UserId, int User_BankId, int Initial_Balance);
        public bool isBankAccountUnique(string Account_Name, int userId);
    }
}
