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

                if (_min.TimeToStart >= 0)
                // если есть пересечение (есть "крайняя левая" задача с которой пересекается очередная), то:
                {
                    if (InsertionCapability(settingsParametrs, TaskManagerClass.ResultListTasks.Count, OverlappingTasks(TaskManagerClass.ResultListTasks.Count)))
                    // проверяется возможность вставки, и если вставка возможна, то:
                    {
                        ShiftTasks(settingsParametrs, TaskManagerClass.ResultListTasks.Count); // сдвиг задач
                        InsertionTask(TaskManagerClass.ResultListTasks.Count); // вставка задачи
                        SortTimeToStart();
                    }
                }
                else
                {
                    if (InsertionCapability(settingsParametrs, TaskManagerClass.ResultListTasks.Count, OverlappingTasks(TaskManagerClass.ResultListTasks.Count)))
                    // проверяется возможность вставки, и если вставка возможна, то:
                    {
                        InsertionTask(TaskManagerClass.ResultListTasks.Count); // вставка задачи
                        SortTimeToStart();
                    }
                }
            }

            OutputResultListTasks();

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
        /// поиск крайней левой задачи
        /// </summary>
        /// <param name="list"> список задач с которыми пересекается очередная </param>
        public void FindLeft(List<TaskClass> list)
        {
            if (list.Count != 0) // если список пересекающихся задач не нулевой, то первый эл-т явл. крайним "левым"
            {
                _min = list[0];
            }
            else
            {
                _min.TimeToStart = -1;
            }
        }

        /// <summary>
        /// поиска задач, с которыми персекается очередная
        /// </summary>
        /// <param name="next">количество задач в ResultTaskList и, в тоже время, номер очередной "пришедшей" задачи</param>
        /// <returns>возвращает список задач с которыми пересекается очередная</returns>
        public List<TaskClass> OverlappingTasks(int next)
        {
            List<TaskClass> numbers = new List<TaskClass>();

            for (int i = 0; i < next; i++)
            {
                // пересечение справа
                bool intersectionRight = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.ResultListTasks[i].TimeToEnd) &&
                    (TaskManagerClass.ListTasks[next].TimeToStart >= TaskManagerClass.ResultListTasks[i].TimeToStart);

                // пересечение слева
                bool intersectionLeft = (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.ResultListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd <= TaskManagerClass.ResultListTasks[i].TimeToEnd);

                // пришедшая задача "поглощает" пересекаемую 
                bool intersectionRightLeft = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.ResultListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.ResultListTasks[i].TimeToEnd);

                if (intersectionLeft || intersectionRight || intersectionRightLeft)
                {
                    numbers.Add(TaskManagerClass.ResultListTasks[i]); // в списке number храняться номера задач с которыми
                                                                     // пересекается очередная "пришедшая" 
                }
            }

            return numbers;
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
            double x;

            if (_min.TimeToStart == -1) // проверка вставки при отсутствии пересечения
            {
                if (TaskManagerClass.ListTasks[next].TimeToEnd < settingsParametrs.DirectTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // проверка вставки при наличии пересечения
            {
                if (TaskManagerClass.ListTasks[next].TimeToStart >= _min.TimeToStart) //если время старта очередной >= 
                                                                                      //времени старта крайней левой задачи, то
                {
                    x = settingsParametrs.DirectTime - _min.TimeToEnd;

                    double y = x;
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
                    x = settingsParametrs.DirectTime - TaskManagerClass.ListTasks[next].TimeToEnd;

                    double y = x;
                    for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                    {
                        if (TaskManagerClass.ResultListTasks[i].TimeToStart > _min.TimeToEnd)
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
        }

            

        /// <summary>
        /// поиск задач подлежащих сдвигу, и их сдвиг
        /// </summary>
        /// <param name="settingsParametrs"></param>
        /// <param name="next"> количество задач в ResultTaskList и, в тоже время, номер очередной "пришедшей" задачи </param>
        public void ShiftTasks(SettingsParametrs settingsParametrs, int next)
        {
            if (TaskManagerClass.ListTasks[next].TimeToStart >= _min.TimeToStart) // если время старта очередной >= 
                                                                                  // времени старта крайней левой задачи, то
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
            else // если время старта очередной < 
                 // времени старта крайней левой задачи, то
            {
                int newTimeToStart = TaskManagerClass.ListTasks[next].TimeToEnd; // время с которого начнуться новые хадачи

                for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
                {
                    if ((TaskManagerClass.ResultListTasks[i].TimeToStart >= _min.TimeToStart) &&
                       (TaskManagerClass.ResultListTasks[i].TimeToStart < newTimeToStart))
                    {
                        TaskManagerClass.ResultListTasks[i].TimeToStart = newTimeToStart;
                        if (i > 0)
                        {
                            newTimeToStart += TaskManagerClass.ResultListTasks[i].TimeToWork;
                        }                        
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
            if (_min.TimeToStart == -1) // если нету пересечения, то вставляем задачу
            {
                TaskManagerClass.ResultListTasks.Add(TaskManagerClass.ListTasks[next]);
            }
            else
            {
                if ((TaskManagerClass.ListTasks[next].TimeToStart >= _min.TimeToStart) &&
                (TaskManagerClass.ListTasks[next].TimeToStart < _min.TimeToEnd))
                {
                    TaskManagerClass.ListTasks[next].TimeToStart = _min.TimeToEnd;
                }

                TaskManagerClass.ResultListTasks.Add(TaskManagerClass.ListTasks[next]);
            }            
        }

        /// <summary>
        /// сортировка элементов по времени старта
        /// </summary>
        public void SortTimeToStart()
        {
            TaskManagerClass.ResultListTasks = TaskManagerClass.ResultListTasks.OrderBy(l => l.TimeToStart).ToList();
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
                ResultTasksListTextBox.AppendText("\n\nРабота алгоритма ВЕРНА, персечений нету");
                ResultTasksListTextBox.AppendText("\n\nВремя простоя процессора равно - " + (directTime));
            }

        }
    }
}