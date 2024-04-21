using Microsoft.Data.SqlClient;

namespace TrainingFPTCo
{
    public class Database
    {
        public static string GetStrConnection()
        {
            string strConnection = @"Data Source=DESKTOP-5O3AH4B\SQLEXPRESS;Initial Catalog=TrainingFPT;Integrated Security=True;Trust Server Certificate=True";
            return strConnection;
        }

        public static SqlConnection GetSqlConnection() 
        {
            string strConnection = Database.GetStrConnection();
            SqlConnection conn = new SqlConnection(strConnection);
            return conn;
        }
    }
}
