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
        private readonly string _connectionstring = "datasource=127.0.01;port=3306;database=kifir;username=root;password=";
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionstring);
        }

        protected string[] GetEntireRow(DbDataReader reader)
        {
            // Oszlopok száma
            int rowcount = reader.FieldCount;

            string[] values = new string[rowcount];
            for(int i = 0; i < rowcount; i++)
            {
                values[i] = reader.GetString(i);
            }
            return values;
        }
    }
}
