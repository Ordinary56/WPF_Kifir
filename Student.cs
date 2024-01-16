using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Kifir
{
    public record Student
    {
        public Student(string oM_Azon, string name, string cim, DateTime dOBirth, string email, int math_Points, int hung_Points)
        {
            OM_Azon = oM_Azon;
            Name = name;
            Cim = cim;
            DOBirth = dOBirth;
            Email = email;
            Math_Points = math_Points;
            Hung_Points = hung_Points;
        }

        public string OM_Azon { get; set; }
        public string Name { get;set; }
        public string Cim { get; set; }
        public DateTime DOBirth { get; set; }
        public string Email { get; set; }
        public int Math_Points { get; set; }
        public int Hung_Points { get; set; }
        

    }
}
