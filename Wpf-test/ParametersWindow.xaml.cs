using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private TaskClass _min = new TaskClass(); //в min хранится первая задача с которой пересекается очередная "пришедшая" задача
        private int _sumWFirst = 0;
                

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
            TasksCountTextBox.Text = settingsParametrs.TaskCounts.ToString();

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
                            Mathematic = rand.Next(1,73),
                            Dispr = rand.Next(1,73),
                            TimeToStart = rand.Next(1, 5400), // значение от 1с до  1ч 30мин
                            TimeToWork = rand.Next(1, 420), // значение от 1c до 7мин
                            Importance = rand.Next(1, 100), // значение от 1 до 100
                            IndexNumber = count + 1,
                            Guid = Guid.NewGuid()
                        };

                        TaskManagerClass.ListTasks.Add(task);

                        //ParametrsFirstResults.AppendText("время поступления " + (count + 1) + "-ой задачи - " + TaskManagerClass.ListTasks[count].TimeToStart + "\n");
                        //ParametrsFirstResults.AppendText("время обработки " + (count + 1) + "-ой задачи    - " + TaskManagerClass.ListTasks[count].TimeToWork + "\n");
                        //ParametrsFirstResults.AppendText("важность " + (count + 1) + "-ой задачи                  - " + TaskManagerClass.ListTasks[count].Importance + "\n\n");
                        //ParametrsFirstResults.AppendText("порядковый номер - " + TaskManagerClass.ListTasks[count].IndexNumber + "\n\n");

                        ParametrsFirstResults.AppendText((count + 1) + "\t" + Convert.ToString(TaskManagerClass.ListTasks[count].TimeToStart) + "\t" +
                            Convert.ToString(TaskManagerClass.ListTasks[count].TimeToWork) + "\t" + Convert.ToString(TaskManagerClass.ListTasks[count].Importance) + "\t" +
                            Convert.ToString(TaskManagerClass.ListTasks[count].NumberProc) + "\t" + Convert.ToString(TaskManagerClass.ListTasks[count].Mathematic) + "\t" +
                            Convert.ToString(TaskManagerClass.ListTasks[count].Dispr));
                        ParametrsFirstResults.AppendText("\n");

                    }
                }
                else
                {
                    for (int count = 0; count < settingsParametrs.TaskCounts; count++)
                    {
                    //ParametrsFirstResults.AppendText("время поступления " + (count + 1) + "-ой задачи - " + TaskManagerClass.ListTasks[count].TimeToStart + "\n");
                    //ParametrsFirstResults.AppendText("время обработки " + (count + 1) + "-ой задачи    - " + TaskManagerClass.ListTasks[count].TimeToWork + "\n");
                    //ParametrsFirstResults.AppendText("важность " + (count + 1) + "-ой задачи                  - " + TaskManagerClass.ListTasks[count].Importance + "\n\n");
                    //ParametrsFirstResults.AppendText("порядковый номер - " + TaskManagerClass.ListTasks[count].IndexNumber + "\n\n");

                    ParametrsFirstResults.AppendText((count + 1) + "\t" + Convert.ToString(TaskManagerClass.ListTasks[count].TimeToStart) + "\t" +
                        Convert.ToString(TaskManagerClass.ListTasks[count].TimeToWork) + "\t" + Convert.ToString(TaskManagerClass.ListTasks[count].Importance) + "\t" +
                        Convert.ToString(TaskManagerClass.ListTasks[count].NumberProc) + "\t" + Convert.ToString(TaskManagerClass.ListTasks[count].Mathematic) + "\t" +
                        Convert.ToString(TaskManagerClass.ListTasks[count].Dispr));
                    ParametrsFirstResults.AppendText("\n");
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

            SumWFirst();

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    this.pBar.Maximum = this._settingsParametrs.TaskCounts * this._settingsParametrs.ProcCount;
                    this.pBar.Value = 0;
                });

                for (int procCount = 0; procCount < _settingsParametrs.ProcCount; procCount++) // цикл количества процессоров
                {
                    if (TaskManagerClass.ListTasks.Count > 0)
                    {
                        for (int i = 0; i < TaskManagerClass.ListTasks.Count; i++)
                        {
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

                            Dispatcher.Invoke(() =>
                            {
                                this.pBar.Value++;
                            });
                        }

                        Dispatcher.Invoke(() =>
                        {
                            //this.pBar.Maximum = this.pBar.Maximum - (TaskManagerClass.MiddleResultListTasks.Count * this._settingsParametrs.ProcCount);
                            for (int k = 0; k < TaskManagerClass.ResultListTasks.Count; k++)
                            {
                                this.pBar.Value++;
                            }
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            for (int k = 0; k < this._settingsParametrs.TaskCounts; k++)
                            {
                                this.pBar.Value++;
                            }
                        });
                    }

                    TaskManagerClass.ResultListTasks.AddRange(TaskManagerClass.MiddleResultListTasks); // перенос из промежуточного в результирующий список элементов
                    TaskManagerClass.MiddleResultListTasks.Clear(); // удаление всех элементов промежуточного списка
                    DeleteTasks(); // удаление элементов выставленных в результирующий список из начального списка
                    _min = null;
                }

            SetResultIndexNumber();

            }).ContinueWith(task => {

                Dispatcher.Invoke(() =>
                {
                    var sheduleWindow = new SheduleWindow(this._settingsParametrs, this._sumWFirst);
                    sheduleWindow.Show();
                    this.Close();
                });
            });
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

        /// <summary>
        /// присвоение порядкового номера
        /// </summary>
        public void SetResultIndexNumber()
        {
            for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
            {
                TaskManagerClass.ResultListTasks[i].ResultIndexNumber = i + 1;
            }
        }
    }
}
