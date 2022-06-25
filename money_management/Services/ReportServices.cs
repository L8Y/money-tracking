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
                /*var AddTransactionquery = "EXEC newTransaction @Date = @Date, @Reason = @Reason, @Amount = @Amount, @Balance = @Balance, @Transaction_type = @Transaction_type, @UserId = @UserId, @User_BankId = @User_BankId ;";*/
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
                    
                    connection.Close();
                    return isTransactionDone;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            return 0;



        }

        public IEnumerable<Report> displayReports(int UserId, int User_BankId)
        {
            List<Report> reports = new List<Report>();
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                // Create the Command and Parameter objects.
                var displayReportsQuery = "SELECT  [Date], [Reason] ,[Amount], [Balance], [Transaction_type] FROM [dbo].[Report] where UserId = @UserId and User_BankId = @User_BankId";
                SqlCommand command = new SqlCommand(displayReportsQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@User_BankId", User_BankId);


                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
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


    }
}

