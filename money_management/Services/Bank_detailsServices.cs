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
            int isAccountCreated = 0;
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var addAccountquery = "insert into [dbo].[Bank_details] values(@Account_Name, @Initial_Balance, @UserId)";
                SqlCommand command = new SqlCommand(addAccountquery, connection);
                command.Parameters.AddWithValue("@Account_Name", Account_Name);
                command.Parameters.AddWithValue("@Initial_Balance", Initial_Balance);
                command.Parameters.AddWithValue("@UserId", UserId);
                try
                {
                    connection.Open();
                    isAccountCreated = command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return isAccountCreated;
        }

        public IEnumerable<Bank_details> getAccounts(int UserId)
        {
            List<Bank_details> accounts = new List<Bank_details>();
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var queryString = "SELECT  [User_BankId],[Account_Name] ,[Initial_Balance] FROM [dbo].[Bank_details] where UserId = @UserId";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
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
            int isBalanceUpdated = 0;
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var updateBalanceQuery = "UPDATE [dbo].[Bank_details] SET Initial_Balance = @balance where UserId = @userId and User_BankId = @user_bankid";
                SqlCommand command = new SqlCommand(updateBalanceQuery, connection);
                command.Parameters.AddWithValue("@balance", balance);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@user_bankid", user_bankid);
                try
                {
                    connection.Open();
                    isBalanceUpdated = command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return isBalanceUpdated;
        }

        public bool isBankAccountUnique(string Account_Name, int userId)
        {
            bool isBankAccountUnique = false;
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var isBankAccountUniquequery = "select count(Account_Name) from [dbo].[Bank_details] where [Account_Name]=@Account_Name and [UserId]=@userId";
                SqlCommand command = new SqlCommand(isBankAccountUniquequery, connection);
                command.Parameters.AddWithValue("@Account_Name", Account_Name);
                command.Parameters.AddWithValue("@userId", userId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader[0]) == 0)
                        {
                            isBankAccountUnique = true;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return isBankAccountUnique;
        }

        public IEnumerable<Bank_details> getCurrentBalance(int UserId, int User_BankId)
        {
            List<Bank_details> currentAccountBalance = new List<Bank_details>();
            using (SqlConnection connection = new SqlConnection(Constr))
            {
                var getCurrentBalanceQuery = "SELECT [Initial_Balance] FROM [dbo].[Bank_details] where UserId = @UserId and User_BankId = @User_BankId";
                SqlCommand command = new SqlCommand(getCurrentBalanceQuery, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@User_BankId", User_BankId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        currentAccountBalance.Add(new Bank_details
                        {
                            Initial_Balance = reader[0].ToString()
                        });

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return currentAccountBalance;
        }
    }
}







