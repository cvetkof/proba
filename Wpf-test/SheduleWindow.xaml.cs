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
using System.IO;

namespace Wpf_test
{
    /// <summary>
    /// Логика взаимодействия для SheduleWindow.xaml
    /// </summary>
    public partial class SheduleWindow : Window
    {

        private SettingsParametrs _settingsParametrs;

        private TaskClass _min = new TaskClass(); //в min хранится первая задача с которой пересекается очередная "пришедшая" задача
        //private ParametersWindow _parametersWindow;
  
        public SheduleWindow(SettingsParametrs settingsParametrs, int sumWFirst)
        {
            this._settingsParametrs = settingsParametrs;

            InitializeComponent();
            FillSettingsValues(settingsParametrs);

            ResultGrid.ItemsSource = TaskManagerClass.ResultListTasks;
            ResultGrid.ColumnWidth = DataGridLength.Auto;
            
            //OutputResultListTasks(settingsParametrs);

            ProcResult();

            OutputW(SumWSecod(), sumWFirst);

            //OutputToFile();

            //Checking(settingsParametrs);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void FillSettingsValues(SettingsParametrs settingsParametrs)
        {
            ProcCountTextBox.Text = settingsParametrs.ProcCount.ToString();
            DirectTimeTextBox.Text = settingsParametrs.DirectTime.ToString();
            TasksCountTextBox.Text = settingsParametrs.TaskCounts.ToString();
        }

        /// <summary>
        /// какое количество задач на каждом процессоре
        /// </summary>
        /// <param name="settingsParametrs"></param>
        public void ProcResult()
        {

            int count1 = 0;
            var count2 = this._settingsParametrs.DirectTime;
            int sumImportance = 0;

            for (int count = 0; count < this._settingsParametrs.ProcCount; count++)
            {
                for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if ((count + 1) == TaskManagerClass.ResultListTasks[i].NumberProc)
                        count1++;
                    if ((count + 1) == TaskManagerClass.ResultListTasks[i].NumberProc)
                        count2 -= TaskManagerClass.ResultListTasks[i].TimeToWork;
                    if ((count + 1) == TaskManagerClass.ResultListTasks[i].NumberProc)
                        sumImportance += TaskManagerClass.ResultListTasks[i].Importance;
                }

                var percent_proc = Math.Round(((count2 / this._settingsParametrs.DirectTime) * 100), 1);
                ResultTasksListTextBox.AppendText("  " + (count + 1) + "-ый процессор - " + (count1) + " задач     важность задач - " + sumImportance +"     время простоя - " + count2 + " (" + percent_proc + "%)\n");
                count1 = 0;
                count2 = this._settingsParametrs.DirectTime;
                sumImportance = 0;
            }

            double value = (Convert.ToDouble(TaskManagerClass.ResultListTasks.Count) / Convert.ToDouble(this._settingsParametrs.TaskCounts)) * 100;
            var percent_tasks = Math.Round((value), 2);
            ResultTasksListTextBox.AppendText("\n  Количество задач выставленных на обработку - " + TaskManagerClass.ResultListTasks.Count + " (" + percent_tasks + "%)");

        }

        /// <summary>
        /// сумма важности задач поступивших на обработку
        /// </summary>
        /// <returns></returns>
        public int SumWSecod()
        {
            int sumWSecod = 0;
            for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                sumWSecod += TaskManagerClass.ResultListTasks[i].Importance;
            return sumWSecod;

        }

        /// <summary>
        /// вывод результатов важности задач
        /// </summary>
        /// <param name="sumWSecond"> сумма важности задач поступивших на обработку </param>
        /// <param name="sumWFirsrt"> сумма важности всех задач </param>
        public void OutputW(int sumWSecond, int sumWFirsrt)
        {
            double percent = (Convert.ToDouble(sumWSecond) / Convert.ToDouble(sumWFirsrt)) * 100;
            ResultTasksListTextBox.AppendText("\n  Cуммарная важность всех задач - " + sumWFirsrt);
            ResultTasksListTextBox.AppendText("\n  Cуммарная важность задач выставленных на обработку - " + sumWSecond + "(" + Math.Round(percent, 2) + "%)\n\n");
        }

        /// <summary>
        /// проверка правильности результата
        /// </summary>
        public void Checking(SettingsParametrs settingsParametrs)
        {
            List<TaskClass> numbers = new List<TaskClass>();

            double directTime = settingsParametrs.DirectTime;

            for (int k = 0; k < settingsParametrs.ProcCount; k++) // цикол, количества процессоров
            {
                for (int j = 0; j < TaskManagerClass.ResultListTasks.Count; j++)
                {
                    for (int i = j + 1; i < TaskManagerClass.ResultListTasks.Count; i++)
                    {
                        if ((TaskManagerClass.ResultListTasks[i].NumberProc == k + 1) && (TaskManagerClass.ResultListTasks[j].NumberProc == k + 1))
                        {
                            bool intersectionRight = ((TaskManagerClass.ResultListTasks[j].TimeToStart < TaskManagerClass.ResultListTasks[i].TimeToEnd) &&
                                                     (TaskManagerClass.ResultListTasks[j].TimeToStart >= TaskManagerClass.ResultListTasks[i].TimeToStart));

                            bool intersectionLeft = ((TaskManagerClass.ResultListTasks[j].TimeToEnd <= TaskManagerClass.ResultListTasks[i].TimeToEnd) &&
                                                    (TaskManagerClass.ResultListTasks[j].TimeToEnd > TaskManagerClass.ResultListTasks[i].TimeToStart));

                            bool intersectionRightLeft = ((TaskManagerClass.ResultListTasks[j].TimeToStart < TaskManagerClass.ResultListTasks[i].TimeToStart) &&
                                                         (TaskManagerClass.ResultListTasks[j].TimeToEnd > TaskManagerClass.ResultListTasks[i].TimeToEnd));

                            if (intersectionLeft || intersectionRight || intersectionRightLeft)
                            {
                                numbers.Add(TaskManagerClass.ResultListTasks[j]);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
            {
                if (TaskManagerClass.ResultListTasks[i].TimeToEnd > directTime)
                    directTime = -1;
                break;
            }

            if ((numbers.Count > 0) && (directTime == -1))
            {
                ResultTasksListTextBox.AppendText("\n  Работа алгоритма НЕВЕРНА, имеются пересечения!");
                for (int i = 0; i < numbers.Count; i++)
                {
                    ResultTasksListTextBox.AppendText("\n\n  Пересекающиеся задачи:\n" + numbers[i].IndexNumber + "\n");
                }
            }
            else
            {
                ResultTasksListTextBox.AppendText("\n  Работа алгоритма ВЕРНА, персечений нет");
            }
        }

        private void Make_Grafic(object sender, RoutedEventArgs e)
        {
            var graficWindow = new GraficWindow(this._settingsParametrs);
            graficWindow.Show();
        }

        //public void OutputToFile()
        //{
        //    File.AppendAllText("d:\\Shedule.docx", "№\tвремя старта\tвремя обработки");
        //    File.AppendAllText("d:\\Shedule.docx", "\n");

        //    for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
        //    {
        //        File.AppendAllText("d:\\Shedule.docx", Convert.ToString(TaskManagerClass.ResultListTasks[i].IndexNumber) + "\t");
        //        File.AppendAllText("d:\\Shedule.docx", Convert.ToString(TaskManagerClass.ResultListTasks[i].TimeToStart) + "\t");
        //        File.AppendAllText("d:\\Shedule.docx", Convert.ToString(TaskManagerClass.ResultListTasks[i].TimeToWork) + "\n");
        //    }
        //}
    }
}