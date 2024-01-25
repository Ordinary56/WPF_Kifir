using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            if(_students.Any(x => x.OM_Azonosito == student.OM_Azonosito))
            {
                Student old = (_students.First(x => x.OM_Azonosito == student.OM_Azonosito) as Student)!;
                _students[_students.IndexOf(old!)] = student;
                return;
            }
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
                    _newStudent = new(_studentStore);
                    _studentStore.GetStudent(_students[dg_Students.SelectedIndex] as Student);
                    _newStudent.ShowDialog();
                    break;
                case '3':
                    if (dg_Students.SelectedIndex == -1) return;
                    _students.RemoveAt(dg_Students.SelectedIndex);
                    break;
                case '4':
                    await Import();
                    break;
                case '5':
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
                Filter = "Comma Seperated Value | *.csv | JavaScript Object Notation (JSON) | *.json"
            };
            if ((bool)sfd.ShowDialog()!)
            {
                B_3.IsEnabled = false;
                await SaveAs(sfd);
            }
            B_3.IsEnabled = true;
        }

        private async Task SaveAs(SaveFileDialog sfd)
        {
            switch (System.IO.Path.GetExtension(sfd.FileName))
            {
                case ".json":
                    JsonSerializerOptions opt = new()
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    string json_data = JsonSerializer.Serialize(_students, opt);
                    await File.WriteAllLinesAsync(sfd.FileName, json_data.Split('\n'));
                    break;
                case ".csv":
                    using (StreamWriter sw = new(sfd.FileName))
                    {
                        // Kell plusz mezőneveket írni különben az elsőt mindig kihagyja importnál
                        await sw.WriteLineAsync("Om_Azonosito;Nev;Ertesitesi_Cim;Szuletesi_Datum;Email;Matek;Magyar");
                        foreach (IFelvetelizo student in _students)
                        {
                            await sw.WriteLineAsync(student.CSVSortAdVissza());
                        }
                    }
                    break;
                default:
                    break;

            }
        }

        async Task Import()
        {

            OpenFileDialog ofd = new()
            {
                Filter = "Comma Seperated Value (.csv) | *.csv | JavaScript Object Notation (JSON) | *.json"
            };
            if ((bool)ofd.ShowDialog()!)
            {
                B_4.IsEnabled = false;
                MessageBoxResult result = MessageBox.Show("Felülírjuk?", "Choice", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes && _students.Count < 1)
                {
                    _students.Clear();
                }
                await OpenAs(ofd);
            }
            B_4.IsEnabled = true;
        }

        async Task OpenAs(OpenFileDialog ofd)
        {
            switch(System.IO.Path.GetExtension(ofd.FileName))
            {
                case ".json":
                    using (FileStream stream = File.OpenRead(ofd.FileName))
                    {
                        Student[]? json_data = await JsonSerializer.DeserializeAsync<Student[]>(stream);
                        foreach (Student student in json_data ?? Enumerable.Empty<Student>())
                        {
                            if (_students.Any(x => x.OM_Azonosito == student.OM_Azonosito) || student is null) continue;
                            _students.Add(student);
                        }
                    }
                    break;
                case ".csv":
                    using (StreamReader reader = new(ofd.FileName))
                    {
                        reader.ReadLine()!.Skip(1);
                        while (!reader.EndOfStream)
                        {
                            string line = await reader!.ReadLineAsync() ?? "";
                            if (_students.Any(x => x.OM_Azonosito == line!.Split(';')[0])) continue;
                            _students.Add(new Student(line!));
                        }
                    }
                    break;
            }
        }
    }
}
