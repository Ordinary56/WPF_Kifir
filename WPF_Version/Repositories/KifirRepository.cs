using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WPF_Kifir.Interfaces;
using WPF_Kifir.Model;

namespace WPF_Kifir.Repositories
{
    public class KifirRepository : RepositoryBase, IRepository
    {
        public async Task Add(Student student)
        {
            try
            {
                using MySqlConnection connection = GetConnection();
                await connection.OpenAsync();
                using MySqlCommand command = new("INSERT INTO felvetelizok VALUES (" +
                    "@OM_Azon,@Nev,@Ertesitesi_Cim,@Szul,@Email,@Matek,@Magyar)", connection);
                command.Parameters.AddWithValue("@OM_Azon", student.OM_Azonosito);
                command.Parameters.AddWithValue("@Nev", student.Neve);
                command.Parameters.AddWithValue("@Ertesitesi_Cim", student.ErtesitesiCime);
                command.Parameters.AddWithValue("@Szul", student.SzuletesiDatum);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@Matek", student.Matematika);
                command.Parameters.AddWithValue("@Magyar", student.Magyar);
                await command.PrepareAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task Delete(Student student)
        {
            try
            {
                using MySqlConnection connection = GetConnection();
                await connection.OpenAsync();
                using MySqlCommand cmd = new($"DELETE FROM felvetelizok WHERE OM_Azon={student.OM_Azonosito}", connection);
                await cmd.PrepareAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch(Exception)
            {
                return;
            }
        }

        public async Task Edit(Student student)
        {
            using MySqlConnection connection = GetConnection();
            await connection.OpenAsync();
            using MySqlCommand command = new("UPDATE felvetelizok SET Neve=@Nev, Ertesitesi_Cim=@Cim, Szul_Datum=@Datum, Email=@Email, " +
                " Matek=@Matek, Magyar=@Magyar WHERE OM_Azon=@OM_Azon", connection);
            command.Parameters.AddWithValue("@Nev",student.Neve);
            command.Parameters.AddWithValue("@Cim",student.ErtesitesiCime);
            command.Parameters.AddWithValue("@Datum",student.SzuletesiDatum);
            command.Parameters.AddWithValue("@Email",student.Email);
            command.Parameters.AddWithValue("@Matek",student.Matematika);
            command.Parameters.AddWithValue("@Magyar",student.Magyar);
            command.Parameters.AddWithValue("@OM_Azon",student.OM_Azonosito);
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();

        }

        public async Task<List<Student>?> GetStudentsAsync()
        {
            List<Student>? result = new();
            using MySqlConnection connection = GetConnection();
            try
            {
                await connection.OpenAsync();
                using MySqlCommand cmd = new("SELECT * FROM felvetelizok", connection);
                using System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    //Ennek létezhez egyszerűbb módja, mint hogy Reader.GetString, GetInt16 és hasonló
                    string[] fields = GetEntireRow(reader);
                    Debug.WriteLine(fields[3]);
                    result.Add(new Student(
                        fields[0],
                        fields[1],
                        fields[2],
                        DateTime.Parse(fields[3]),
                        fields[4],
                        int.Parse(fields[5]),
                        int.Parse(fields[^1])));
                }
                await reader.CloseAsync();
                return result;
            }
            catch(Exception)
            {
                return result;
            }
        }


    }
}
