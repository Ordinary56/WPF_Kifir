using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Kifir.Model;

namespace WPF_Kifir.Repositories
{
    public class KifirRepository : RepositoryBase
    {


        public async IAsyncEnumerable<Student?> GetStudentsAsync()
        {
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand cmd = new("SELECT * FROM kifir", connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            //Ennek létezhez egyszerűbb módja, mint hogy Reader.GetString, GetInt16 és hasonló
                            string[] fields = GetEntireRow(reader);
                            yield return new Student(
                                fields[0],
                                fields[1],
                                fields[2],
                                DateTime.Parse(fields[3]),
                                fields[4],
                                int.Parse(fields[5]),
                                int.Parse(fields[^1]));
                        }
                        await reader.CloseAsync();
                    }
                }
            }
        }

    }
}
