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

        public async Task<List<Student>> GetStudents()
        {
            List<Student> result = new();
            using (MySqlConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand cmd = new("SELECT * FROM kifir"))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            //Ennek létezhez egyszerűbb módja, mint hogy Reader.GetString, GetInt16 és hasonló
                            string[] fields = GetEntireRow(reader);
                            result.Add(new Student(
                                fields[0],
                                fields[1],
                                fields[2],
                                DateTime.Parse(fields[3]),
                                fields[4],
                                int.Parse(fields[5]),
                                int.Parse(fields[^1])
                                    )
                                );
                        }
                    }
                }
            }
            return result;
        }

    }
}
