﻿using System;
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

namespace Wpf_test
{
    /// <summary>
    /// Логика взаимодействия для ManuallyInpunWindow.xaml
    /// </summary>
    public partial class ManuallyInputWindow : Window
    {
        //private readonly SettingsParametrs _settingsParametrs;
        public ManuallyInputWindow()
        {
            InitializeComponent();
            //this._settingsParametrs = settingsParametrs;
            //TaskManagerClass.InitializeTaskArray();
        }

        public void FillTaskValue()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var task = new TaskClass
            {
                TimeToStart = Convert.ToInt16(this.TimeToStartTextBox.Text),
                TimeToWork = Convert.ToInt16(this.TimeToWorkTextBox.Text),
                Importance = Convert.ToInt16(this.ImportanceTextBox.Text)
            };

            TaskManagerClass.ListTasks.Add(task);
            //var manuallyInpunWindow = new ManuallyInputWindow();
           // manuallyInpunWindow.Show();
            this.Close();
        }        
    }
}