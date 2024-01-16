using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Kifir.Store;

namespace WPF_Kifir.Windows
{
    /// <summary>
    /// Interaction logic for New_Student.xaml
    /// </summary>
    public partial class New_Student : Window
    {
        readonly StudentStore _store;
        public New_Student(StudentStore store)
        {
            _store = store;
            InitializeComponent();
        }
        void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            _store.GetStudent(new Student(
                txt_OMid.Text,
                txt_Name.Text,
                txt_Address.Text,
                DateTime.Parse(date_Day_of_birth.Text),
                txt_Email.Text,
                int.Parse(txt_Maths.Text),
                int.Parse(txt_Hungarian.Text)));

            this.Close();
        }
        void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close(); 
    }
}
