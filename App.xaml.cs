﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_Kifir.Store;
using WPF_Kifir.Windows;

namespace WPF_Kifir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            StudentStore store = new StudentStore();
            var window = new MainWindow(store);
            window.Show();
            base.OnStartup(e);
        }
    }
}
