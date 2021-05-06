using System.Data.SqlClient;
using System.Runtime.InteropServices;
namespace zad2REST {
    class SQLConnection
    {
        public static SqlConnection GetDBConnection(string DBSource, string DB="", string userName="", string password="")
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = DBSource;
            builder.UserID = userName;
            builder.Password = password;
            builder.InitialCatalog = DB;
            builder.IntegratedSecurity = true;
            SqlConnection sQLConnection = new SqlConnection(builder.ConnectionString);
            return sQLConnection;
        }
    }
}
