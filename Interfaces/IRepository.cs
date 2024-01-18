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
        void Add(Student student);
        void Edit(Student student);
        void Delete(Student student);
        
    }
}
