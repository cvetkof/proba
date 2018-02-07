using System;
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
    /// Логика взаимодействия для SheduleWindow.xaml
    /// </summary>
    public partial class SheduleWindow : Window
    {

        private TaskClass _min = new TaskClass(); //в min хранится первая задача с которой пересекается очередная "пришедшая" задача
        //private ParametersWindow _parametersWindow;
  
        public SheduleWindow(SettingsParametrs settingsParametrs, int sumWFirst)
        {
            InitializeComponent();
            FillSettingsValues(settingsParametrs);
            OutputResultListTasks(settingsParametrs);
            ProcResult(settingsParametrs);
            OutputW(SumWSecod(), sumWFirst);
            Checking(settingsParametrs);
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
        }

        /// <summary>
        /// вывод результирующего списка
        /// </summary>
        public void OutputResultListTasks(SettingsParametrs settingsParametrs)
        {
            for (int task = 0; task < TaskManagerClass.ResultListTasks.Count; task++)
            {
                ResultTasksListTextBox.AppendText((task + 1) + "\t" + Convert.ToString(TaskManagerClass.ResultListTasks[task].TimeToStart) + "\t" +
                    Convert.ToString(TaskManagerClass.ResultListTasks[task].TimeToWork) + "\t" + Convert.ToString(TaskManagerClass.ResultListTasks[task].Importance) + "\t" +
                    Convert.ToString(TaskManagerClass.ResultListTasks[task].NumberProc) + "\t" + Convert.ToString(TaskManagerClass.ResultListTasks[task].IndexNumber));
                ResultTasksListTextBox.AppendText("\n");
            }

            double count = (Convert.ToDouble(TaskManagerClass.ResultListTasks.Count) / Convert.ToDouble(settingsParametrs.TaskCounts))*100;
            var percent = Math.Round((count),2);
            ResultTasksListTextBox.AppendText("\n\nКоличество задач выставленных на обработку = " + TaskManagerClass.ResultListTasks.Count + " (" + percent + "%)" + "\n\n");

        }

        /// <summary>
        /// какое количество задач на каждом процессоре
        /// </summary>
        /// <param name="settingsParametrs"></param>
        public void ProcResult(SettingsParametrs settingsParametrs)
        {
            int count1 = 0;
            var count2 = settingsParametrs.DirectTime;
            for (int count = 0; count < settingsParametrs.ProcCount; count++)
            {
                for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if ((count+1) == TaskManagerClass.ResultListTasks[i].NumberProc)
                        count1++;
                    if((count+1) == TaskManagerClass.ResultListTasks[i].NumberProc)
                        count2 -= TaskManagerClass.ResultListTasks[i].TimeToWork;
                }

                var percent = Math.Round(((count2 / settingsParametrs.DirectTime) * 100),1); 
                ResultTasksListTextBox.AppendText((count+1) + "-ый процессор - " + (count1) + " задач; время простоя - " + count2 + " (" + percent + "%)\n");
                count1 = 0;
                count2 = settingsParametrs.DirectTime;
            }             

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
            ResultTasksListTextBox.AppendText("\nсуммарная важность всех задач - " + sumWFirsrt);
            ResultTasksListTextBox.AppendText("\nсуммарная важность задач выставленных на обработку - " + sumWSecond + "(" + Math.Round(percent,2) + "%)\n\n");
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
                    for (int i = j+1; i < TaskManagerClass.ResultListTasks.Count; i++)
                    {
                        if ((TaskManagerClass.ResultListTasks[i].NumberProc == k+1) && (TaskManagerClass.ResultListTasks[j].NumberProc == k+1))
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

            for(int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
            {
                if (TaskManagerClass.ResultListTasks[i].TimeToEnd > directTime)
                    directTime = -1;
                break;
            }

            if ((numbers.Count > 0) && (directTime == -1))
            {
                ResultTasksListTextBox.AppendText("\n\n\nРабота алгоритма НЕВЕРНА, имеются пересечения!");
                for (int i = 0; i < numbers.Count; i++)
                {
                    ResultTasksListTextBox.AppendText("\n\nПересекающиеся задачи:\n" + numbers[i].IndexNumber + "\n");
                }
            }
            else
            {
                ResultTasksListTextBox.AppendText("\n\nРабота алгоритма ВЕРНА, персечений нет");
            }
        }
    }
}