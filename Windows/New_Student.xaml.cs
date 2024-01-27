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
using System.Windows.Xps.Packaging;
using WPF_Kifir.Interfaces;
using WPF_Kifir.Model;
using WPF_Kifir.Repositories;
using WPF_Kifir.Store;

namespace WPF_Kifir.Windows
{
    /// <summary>
    /// Interaction logic for New_Student.xaml
    /// </summary>
    public partial class New_Student : Window
    {
        readonly Mediator _store;
        Regex? _regex;
        // Az összes mező helyességét bitekbek tároljuk el
        // Ha az összes bit 1, akkor mindengyik helyes
        // Ha nem, akkor a diák felvétel nem fog működni
        // BitWise műveletekkel több teljesítményt érhetünk el
        byte _flags;
        public New_Student(Mediator store)
        {
            _store = store;
            //lehet 0 is, de így sokkal olvashatóbb
            _flags = 0b0_0_0_0_0_0;
            InitializeComponent();
            store.ObjectSent += (sender, obj) =>
            {
                if (sender != this)
                {
                    HandleStudent(obj as Student);
                }
            };
        }

        private void HandleStudent(Student? student)
        {
            if (student == null) return;
            txt_OMid.Text = student.OM_Azonosito.ToString();
            txt_OMid.IsEnabled = false;
            txt_Name.Text = student.Neve;
            txt_Address.Text = student.ErtesitesiCime;
            dp_DOB.Text = student.SzuletesiDatum.ToString();
            txt_Email.Text = student.Email;
            txt_Maths.Text = student.Matematika.ToString();
            txt_Hungarian.Text = student.Magyar.ToString();
            Add.Content = "Diák Módosítása";
            this.Title = "Diák módosítása";
        }

        void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (!(_flags == 0b1_1_1_1_1_1))
            {
                MessageBox.Show("Valamelyik megadott mező helytelen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Student newStudent = new(txt_OMid.Text,
                    txt_Name.Text,
                    txt_Address.Text,
                    DateTime.Parse(dp_DOB.Text),
                    txt_Email.Text,
                    int.Parse(txt_Maths.Text),
                    int.Parse(txt_Hungarian.Text));
                _store.SendMessage(this, newStudent);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Hiba, Ilyen tanuló Ezzel az OM azonosítóval az adatbázisban már létezik!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        void Quit(object sender, RoutedEventArgs e) => Close();
        void TextChanged(object? sender, TextChangedEventArgs e)
        {
            TextBox tb = (sender as TextBox)!;
            if (tb == null) return;
            switch (tb.Name.Split('_')[1][0])
            {
                // Abban az esetben, hogy a kiválasztott textbox regexje igaz, akkor a _flags bitjék 1-re váltjuk
                // Az átváltás így néz ki
                // 1 << x -> x-szer toljuk balra az 1-et
                // _flags | (1 << x) -> OR művelet, így ezáltal 1 (igaz) lesz az értéke
                // ~( 1 << x) -> x-szer toljuk balra az 1-et és vegyük a komplemensét (az összes 0 bit 1 lesz és így fordítva)
                // _flags & ~(1 << x) -> És művelettel azt az 1 bitet 0-ra állítjuk
                case 'O':
                    _regex = OM_Regex();
                    ChangeBit(_regex.IsMatch(tb.Text), 0);
                    DisplayErrorMessage(tb.Name.Split('_')[1], 0);
                    break;
                case 'N':
                    ChangeBit(!string.IsNullOrEmpty(tb.Text), 1);
                    DisplayErrorMessage(tb.Name.Split('_')[1], 1);
                    break;
                case 'A':
                    ChangeBit(!string.IsNullOrEmpty(tb.Text), 2);
                    DisplayErrorMessage(tb.Name.Split('_')[1], 2);
                    break;
                case 'E':
                    _regex = Email();
                    ChangeBit(_regex.IsMatch(tb.Text), 3);
                    DisplayErrorMessage(tb.Name.Split('_')[1], 3);
                    break;
                case 'M':
                    _regex = Points();
                    ChangeBit(_regex.IsMatch(tb.Text), 4);
                    DisplayErrorMessage(tb.Name.Split('_')[1], 4);
                    break;
                case 'H':
                    _regex = Points();
                    ChangeBit(_regex.IsMatch(tb.Text), 5);
                    DisplayErrorMessage(tb.Name.Split('_')[1], 5);
                    break;
                default:
                    break;
            }
        }

        void Drag(object sender, MouseButtonEventArgs e) => DragMove();
        void DisplayErrorMessage(string labelname, byte nth_bit)
        {
            TextBlock? TargetBlock = FindName($"tbl_{labelname}") as TextBlock;
            if (TargetBlock == null) return;
            TargetBlock.Visibility = (byte)((_flags >> nth_bit) & 1) == 1 ? Visibility.Collapsed : Visibility.Visible;

        }
        void ChangeBit(bool predicate, byte nth_bit)
        {
            _flags = (byte)(predicate ? _flags | (1 << nth_bit) : _flags & ~(1 << nth_bit));
        }
        #region Regex
        [GeneratedRegex(@"[A-Z]\w+\s[A-Z]\w+")]
        private partial Regex Name_Regex();
        [GeneratedRegex(@"^7255\d{7}$", RegexOptions.Multiline)]
        private partial Regex OM_Regex();

        [GeneratedRegex(@"^(?:[0-9]|[0-4][0-9]|50)$", RegexOptions.Multiline)]
        private partial Regex Points();
        [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Multiline)]
        private partial Regex Email();
        #endregion
    }
}
