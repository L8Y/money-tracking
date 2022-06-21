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
            using (SqlConnection connection =
           new SqlConnection(Constr))
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
            using (SqlConnection connection =
           new SqlConnection(Constr))
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
                    int isTransactionDone = command.ExecuteNonQuery();
                    return isTransactionDone;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            return 0;



        }
    }
}

