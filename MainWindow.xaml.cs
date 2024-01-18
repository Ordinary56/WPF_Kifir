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
        ObservableCollection<IFelvetelizo> _students;
        StudentStore _studentStore;
        KifirRepository _repo;
        New_Student? _newStudent;
        public MainWindow(StudentStore store, KifirRepository repo)
        {
            InitializeComponent();
            _students = new();
            _studentStore = store;
            _studentStore.OnStudentCreated += HandleStudent;
            dg_Students.ItemsSource = _students;
            _repo = repo;
            // TODO: adatbázisból való betöltés
            // Tipp:
            // Loaded += LoadFromDatabase
            // async Task LoadFromDataBase()
            // AddRange-el fel lehet venni az értékeket :)
        }

        private void HandleStudent(Student? student)
        {
            if (student is null) return;
            _students.Add(student);
        }


        async void Button_Event(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button)!;
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
            SaveFileDialog sfd = new();
            sfd.Filter = "Comma Seperated Value | *.csv";
            if (sfd.ShowDialog() == true)
            {

                using (StreamWriter sw = new(sfd.FileName))
                {
                    foreach (Student student in _students)
                    {
                        await sw.WriteLineAsync(student.CSVSortAdVissza());
                    }

                }
            }
        }
        async Task LoadFromDatabase()
        {
            var result = await _repo.GetStudents();
            try
            {
                foreach(Student student in result)
                {
                    _students.Add(student);
                }
            }
            catch (Exception ex) 
            {
                #if DEBUG
                   Debug.WriteLine(ex.ToString());  
                #endif
                throw;
            }
        }

        async Task Import()
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "Comma Seperated Value (.csv) | *.csv";
            if (ofd.ShowDialog() == true)
            {

                MessageBoxResult result = MessageBox.Show("Felülírjuk?", "Choice", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _students.Clear();
                }
                using (StreamReader reader = new(ofd.FileName))
                {
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
}
