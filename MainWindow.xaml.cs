using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Kifir.Interfaces;
using WPF_Kifir.Model;
using WPF_Kifir.Repositories;
using WPF_Kifir.Store;
using WPF_Kifir.Windows;
namespace WPF_Kifir
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ObservableCollection<IFelvetelizo> _students;
        readonly StudentStore _studentStore;
        readonly KifirRepository _repo;
        New_Student? _newStudent;
        public MainWindow(StudentStore store, KifirRepository repo)
        {
            InitializeComponent();
            _students = new();
            _studentStore = store;
            _studentStore.OnStudentCreated += HandleStudent;
            dg_Students.ItemsSource = _students;
            _repo = repo;
            // TODO: adatbázisból való betöltés (később)
            // Tipp:
            // Loaded += LoadFromDatabase
            // async Task LoadFromDataBase()
        }

        private void HandleStudent(Student? student)
        {
            if (student is null) return;
            _students.Add(student);
        }


        async void Button_Event(object sender, RoutedEventArgs e)
        {
            // Feltéve ha egy nagyokos máshoz kötné
            Button? btn = sender as Button;
            if (btn is null) return;
            switch (btn.Name[^1])
            {
                case '1':
                    _newStudent = new(_studentStore);
                    _newStudent.ShowDialog();
                    break;
                case '2':
                    if (dg_Students.SelectedIndex == -1) return;
                    _students.RemoveAt(dg_Students.SelectedIndex);
                    break;
                case '3':
                    await Import();
                    break;
                case '4':
                    await Export();
                    break;
                default:
                    break;
            }

        }

        async Task Export()
        {
            SaveFileDialog sfd = new()
            {
                Filter = "Comma Seperated Value | *.csv"
            };
            if ((bool)sfd.ShowDialog()!)
            {

                using StreamWriter sw = new(sfd.FileName);
                // Kell plusz mezőneveket írni különben az elsőt mindig kihagyja importnál
                await sw.WriteLineAsync("Om_Azonosito;Nev;Ertesitesi_Cim;Szuletesi_Datum;Email;Matek;Magyar");
                foreach (IFelvetelizo student in _students)
                {
                    await sw.WriteLineAsync(student.CSVSortAdVissza());
                }
            }
        }
        async Task LoadFromDatabase()
        {
            try
            {
                await foreach (Student? student in _repo.GetStudentsAsync())
                {
                    _students.Add(student!);
                }
            }
            catch (Exception ex)
            {
                #if DEBUG
                    Debug.WriteLine(ex.Message);
                #endif
                throw;
            }
        }

        async Task Import()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Comma Seperated Value (.csv) | *.csv"
            };
            if (ofd.ShowDialog() == true)
            {

                MessageBoxResult result = MessageBox.Show("Felülírjuk?", "Choice", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _students.Clear();
                }
                using StreamReader reader = new(ofd.FileName);
                reader.ReadLine()!.Skip(1);
                while (!reader.EndOfStream)
                {
                    string? line = await reader!.ReadLineAsync();
                    if (_students.Any(x => x.OM_Azonosito == line!.Split(';')[0])) continue;
                    _students.Add(new Student(
                        line!.Split(';')[0],
                        line.Split(';')[1],
                        line.Split(';')[2],
                        DateTime.Parse(line.Split(';')[3]),
                        line.Split(';')[4],
                        int.Parse(line.Split(';')[5]),
                        int.Parse(line.Split(';')[6])
                        ));
                }
            }
        }
    }
}
