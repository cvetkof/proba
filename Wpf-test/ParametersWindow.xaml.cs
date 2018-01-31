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
    /// Логика взаимодействия для ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        private SettingsParametrs _settingsParametrs;
        private TaskClass _min = new TaskClass(); //в min хранится первая задача с которой пересекается очередная "пришедшая" задача

        public ParametersWindow(SettingsParametrs settingsParametrs)
        {
            this._settingsParametrs = settingsParametrs;
            InitializeComponent();
            FillSettingsValues(settingsParametrs);            
        }

        private void FillSettingsValues(SettingsParametrs settingsParametrs)
        {
            ProcCountTextBox.Text = settingsParametrs.ProcCount.ToString();
            DirectTimeTextBox.Text = settingsParametrs.DirectTime.ToString();

            GenerateTable(settingsParametrs);
        }

        private void GenerateTable(SettingsParametrs settingsParametrs)
        {
                if(settingsParametrs.InputType == Enums.InputType.Random)
                {
                    Random rand = new Random();

                    TaskManagerClass.InitializeListTasks();

                    for (int count = 0; count < settingsParametrs.TaskCounts; count++)
                    {
                        var task = new TaskClass
                        {
                            TimeToStart = rand.Next(1, 5400), // значение от 1с до  1ч 30мин
                            TimeToWork = rand.Next(1, 420), // значение от 1c до 7мин
                            Importance = rand.Next(1, 100), // значение от 1 до 100
                            IndexNumber = count + 1
                        };

                        TaskManagerClass.ListTasks.Add(task);

                        ParametrsFirstResults.AppendText("время поступления " + (count + 1) + "-ой задачи - " + TaskManagerClass.ListTasks[count].TimeToStart + "\n");
                        ParametrsFirstResults.AppendText("время обработки " + (count + 1) + "-ой задачи    - " + TaskManagerClass.ListTasks[count].TimeToWork + "\n");
                        ParametrsFirstResults.AppendText("важность " + (count + 1) + "-ой задачи                  - " + TaskManagerClass.ListTasks[count].Importance + "\n\n");
                        //ParametrsFirstResults.AppendText("порядковый номер - " + TaskManagerClass.ListTasks[count].IndexNumber + "\n\n");
                    }
                }
                else
                {
                    for (int count = 0; count < settingsParametrs.TaskCounts; count++)
                    {
                        ParametrsFirstResults.AppendText("время поступления " + (count + 1) + "-ой задачи - " + TaskManagerClass.ListTasks[count].TimeToStart + "\n");
                        ParametrsFirstResults.AppendText("время обработки " + (count + 1) + "-ой задачи    - " + TaskManagerClass.ListTasks[count].TimeToWork + "\n");
                        ParametrsFirstResults.AppendText("важность " + (count + 1) + "-ой задачи                  - " + TaskManagerClass.ListTasks[count].Importance + "\n\n");
                        //ParametrsFirstResults.AppendText("порядковый номер - " + TaskManagerClass.ListTasks[count].IndexNumber + "\n\n");
                    }
                }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();            
        }

        private void MakeShedule(object sender, RoutedEventArgs e)
        {
            SortRelativityImportance();
            TaskManagerClass.InitializeMiddleResultListTasks();
            TaskManagerClass.InitializeResultListTasks();

            for (int procCount = 0; procCount < _settingsParametrs.ProcCount; procCount++) // цикл количества процессоров
            {
                
                InsertFirstTask();
                //TaskManagerClass.ListTasks.RemoveAt(0);
                
                for (int i = 1; i < TaskManagerClass.ListTasks.Count; i++)
                {

                    FindLeft(OverlappingTasks(TaskManagerClass.MiddleResultListTasks.Count));

                    if (_min.TimeToStart >= 0)
                    // если есть пересечение (есть "крайняя левая" задача с которой пересекается очередная), то:
                    {
                        if (InsertionCapability(this._settingsParametrs, TaskManagerClass.MiddleResultListTasks.Count, OverlappingTasks(TaskManagerClass.MiddleResultListTasks.Count)))
                        // проверяется возможность вставки, и если вставка возможна, то:
                        {
                            ShiftTasks(this._settingsParametrs, TaskManagerClass.MiddleResultListTasks.Count); // сдвиг задач
                            InsertionTask(TaskManagerClass.MiddleResultListTasks.Count, procCount); // вставка задачи
                            SortTimeToStart();
                        }
                    }
                    else // если нет пересечения
                    {
                        if (InsertionCapability(this._settingsParametrs, TaskManagerClass.MiddleResultListTasks.Count, OverlappingTasks(TaskManagerClass.MiddleResultListTasks.Count)))
                        // проверяется возможность вставки, и если вставка возможна, то:
                        {
                            InsertionTask(TaskManagerClass.MiddleResultListTasks.Count, procCount); // вставка задачи
                            SortTimeToStart();
                        }
                    }
                }

                // удаление выставленных в промежуточный результирующий список элементов
                TaskManagerClass.ResultListTasks.AddRange(TaskManagerClass.MiddleResultListTasks); // перенос из промежуточного в результирующий список элементов
                TaskManagerClass.MiddleResultListTasks.Clear(); // удаление всех элементов промежуточного списка
            }
                

            var sheduleWindow = new SheduleWindow(this._settingsParametrs);
            //this.WindowState = WindowState.Maximized;
            sheduleWindow.Show();
        }

        /// <summary>
        /// сортировка по относительной важности
        /// </summary>
        public void SortRelativityImportance()
        {
            TaskManagerClass.ListTasks = TaskManagerClass.ListTasks.OrderByDescending(l => l.RelativityImportance).ToList();
        }

        /// <summary>
        /// вставка первой задачи в список-результат
        /// </summary>
        public void InsertFirstTask()
        {
            TaskManagerClass.MiddleResultListTasks.Add(TaskManagerClass.ListTasks[0]);
        }

        private void SetPBar()
        {
            this.pBar.Maximum = this._settingsParametrs.TaskCounts;
            this.pBar.Minimum = 0;
            this.pBar.Value = 0;     
        }

        /// <summary>
        /// поиск крайней левой задачи
        /// </summary>
        /// <param name="list"> список задач с которыми пересекается очередная </param>
        public void FindLeft(List<TaskClass> list)
        {
            if (list.Count != 0) // если список пересекающихся задач не нулевой, то первый эл-т явл. крайним "левым"
            {
                _min = new TaskClass
                {
                    TimeToStart = list[0].TimeToStart,
                    TimeToWork = list[0].TimeToWork,
                    Importance = list[0].Importance,
                    IndexNumber = list[0].IndexNumber
                };
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
        /// <returns> возвращает список задач с которыми пересекается очередная </returns>
        public List<TaskClass> OverlappingTasks(int next)
        {
            List<TaskClass> numbers = new List<TaskClass>();

            for (int i = 0; i < next; i++)
            {
                // пересечение справа
                bool intersectionRight = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.MiddleResultListTasks[i].TimeToEnd) &&
                    (TaskManagerClass.ListTasks[next].TimeToStart >= TaskManagerClass.MiddleResultListTasks[i].TimeToStart);

                // пересечение слева
                bool intersectionLeft = (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.MiddleResultListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd <= TaskManagerClass.MiddleResultListTasks[i].TimeToEnd);

                // пришедшая задача "поглощает" пересекаемую 
                bool intersectionRightLeft = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.MiddleResultListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.MiddleResultListTasks[i].TimeToEnd);

                if (intersectionLeft || intersectionRight || intersectionRightLeft)
                {
                    numbers.Add(TaskManagerClass.MiddleResultListTasks[i]); // в списке number храняться номера задач с которыми
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
                    for (int i = 0; i < TaskManagerClass.MiddleResultListTasks.Count; i++)
                    {
                        if (TaskManagerClass.MiddleResultListTasks[i].TimeToStart > _min.TimeToStart)
                        {
                            y -= TaskManagerClass.MiddleResultListTasks[i].TimeToWork;
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
                    for (int i = 0; i < TaskManagerClass.MiddleResultListTasks.Count; i++)
                    {
                        if (TaskManagerClass.MiddleResultListTasks[i].TimeToStart > _min.TimeToEnd)
                        {
                            y -= TaskManagerClass.MiddleResultListTasks[i].TimeToWork;
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

                for (int i = 0; i < TaskManagerClass.MiddleResultListTasks.Count; i++)
                {
                    if ((TaskManagerClass.MiddleResultListTasks[i].TimeToStart > _min.TimeToStart) &&
                       (TaskManagerClass.MiddleResultListTasks[i].TimeToStart < newTimeToStart))
                    {
                        TaskManagerClass.MiddleResultListTasks[i].TimeToStart = newTimeToStart;
                        newTimeToStart += TaskManagerClass.MiddleResultListTasks[i].TimeToWork;
                    }
                }
            }
            else // если время старта очередной < 
                 // времени старта крайней левой задачи, то
            {
                int newTimeToStart = TaskManagerClass.ListTasks[next].TimeToEnd; // время с которого начнуться новые хадачи

                for (int i = 0; i < TaskManagerClass.MiddleResultListTasks.Count; i++)
                {
                    if ((TaskManagerClass.MiddleResultListTasks[i].TimeToStart >= _min.TimeToStart) &&
                       (TaskManagerClass.MiddleResultListTasks[i].TimeToStart < newTimeToStart))
                    {
                        TaskManagerClass.MiddleResultListTasks[i].TimeToStart = newTimeToStart;

                        newTimeToStart += TaskManagerClass.MiddleResultListTasks[i].TimeToWork;
                    }
                }
            }
        }

        /// <summary>
        /// вставка очередной задачи
        /// </summary>
        /// <param name="next"> порядковый номер очередной задачи </param>
        /// <param name="proc"> порядковый номер текущего процессора </param>
        public void InsertionTask(int next, int proc)
        {
            if (_min.TimeToStart == -1) // если нету пересечения, то вставляем задачу
            {
                TaskManagerClass.ListTasks[next].NumberProc = proc + 1;
                TaskManagerClass.MiddleResultListTasks.Add(TaskManagerClass.ListTasks[next]);
            }
            else // если есть пересечение
            {
                if ((TaskManagerClass.ListTasks[next].TimeToStart >= _min.TimeToStart) &&
                (TaskManagerClass.ListTasks[next].TimeToStart < _min.TimeToEnd))
                {
                    TaskManagerClass.ListTasks[next].TimeToStart = _min.TimeToEnd;
                }

                TaskManagerClass.ListTasks[next].NumberProc = proc + 1;
                TaskManagerClass.MiddleResultListTasks.Add(TaskManagerClass.ListTasks[next]);
            }
        }

        /// <summary>
        /// сортировка элементов по времени старта
        /// </summary>
        public void SortTimeToStart()
        {
            TaskManagerClass.MiddleResultListTasks = TaskManagerClass.MiddleResultListTasks.OrderBy(l => l.TimeToStart).ToList();
        }

    }
}
