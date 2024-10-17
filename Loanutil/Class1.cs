using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
namespace Loanutil
{
    public class DBUtil
    {
        private static IConfigurationRoot configuration;
        static string s = null;
        static DBUtil()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C: \\Users\\ACER\\source\repos\\Loanutil\appsettings.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();



        }
        public static string ReturnCn(string key)
        {
            string connectionString = configuration.GetConnectionString(key);
            if (!string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }
            else
            {
                return null;
            }
        }
        public static SqlConnection GetDBConn()
        {
            string connectionString = "Data Source=DESKTOP-295VFEG\\SQLEXPRESS;Initial Catalog=Loan;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
