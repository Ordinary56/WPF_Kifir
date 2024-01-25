using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace WPF_Kifir.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionstring = "datasource=127.0.01;port=3306;database=minikifir;username=root;password=";
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionstring);
        }

        protected static string[] GetEntireRow(DbDataReader reader)
        {
            // Oszlopok száma
            int columncount = reader.FieldCount;
            string[] values = new string[columncount];
            for(int i = 0; i < columncount; i++)
            {
                values[i] = reader.GetValue(i).ToString();
            }
            return values;
        }
    }
}
