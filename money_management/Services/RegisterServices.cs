using money_management.Interface;
using money_management.Models;
using System.Data.SqlClient;
using System.Linq;

namespace money_management.Services
{
    public class RegisterServices : IRegisterService
    {
        public string Constr { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public RegisterServices(IConfiguration configuration)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DBConnection");
            con = new SqlConnection(Constr);
        }

        public IEnumerable<Register> RegisteredUser()
        {
            List<Register> users = new List<Register>();
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                // Create the Command and Parameter objects.
                var queryString = "SELECT  [Name],[Email] ,[Password],[UserId] FROM [dbo].[Register]";
                SqlCommand command = new SqlCommand(queryString, connection);


                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new Register
                        {
                            Name = reader[0].ToString(),
                            Email = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            UserId = Convert.ToInt32(reader[3])

                        });

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return users;




        }

        public int createUser(string Name, string EmailId, string Password)
        {
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                var addUserquery = "insert into [dbo].[Register] values(@Name, @Email, @Password)";
                SqlCommand command = new SqlCommand(addUserquery, connection);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Email", EmailId);
                command.Parameters.AddWithValue("@Password", Password);

                try
                {
                    connection.Open();
                    int isUserAdded = command.ExecuteNonQuery();
                    return isUserAdded;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            return 0;



        }

        public IEnumerable<Register> isValidUser(string email, string password)
        {
            List<Register> currentUser = new List<Register>();
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                var isValidUserquery = "select count(Email) from [dbo].[Register] where [Email]=@email and [Password]=@password";
                SqlCommand command = new SqlCommand(isValidUserquery, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        currentUser.Add(new Register
                        {

                            
                            count = Convert.ToInt32(reader[0])  

                        });

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return currentUser;
        }

        public IEnumerable<Register> GetUserId(string email)
        {
            List<Register> currentUserId = new List<Register>();
            using (SqlConnection connection =
           new SqlConnection(Constr))
            {
                var getUserIdQuery = "select [UserId] from [dbo].[Register] where [Email]=@email";
                SqlCommand command = new SqlCommand(getUserIdQuery, connection);
                command.Parameters.AddWithValue("@email", email);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        currentUserId.Add(new Register
                        {
                            
                            UserId = Convert.ToInt32(reader[0])

                        });

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return currentUserId;
            }

        }
            
    }
}
