using System.Data.SqlClient;

namespace HolidayDatabase
{
    class HolidayDbConnection
    {
        public HolidayDbConnection()
        {

        }

        public static SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\COB14\source\repos\HolidayDatabase\HolidayDatabase\bin\Debug\Travel.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
