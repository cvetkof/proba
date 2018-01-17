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
  
        public SheduleWindow(SettingsParametrs settingsParametrs)
        {
            InitializeComponent();

            FillSettingsValues(settingsParametrs);
            SortRelativityImportance(); // сортировка списка задач по убыванию относительной важности
            InsertFirstTask(); // вставка первой задачи в список-результат

            for (int i = 1; i < TaskManagerClass.ListTasks.Count; i++)
            {
                FindLeft(OverlappingTasks(TaskManagerClass.ResultListTasks.Count));

                if (_min.TimeToWork != 0)
                // если есть пересечение (есть "крайняя левая" задача с которой пересекается очередная), то:
                {
                    if (InsertionCapability(settingsParametrs, TaskManagerClass.ResultListTasks.Count, OverlappingTasks(TaskManagerClass.ResultListTasks.Count)))
                    // проверяется возможность вставки, и если вставка возможна, то:
                    {
                        ShiftTasks(settingsParametrs, TaskManagerClass.ResultListTasks.Count); // сдвиг задач
                        InsertionTask(TaskManagerClass.ResultListTasks.Count); // вставка задачи
                    }
                }
                else
                {
                    if (InsertionCapability(settingsParametrs, TaskManagerClass.ResultListTasks.Count, OverlappingTasks(TaskManagerClass.ResultListTasks.Count)))
                    // проверяется возможность вставки, и если вставка возможна, то:
                    {
                        InsertionTask(TaskManagerClass.ResultListTasks.Count); // вставка задачи
                    }
                }
            }

            OutputResultListTasks();
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

        public void SortRelativityImportance()
        {
            TaskManagerClass.ListTasks = TaskManagerClass.ListTasks.OrderByDescending(l => l.RelativityImportance).ToList();
        }

        /// <summary>
        /// вставка первой задачи в список-результат
        /// </summary>
        public void InsertFirstTask()
        {
            TaskManagerClass.InitializeResultListTasks();
            TaskManagerClass.ResultListTasks.Add(TaskManagerClass.ListTasks[0]);
        }

        /// <summary>
        /// вывод результирующего списка
        /// </summary>
        public void OutputResultListTasks()
        {
            for(int task = 0; task < TaskManagerClass.ResultListTasks.Count; task++)
            {
                ResultTasksListTextBox.AppendText("Время поступления " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].TimeToStart + "\n");
                ResultTasksListTextBox.AppendText("Время обработки  " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].TimeToWork + "\n");
                ResultTasksListTextBox.AppendText("Важность " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].Importance + "\n");
                ResultTasksListTextBox.AppendText("Порядковый номер " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].IndexNumber + "\n");
            }
        }

        /// <summary>
        /// поиск крайней левой задачи
        /// </summary>
        /// <param name="list"> список задач с которыми пересекается очередная </param>
        public void FindLeft(List<TaskClass> list)
        {
            if (list.Count != 0) // если список пересекающихся задач не нулевой, то первый эл-т явл. крайним "левым"
            {
                _min = list[0];
            }
        }

        /// <summary>
        /// поиска задач, с которыми персекается очередная
        /// </summary>
        /// <param name="next">количество задач в ResultTaskList и, в тоже время, номер очередной "пришедшей" задачи</param>
        /// <returns>возвращает список задач с которыми пересекается очередная</returns>
        public List<TaskClass> OverlappingTasks(int next)
        {
            List<TaskClass> number = new List<TaskClass>();

            for (int i = 0; i < next; i++)
            {
                // пересечение слева
                bool intersectionRight = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.ListTasks[i].TimeToEnd) &&
                    (TaskManagerClass.ListTasks[next].TimeToStart >= TaskManagerClass.ListTasks[i].TimeToStart);
                // пересечение справа
                bool intersectionLeft = (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.ListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd <= TaskManagerClass.ListTasks[i].TimeToEnd);
                // пришедшая задача "поглощает" пересекаемую 
                bool intersectionRightLeft = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.ListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.ListTasks[i].TimeToEnd);

                if (intersectionLeft || intersectionRight || intersectionRightLeft)
                {
                    number.Add(TaskManagerClass.ListTasks[i]); // в списке number храняться номера задач с которыми
                                                               // пересекается очередная "пришедшая" 
                }
            }

            return number;
        }

        /// <summary>
        /// проверка возможность вставки очередной задачи
        /// </summary>
        /// <param name="settingsParametrs"></param>
        /// <param name="next"> количество задач в ResultTaskList и, в тоже время, номер очередной "пришедшей" задачи </param>
        /// <param name="list"> список задач с которыми пересекается очередная </param>
        /// <returns></returns>
        public bool InsertionCapability(SettingsParametrs settingsParametrs, int next, List<TaskClass> list)
            {
            int x;

            if (TaskManagerClass.ListTasks[next].TimeToStart >= _min.TimeToStart) //если время старта очередной >= 
                                                                                  //времени старта крайней левой задачи, то
            {
                x = settingsParametrs.DirectTime - _min.TimeToEnd;

                int y = x;
                for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if (TaskManagerClass.ResultListTasks[i].TimeToStart > _min.TimeToStart)
                    {
                        y -= TaskManagerClass.ResultListTasks[i].TimeToWork;
                    }
                }
               
                bool firstCheck = TaskManagerClass.ListTasks[next].TimeToEnd < settingsParametrs.DirectTime;
                bool secondCheck = x >= TaskManagerClass.ListTasks[next].TimeToWork;
                bool thirdCheck = y > TaskManagerClass.ListTasks[next].TimeToWork;

                if (firstCheck && secondCheck && thirdCheck)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else //если время старта очередной задачи < 
                 //времени старта крайней левой задачи, то
            {
                x =settingsParametrs.DirectTime - TaskManagerClass.ListTasks[next].TimeToEnd;

                int y = x;
                for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if (TaskManagerClass.ResultListTasks[i].TimeToStart > TaskManagerClass.ListTasks[next].TimeToEnd)
                    {
                        y -= TaskManagerClass.ResultListTasks[i].TimeToWork;
                    }
                }

                var sumTimeToWork = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    sumTimeToWork += list[i].TimeToWork; // сумма времен работы задач с которыми пересекается очередная
                }

                bool firstCheck = TaskManagerClass.ListTasks[next].TimeToEnd < settingsParametrs.DirectTime;
                bool secondCheck = x >= sumTimeToWork;
                bool thirdCheck = y > sumTimeToWork;

                if (firstCheck && secondCheck && thirdCheck)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }            
        }

        /// <summary>
        /// поиск задач подлежащих сдвигу, и их сдвиг
        /// </summary>
        /// <param name="settingsParametrs"></param>
        /// <param name="next"> количество задач в ResultTaskList и, в тоже время, номер очередной "пришедшей" задачи </param>
        public void ShiftTasks(SettingsParametrs settingsParametrs, int next)
        {
            if (TaskManagerClass.ListTasks[next].TimeToStart >= _min.TimeToStart) //если время старта очередной >= 
                                                                                 //времени старта крайней левой задачи, то
            {
                int newTimeToStart = _min.TimeToEnd + TaskManagerClass.ListTasks[next].TimeToWork; // время с которого начнуться новые хадачи

                for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if ((TaskManagerClass.ResultListTasks[i].TimeToStart > _min.TimeToStart) && 
                       (TaskManagerClass.ResultListTasks[i].TimeToStart < newTimeToStart))
                    {
                        TaskManagerClass.ResultListTasks[i].TimeToStart = newTimeToStart;
                        newTimeToStart += TaskManagerClass.ResultListTasks[i].TimeToWork;
                    }
                }
            }
            else //если время старта очередной < 
                 //времени старта крайней левой задачи, то
            {
                int newTimeToStart = TaskManagerClass.ListTasks[next].TimeToEnd + _min.TimeToWork; // время с которого начнуться новые хадачи

                for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if ((TaskManagerClass.ResultListTasks[i].TimeToStart >= _min.TimeToStart) &&
                       (TaskManagerClass.ResultListTasks[i].TimeToStart < newTimeToStart))
                    {
                        TaskManagerClass.ResultListTasks[i].TimeToStart = newTimeToStart;
                        newTimeToStart += TaskManagerClass.ResultListTasks[i].TimeToWork; // ?????????????????????????????????????
                    }
                }
            }
        }

        /// <summary>
        /// вставка очередной задачи
        /// </summary>
        /// <param name="next"> порядковый номер очередной задачи </param>
        public void InsertionTask(int next)
        {
            if (TaskManagerClass.ListTasks[next].TimeToStart >= _min.TimeToStart)
            {
                TaskManagerClass.ListTasks[next].TimeToStart = _min.TimeToEnd;
            }
            
            TaskManagerClass.ResultListTasks.Add(TaskManagerClass.ListTasks[next]);
        }

    }
}