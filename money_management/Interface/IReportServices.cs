using money_management.Models;

namespace money_management.Interface
{
    public interface IReportServices
    {
        public int AddTransaction(string Reason, int Amount, int balance, byte Transaction_type, DateTime currentDate, int userId, int User_BankId);

        public IEnumerable<Report> displayReports(int UserId, int User_BankId);
        public IEnumerable<Report> displayReportsById(int UserId);
        public IEnumerable<Report> searchReportByAllAccounts(int UserId, string search);
        public IEnumerable<Report> searchReportByAccount(int UserId, string search, int BankId);
    }
}
