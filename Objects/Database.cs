using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Stylist_Clients
{
  public class DB
  {
    public static SqlConnection Connection()
    {
      SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
      return conn;
    }
  }
}
