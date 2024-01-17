using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Kifir.Model;
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
            if(!IsInputValid())
            {
                MessageBox.Show("Valamelyik megadott mező helytelen","Error", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            _store.GetStudent(new Student(
                txt_OMid.Text,
                txt_Name.Text,
                txt_Address.Text,
                DateTime.Parse(dp_DOB.Text),
                txt_Email.Text,
                int.Parse(txt_Maths.Text),
                int.Parse(txt_Hungarian.Text)));

            this.Close();
        }
        void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close(); 

        bool IsInputValid()
        {
           return new Regex(@"7255\d{7}",RegexOptions.Compiled | RegexOptions.Multiline).IsMatch(txt_OMid.Text) &&
                int.Parse(txt_Hungarian.Text) >= 0 && int.Parse(txt_Hungarian.Text) <= 50 &&
                 int.Parse(txt_Maths.Text) >= 0 && int.Parse(txt_Maths.Text) <= 50;           
        }
    }
}
