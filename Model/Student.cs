using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Kifir.Interfaces;
namespace WPF_Kifir.Model
{
    public class Student : IFelvetelizo
    {
        public Student(string csvString)
        {
            ModositCSVSorral(csvString);
        }
        public Student(string oM_Azonosito, string neve, string ertesitesiCime,  DateTime szuletesiDatum, string email, int matematika, int magyar)
        {
            OM_Azonosito = oM_Azonosito;
            Neve = neve;
            ErtesitesiCime = ertesitesiCime;
            Email = email;
            SzuletesiDatum = szuletesiDatum;
            Matematika = matematika;
            Magyar = magyar;
        }

        public string OM_Azonosito { get;  set; }
        public string Neve { get;  set; } 
        public string ErtesitesiCime { get; set; }
        public string Email { get; set; }
        public DateTime SzuletesiDatum { get; set; }
        public int Matematika { get; set; }
        public int Magyar { get; set; }

        public string CSVSortAdVissza()
        {
            return $"{this.OM_Azonosito};{this.Neve};{this.ErtesitesiCime};{this.SzuletesiDatum.ToShortDateString()};{this.Email};" +
                $"{this.Matematika};{this.Magyar}";
        }

        public void ModositCSVSorral(string csvString)
        {
            string[] strings = csvString.Split(';');
            OM_Azonosito = strings[0];
            Neve = strings[1];
            ErtesitesiCime = strings[2];
            Email = strings[4];
            SzuletesiDatum = DateTime.Parse(strings[3]);
            Matematika = int.Parse(strings[5]);
            Magyar = int.Parse(strings[6]);

        }
       


    }
}
