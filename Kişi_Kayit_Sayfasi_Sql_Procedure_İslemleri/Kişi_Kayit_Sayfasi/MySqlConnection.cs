using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Kisi_Kayit_Sql_Procedure
{
    public class MySqlConnection
    {
        public SqlConnection Connection()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString);
            connection.Open();
            return connection;
        }
    }

}
