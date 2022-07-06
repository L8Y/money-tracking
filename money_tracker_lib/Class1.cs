using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace money_tracker_lib
{
    public class Class1
    {
        public object DisplayStudents()
        {
            SqlConnection con = new SqlConnection("Integrated security=sspi;database=money_management;server=DESKTOP-LK2QAA8\\SQLEXPRESS");
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Register", con);

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
            con.Close();

        }
    }
}
