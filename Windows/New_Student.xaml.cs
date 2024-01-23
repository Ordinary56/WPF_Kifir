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
        Regex? _regex;
        // Az összes mező helyességét bitekbek tároljuk el
        // Ha az összes bit 1, akkor mindengyik helyes
        // Ha nem, akkor a diák felvétel nem fog működni
        // BitWise műveletekkel több teljesítményt érhetünk el
        byte _flags;
        public New_Student(StudentStore store)
        {
            _store = store;
            //lehet 0 is, de így sokkal olvashatóbb
            _flags = 0b0000;
            InitializeComponent();
        }
        void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (!(_flags == 0b1111))
            {
                MessageBox.Show("Valamelyik megadott mező helytelen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    _flags = (byte)(_regex.IsMatch(tb.Text) ? (_flags | 1) : (_flags & ~1));
                    break;
                case 'E':
                    _regex = Email();
                    _flags = (byte)(_regex.IsMatch(tb.Text) ? (_flags | (1 << 1)) : (_flags & ~(1 << 1)));
                    break;
                case 'M':
                    _regex = Points();
                    _flags = (byte)(_regex.IsMatch(tb.Text) ? (_flags | (1 << 2)) : _flags & ~(1 << 2));
                    break;
                case 'H':
                    _regex = Points();
                    _flags = (byte)(_regex.IsMatch(tb.Text) ? (_flags | (1 << 3)) : _flags & ~(1 << 3));
                    break;
                default:
                    break;
            }
        }
        [GeneratedRegex(@"^7255\d{7}$", RegexOptions.Multiline)]
        private partial Regex OM_Regex();

        [GeneratedRegex(@"^(?:[0-9]|[0-4][0-9]|50)$", RegexOptions.Multiline)]
        private partial Regex Points();
        [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Multiline)]
        private partial Regex Email();
    }
}
