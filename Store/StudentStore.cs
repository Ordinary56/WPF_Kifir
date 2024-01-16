using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Kifir.Store
{
    public class StudentStore
    {
        public event Action<Student?> OnStudentCreated;
        public void GetStudent(Student? stud)
        {
            OnStudentCreated?.Invoke(stud);
        }
    }
}
