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
  
        public SheduleWindow(SettingsParametrs settingsParametrs)
        {
            InitializeComponent();
            FillSettingsValues(settingsParametrs);
            OutputResultListTasks();

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
        }

        /// <summary>
        /// вывод результирующего списка
        /// </summary>
        public void OutputResultListTasks()
        {
            ResultTasksListTextBox.AppendText("Количество задач на обработку = " + TaskManagerClass.ResultListTasks.Count + "\n\n");

            for (int task = 0; task < TaskManagerClass.ResultListTasks.Count; task++)
            {
                ResultTasksListTextBox.AppendText("Время поступления " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].TimeToStart + "\n");
                ResultTasksListTextBox.AppendText("Время обработки  " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].TimeToWork + "\n");
                ResultTasksListTextBox.AppendText("Важность " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].Importance + "\n");
                ResultTasksListTextBox.AppendText("Порядковый номер " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].IndexNumber + "\n");
                ResultTasksListTextBox.AppendText("\n");
            }
        }

        /// <summary>
        /// проверка правильности результата
        /// </summary>
        public void Checking(SettingsParametrs settingsParametrs)
        {
            List<TaskClass> numbers = new List<TaskClass>();

            int a = 0;
            double directTime = settingsParametrs.DirectTime;

            for (int j = 0; j < TaskManagerClass.ResultListTasks.Count; j++)
            {
                for (int i = a; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if (i != j)
                    {
                        bool intersectionRight = ((TaskManagerClass.ResultListTasks[j].TimeToStart < TaskManagerClass.ResultListTasks[i].TimeToEnd) &&
                                                 (TaskManagerClass.ResultListTasks[j].TimeToStart >= TaskManagerClass.ResultListTasks[i].TimeToStart));

                        bool intersectionLeft = ((TaskManagerClass.ResultListTasks[j].TimeToEnd <= TaskManagerClass.ResultListTasks[i].TimeToEnd) &&
                                                (TaskManagerClass.ResultListTasks[j].TimeToEnd > TaskManagerClass.ResultListTasks[i].TimeToStart));

                        bool intersectionRightLeft = ((TaskManagerClass.ResultListTasks[j].TimeToStart < TaskManagerClass.ResultListTasks[i].TimeToStart) &&
                                                     (TaskManagerClass.ResultListTasks[j].TimeToEnd > TaskManagerClass.ResultListTasks[i].TimeToEnd));

                        if (intersectionLeft || intersectionRight || intersectionRightLeft)
                        {
                            numbers.Add(TaskManagerClass.ResultListTasks[i]);
                        }
                    }
                }

                directTime -= TaskManagerClass.ResultListTasks[j].TimeToWork;

                a++;
            }

            if (numbers.Count > 0)
            {
                ResultTasksListTextBox.AppendText("\n\n\nРабота алгоритма НЕВЕРНА, имеются пересечения!");
            }
            else
            {
                ResultTasksListTextBox.AppendText("\n\nРабота алгоритма ВЕРНА, персечений нет");
                ResultTasksListTextBox.AppendText("\n\nВремя простоя процессора равно - " + (directTime));
            }

        }
    }
}