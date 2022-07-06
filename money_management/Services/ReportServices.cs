using money_management.Interface;
using money_management.Models;
using System.Data.SqlClient;
using System.Linq;

namespace money_management.Services
{
    public class ReportServices : IReportServices
    {
        public string Constr { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public ReportServices(IConfiguration configuration)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DBConnection");
            con = new SqlConnection(Constr);
        }

        public bool isFirstEntry(int UserId, int User_BankId)
        {
            List<Report> reportCount = new List<Report>();  
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var isFirstEntryQuery = "select count(Report_No) from [dbo].[Report] where UserId = @UserId and User_BankId = @User_BankId";
                SqlCommand command = new SqlCommand(isFirstEntryQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@User_BankId", User_BankId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader[0]) == 0)
                        {
                            return true;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return false;
        }
        public int AddTransaction(string Reason, int Amount, int balance, byte Transaction_type, DateTime currentDate, int userId, int User_BankId)
        {
            int isTransactionDone = 0;
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var AddTransactionquery = "insert into [dbo].[Report] values(@Date, @Reason, @Amount, @Balance, @Transaction_type, @UserId, @User_BankId)";
                SqlCommand command = new SqlCommand(AddTransactionquery, connection);
                command.Parameters.AddWithValue("@Date", currentDate);
                command.Parameters.AddWithValue("@Reason", Reason);
                command.Parameters.AddWithValue("@Amount", Amount);
                command.Parameters.AddWithValue("@Balance", balance);
                command.Parameters.AddWithValue("@Transaction_type", Transaction_type);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@User_BankId", User_BankId);
                try
                {
                    connection.Open();
                    isTransactionDone = command.ExecuteNonQuery();
                    connection.Close();                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return isTransactionDone;
        }

        public IEnumerable<Report> displayReports(int UserId, int User_BankId)
        {
            List<Report> reports = new List<Report>();
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var displayReportsQuery = "SELECT  [Date], [Reason] ,[Amount], [Balance], [Transaction_type] FROM [dbo].[Report] where UserId = @UserId and User_BankId = @User_BankId";
                SqlCommand command = new SqlCommand(displayReportsQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@User_BankId", User_BankId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new Report
                        {
                            Date = Convert.ToDateTime(reader[0]),
                            Reason = reader[1].ToString(),
                            Amount = Convert.ToInt32(reader[2]),
                            Balance = Convert.ToInt32(reader[3]),
                            Transaction_type = Convert.ToByte(reader[4])

                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reports;
        }

        public IEnumerable<Report> displayReportsById(int UserId)
        {
            List<Report> reports = new List<Report>();
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var displayReportsQuery = "SELECT  [Date], [Reason] ,[Amount], [Balance], [Transaction_type] FROM [dbo].[Report] where UserId = @UserId";
                SqlCommand command = new SqlCommand(displayReportsQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);                
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new Report
                        {
                            Date = Convert.ToDateTime(reader[0]),
                            Reason = reader[1].ToString(),
                            Amount = Convert.ToInt32(reader[2]),
                            Balance = Convert.ToInt32(reader[3]),
                            Transaction_type = Convert.ToByte(reader[4])

                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reports;
        }
        public IEnumerable<Report> searchReportByAllAccounts(int UserId, string search)
        {
            List<Report> searchReportByAllAccounts = new List<Report>();
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var searchReportByAllAccountsQuery = "SELECT  [Date], [Reason] ,[Amount], [Balance], [Transaction_type] FROM [dbo].[Report] where UserId = @UserId and Reason like '%' + @search + '%'";
                SqlCommand command = new SqlCommand(searchReportByAllAccountsQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@search", search);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        searchReportByAllAccounts.Add(new Report
                        {
                            Date = Convert.ToDateTime(reader[0]),
                            Reason = reader[1].ToString(),
                            Amount = Convert.ToInt32(reader[2]),
                            Balance = Convert.ToInt32(reader[3]),
                            Transaction_type = Convert.ToByte(reader[4])

                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return searchReportByAllAccounts;
        }
        public IEnumerable<Report> searchReportByAccount(int UserId, string search, int BankId)
        {
            List<Report> searchReportByAccount = new List<Report>();
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var searchReportByAccountQuery = "SELECT  [Date], [Reason] ,[Amount], [Balance], [Transaction_type] FROM [dbo].[Report] where UserId = @UserId and User_BankId = @BankId and Reason like '%' + @search + '%'";
                SqlCommand command = new SqlCommand(searchReportByAccountQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@BankId", BankId);
                command.Parameters.AddWithValue("@search", search);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        searchReportByAccount.Add(new Report
                        {
                            Date = Convert.ToDateTime(reader[0]),
                            Reason = reader[1].ToString(),
                            Amount = Convert.ToInt32(reader[2]),
                            Balance = Convert.ToInt32(reader[3]),
                            Transaction_type = Convert.ToByte(reader[4])

                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return searchReportByAccount;
        }
    }
}

