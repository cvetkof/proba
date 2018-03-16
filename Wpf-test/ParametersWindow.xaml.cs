using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
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
    /// Логика взаимодействия для ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        private SettingsParametrs _settingsParametrs;

        public ParametersWindow(SettingsParametrs settingsParametrs)
        {
            this._settingsParametrs = settingsParametrs;
            InitializeComponent();
            FillSettingsValues();            
        }

        private void FillSettingsValues()
        {
            ProcCountTextBox.Text = this._settingsParametrs.ProcCount.ToString();
            DirectTimeTextBox.Text = this._settingsParametrs.DirectTime.ToString();
            TasksCountTextBox.Text = this._settingsParametrs.TaskCounts.ToString();

            GenerateTable();
        }

        /// <summary>
        /// установка параметров задач и занесение в таблицу
        /// </summary>
        /// <param name="settingsParametrs"></param>
        private void GenerateTable()
        {
            if (this._settingsParametrs.InputType == Enums.InputType.Random)
            {
                Random rand = new Random();

                TaskManagerClass.InitializeListTasks();

                for (int count = 0; count < this._settingsParametrs.TaskCounts; count++)
                {
                    var task = new TaskClass
                    {
                        Mathematic = rand.Next(1,50000),
                        Dispr = rand.Next(1,50000),
                        TimeToWork = rand.Next(1, 420), // значение от 1c до 7мин
                        Importance = rand.Next(1, 100), // значение от 1 до 100
                        IndexNumber = count + 1,
                        Guid = Guid.NewGuid()
                    };

                    task.Mathematic = task.Mathematic / 10; // сведение к интервалу (0.1; 5000)
                    task.Dispr = task.Dispr / 10; // сведение к интервалу (0.1; 5000)

                    TaskManagerClass.ListTasks.Add(task);
                }

            ParametersWindowGrid.ItemsSource = TaskManagerClass.ListTasks;
            ParametersWindowGrid.ColumnWidth = DataGridLength.Auto;

            }
            else
            {
                ParametersWindowGrid.ItemsSource = TaskManagerClass.ListTasks;
                ParametersWindowGrid.ColumnWidth = DataGridLength.Auto;
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();            
        }

        private void SetStartTime(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    this.pBar.Maximum = this._settingsParametrs.TaskCounts;
                    this.pBar.Value = 0;
                });

                SetTimeToStart();


            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    var fullParametersWindow = new FullParametrsWindow(this._settingsParametrs);
                    fullParametersWindow.Show();
                    this.Close();
                });                
            });
        }

        /// <summary>
        /// расчет начального времени
        /// </summary>
        public void SetTimeToStart()
        {
            for (int i = 0; i < TaskManagerClass.ListTasks.Count; i++)
            {
                double dispr = TaskManagerClass.ListTasks[i].Dispr;
                double math = TaskManagerClass.ListTasks[i].Mathematic;

                double c1 = 100000, c2 = 0.05;
                double c, a, timeToStart;
                Random rand = new Random();

                double secondMoment = dispr + Math.Pow(math, 2);
                double e = 1e-7;
                double E = secondMoment / Math.Pow(math, 2);
                double middleC, middleK = 100;

                while (Math.Abs(middleK - E) > e)
                {
                    middleC = (c1 + c2) / 2;

                    middleK = Gamma((2 / middleC) + 1) / Math.Pow(Gamma((1 / middleC) + 1), 2);

                    if (middleK <= E)
                    {
                        c1 = middleC;
                    }
                    else
                    {
                        c2 = middleC;
                    };

                    System.Threading.Thread.Sleep(rand.Next(1,10));
                }
                
            
                c = (c1 + c2) / 2;
                a = math / (Gamma((1 / c) + 1)); 

                timeToStart = a * Math.Pow(Math.Log(10), (1 / c));
                TaskManagerClass.ListTasks[i].TimeToStart = Math.Round(timeToStart, 2);

                Dispatcher.Invoke(() =>
                {
                    this.pBar.Value++;
                });
            }
        }

        /// <summary>
        /// расчет гамма функции
        /// </summary>
        /// <param name="c"> параметр функции </param>
        /// <returns></returns>
        public double Gamma(double c)

        {
            double x = c - 1;

            double c0 = 2.5066282746310005;
            double c1 = 1.0000000000190015;
            double c2 = 76.18009172947146;
            double c3 = -86.50532032941677;
            double c4 = 24.01409824083091;
            double c5 = -1.231739572450155;
            double c6 = 1.208650973866179e-3;
            double c7 = -5.395239384953e-6;

            double gamma2 = (x + 0.5) * Math.Log(x + 5.5) - (x + 5.5) + Math.Log(c0 * (c1 + c2/(x + 1) + c3/(x + 2) +
                c4/(x + 3) + c5/(x + 4) + c6/(x + 5) + c7/(x + 6)));

            gamma2 = Math.Exp(gamma2);

            return gamma2;
        }

    }
}
