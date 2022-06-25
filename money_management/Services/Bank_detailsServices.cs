using money_management.Interface;
using money_management.Models;
using System.Data.SqlClient;
using System.Linq;

namespace money_management.Services
{
    public class Bank_detailsServices : IBank_detailsService
    {
        public string Constr { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public Bank_detailsServices(IConfiguration configuration)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DBConnection");
            con = new SqlConnection(Constr);
        }

        public int AddBankAccount(string Account_Name, string Initial_Balance, int UserId)
        {
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                var addAccountquery = "insert into [dbo].[Bank_details] values(@Account_Name, @Initial_Balance, @UserId)";
                SqlCommand command = new SqlCommand(addAccountquery, connection);
                command.Parameters.AddWithValue("@Account_Name", Account_Name);
                command.Parameters.AddWithValue("@Initial_Balance", Initial_Balance);
                command.Parameters.AddWithValue("@UserId", UserId);

                try
                {
                    connection.Open();
                    int isAccountCreated = command.ExecuteNonQuery();
                    return isAccountCreated;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            return 0;



        }

        public IEnumerable<Bank_details> getAccounts(int UserId)
        {
            List<Bank_details> accounts = new List<Bank_details>();
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                // Create the Command and Parameter objects.

                var queryString = "SELECT  [User_BankId],[Account_Name] ,[Initial_Balance] FROM [dbo].[Bank_details] where UserId = @UserId";
               
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@UserId", UserId);



                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        accounts.Add(new Bank_details
                        {
                            User_BankId = Convert.ToInt32(reader[0]),
                            Account_Name = reader[1].ToString(),
                            Initial_Balance = reader[2].ToString(),
                            
                        });

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return accounts;




        }

        public int updateInitialBalance(int userId, int user_bankid, int balance)
        {
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                var updateBalanceQuery = "UPDATE [dbo].[Bank_details] SET Initial_Balance = @balance where UserId = @userId and User_BankId = @user_bankid";
                SqlCommand command = new SqlCommand(updateBalanceQuery, connection);
                command.Parameters.AddWithValue("@balance", balance);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@user_bankid", user_bankid);

                try
                {
                    connection.Open();
                    int isBalanceUpdated = command.ExecuteNonQuery();
                    
                    connection.Close();
                    return isBalanceUpdated;
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







