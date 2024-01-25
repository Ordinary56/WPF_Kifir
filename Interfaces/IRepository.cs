using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Kifir.Model;

namespace WPF_Kifir.Interfaces
{
    public interface IRepository
    {
        //TODO: KifirRepository-nak ezt implemetálnia kell
        Task Add(Student student);
        Task Edit(Student student);
        Task Delete(Student student);
        

       
        
    }
}
