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
        int _sumWFirst = 0;

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
                            IndexNumber = count + 1,
                            Guid = Guid.NewGuid()
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
            SetPBar();
            SortRelativityImportance();
            TaskManagerClass.InitializeMiddleResultListTasks();
            TaskManagerClass.InitializeResultListTasks();

            SumWFirst();

            for (int procCount = 0; procCount < _settingsParametrs.ProcCount; procCount++) // цикл количества процессоров
            {                
                for (int i = 0; i < TaskManagerClass.ListTasks.Count; i++)
                {
                    pBar.Value++;
                 
                    FindLeft(OverlappingTasks(i));

                    if (_min.TimeToStart >= 0)
                    // если есть пересечение (есть "крайняя левая" задача с которой пересекается очередная), то:
                    {
                        if (InsertionCapability(i, OverlappingTasks(i)))
                        // проверяется возможность вставки, и если вставка возможна, то:
                        {
                            ShiftTasks(i); // сдвиг задач
                            InsertionTask(i, procCount); // вставка задачи
                            SortTimeToStart();
                        }
                    }
                    else // если нет пересечения
                    {
                        if (InsertionCapability(i, OverlappingTasks(i)))
                        // проверяется возможность вставки, и если вставка возможна, то:
                        {
                            InsertionTask(i, procCount); // вставка задачи
                            SortTimeToStart();
                        }
                    }
                }

                TaskManagerClass.ResultListTasks.AddRange(TaskManagerClass.MiddleResultListTasks); // перенос из промежуточного в результирующий список элементов
                TaskManagerClass.MiddleResultListTasks.Clear(); // удаление всех элементов промежуточного списка
                DeleteTasks(); // удаление элементов выставленных в результирующий список из начального списка
                _min = null;
            }


            var sheduleWindow = new SheduleWindow(this._settingsParametrs, this._sumWFirst);
            //this.WindowState = WindowState.Maximized;
            sheduleWindow.Show();
        }

        /// <summary>
        /// сумма важностей всех задач
        /// </summary>
        public void SumWFirst()
        {
            for (int i = 0; i < TaskManagerClass.ListTasks.Count; i++)
                _sumWFirst += TaskManagerClass.ListTasks[i].Importance;
        }

        /// <summary>
        /// сортировка по относительной важности
        /// </summary>
        public void SortRelativityImportance()
        {
            TaskManagerClass.ListTasks = TaskManagerClass.ListTasks.OrderByDescending(l => l.RelativityImportance).ToList();
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
                _min = new TaskClass
                {
                    TimeToStart = -1
                };
            }
        }

        /// <summary>
        /// поиска задач, с которыми персекается очередная
        /// </summary>
        /// <param name="next_task"> номер следующей задачи </param>
        /// <returns> возвращает список задач с которыми пересекается очередная </returns>
        public List<TaskClass> OverlappingTasks(int next_task)
        {
            List<TaskClass> numbers = new List<TaskClass>();

            for (int i = 0; i < TaskManagerClass.MiddleResultListTasks.Count; i++)
            {
                // пересечение справа
                bool intersectionRight = (TaskManagerClass.ListTasks[next_task].TimeToStart < TaskManagerClass.MiddleResultListTasks[i].TimeToEnd) &&
                    (TaskManagerClass.ListTasks[next_task].TimeToStart >= TaskManagerClass.MiddleResultListTasks[i].TimeToStart);

                // пересечение слева
                bool intersectionLeft = (TaskManagerClass.ListTasks[next_task].TimeToEnd > TaskManagerClass.MiddleResultListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next_task].TimeToEnd <= TaskManagerClass.MiddleResultListTasks[i].TimeToEnd);

                // пришедшая задача "поглощает" пересекаемую 
                bool intersectionRightLeft = (TaskManagerClass.ListTasks[next_task].TimeToStart < TaskManagerClass.MiddleResultListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next_task].TimeToEnd > TaskManagerClass.MiddleResultListTasks[i].TimeToEnd);

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
        /// <param name="next_task"> номер следующей задачи </param>
        /// <param name="list"> список задач с которыми пересекается очередная </param>
        /// <returns></returns>
        public bool InsertionCapability(int next_task, List<TaskClass> list)
        {
            double x;

            if (_min.TimeToStart == -1) // проверка вставки при отсутствии пересечения
            {
                if (TaskManagerClass.ListTasks[next_task].TimeToEnd <= this._settingsParametrs.DirectTime)
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
                if (TaskManagerClass.ListTasks[next_task].TimeToStart >= _min.TimeToStart) //если время старта очередной >= 
                                                                                           //времени старта крайней левой задачи, то
                {
                    x = this._settingsParametrs.DirectTime - _min.TimeToEnd;

                    double y = x;

                    for (int i = 0; i < TaskManagerClass.MiddleResultListTasks.Count; i++)
                    {
                        if (TaskManagerClass.MiddleResultListTasks[i].TimeToStart >= _min.TimeToEnd)
                        {
                            y -= TaskManagerClass.MiddleResultListTasks[i].TimeToWork;
                        }
                    }

                    bool firstCheck = TaskManagerClass.ListTasks[next_task].TimeToEnd <= this._settingsParametrs.DirectTime;
                    bool secondCheck = TaskManagerClass.ListTasks[next_task].TimeToWork <= x;
                    bool thirdCheck = TaskManagerClass.ListTasks[next_task].TimeToWork <= y;

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
                    x = this._settingsParametrs.DirectTime - TaskManagerClass.ListTasks[next_task].TimeToEnd;

                    double y = x;

                    for (int i = 0; i < TaskManagerClass.MiddleResultListTasks.Count; i++)
                    {
                        if (TaskManagerClass.MiddleResultListTasks[i].TimeToStart >= TaskManagerClass.ListTasks[next_task].TimeToEnd)
                        {
                            y -= TaskManagerClass.MiddleResultListTasks[i].TimeToWork;
                        }
                    }

                    var sumTimeToWork = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        sumTimeToWork += list[i].TimeToWork; // сумма времен работы задач с которыми пересекается очередная
                    }

                    bool firstCheck = TaskManagerClass.ListTasks[next_task].TimeToEnd < this._settingsParametrs.DirectTime;
                    bool secondCheck = sumTimeToWork <= x;
                    bool thirdCheck = sumTimeToWork <= y;

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
        /// <param name="next_task"> количество задач в ResultTaskList и, в тоже время, номер очередной "пришедшей" задачи </param>
        public void ShiftTasks(int next_task)
        {
            if (TaskManagerClass.ListTasks[next_task].TimeToStart >= _min.TimeToStart) // если время старта очередной >= 
                                                                                  // времени старта крайней левой задачи, то
            {
                int newTimeToStart = _min.TimeToEnd + TaskManagerClass.ListTasks[next_task].TimeToWork; // время с которого начнуться новые хадачи

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
                int newTimeToStart = TaskManagerClass.ListTasks[next_task].TimeToEnd; // время с которого начнуться новые хадачи

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
        /// <param name="next_task"> порядковый номер очередной задачи </param>
        /// <param name="proc"> порядковый номер текущего процессора </param>
        public void InsertionTask(int next_task, int proc)
        {
            if (_min.TimeToStart == -1) // если нету пересечения, то вставляем задачу
            {
                TaskManagerClass.ListTasks[next_task].NumberProc = proc + 1;
                TaskManagerClass.MiddleResultListTasks.Add(TaskManagerClass.ListTasks[next_task]);
            }
            else // если есть пересечение
            {
                if ((TaskManagerClass.ListTasks[next_task].TimeToStart >= _min.TimeToStart) &&
                (TaskManagerClass.ListTasks[next_task].TimeToStart < _min.TimeToEnd))
                {
                    TaskManagerClass.ListTasks[next_task].TimeToStart = _min.TimeToEnd;
                }

                TaskManagerClass.ListTasks[next_task].NumberProc = proc + 1;
                TaskManagerClass.MiddleResultListTasks.Add(TaskManagerClass.ListTasks[next_task]);
            }
        }

        /// <summary>
        /// сортировка элементов по времени старта
        /// </summary>
        public void SortTimeToStart()
        {
            TaskManagerClass.MiddleResultListTasks = TaskManagerClass.MiddleResultListTasks.OrderBy(l => l.TimeToStart).ToList();
        }

        /// <summary>
        /// удаление совпадающих задач
        /// </summary>
        public void DeleteTasks()
        {
            foreach (var task in TaskManagerClass.ResultListTasks)
            {
                TaskManagerClass.ListTasks.RemoveAll(t => t.Guid == task.Guid);
            }
        }
    }
}
