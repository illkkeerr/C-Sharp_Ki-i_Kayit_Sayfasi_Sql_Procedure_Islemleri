using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Kişi_Kayit_Sayfasi_Sql_Procedure_İslemleri
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
