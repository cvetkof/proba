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

        private TaskClass _min = new TaskClass
        {
            IndexNumber = 0
        };
        public SheduleWindow(SettingsParametrs settingsParametrs)
        {
            InitializeComponent();
            FindRelativityImportance(); // поиск относительной важности задач
            FindTimeToEnd(); // поис время окончания каждой задачи
            SortRelativityImportance(); // сортировка списка задач по убыванию относительной важности
            FirstInsert(); // вставка первой задачи в список-результат

            for (int i = 1; i < TaskManagerClass.ListTasks.Count; i ++)
            {
                //OutputResultListTasks();
                LeftMost(TaskManagerClass.ResultListTasks.Count); // поис крайней левой задачи с которой пересекается очередная
                if (_min.IndexNumber != 0) // если есть крайняя левая задача, то проверяется возможность вставки
                {
                    if (InsertionCapability(settingsParametrs, TaskManagerClass.ResultListTasks.Count)) // если вставка возможна то...
                    {

                    }
                   
                }
            }            
        }

        public void FindRelativityImportance()
        {
            foreach(var task in TaskManagerClass.ListTasks)
            {
                task.RelativityImportance = Math.Round((Math.Pow(task.Importance, 2) / task.TimeToWork),3);
            }          
        }

        public void FindTimeToEnd()
        {
            foreach(var task in TaskManagerClass.ListTasks)
            {
                task.TimeToEnd = task.TimeToStart + task.TimeToWork;
            }
        }

        public void SortRelativityImportance()
        {
            TaskManagerClass.ListTasks = TaskManagerClass.ListTasks.OrderByDescending(l => l.RelativityImportance).ToList();
        }

        public void FirstInsert() // вставка первой задачи в список-результат
        {
            TaskManagerClass.InitializeResultListTasks();
            TaskManagerClass.ResultListTasks.Add(TaskManagerClass.ListTasks[0]);
        }

        public void OutputResultListTasks() // метод вывода ResultListTasks
        {
            for(int task = 0; task < TaskManagerClass.ResultListTasks.Count; task++)
            {
                ResultTasksListTextBox.AppendText("Время поступления " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].TimeToStart + "\n");
                ResultTasksListTextBox.AppendText("Время обработки  " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].TimeToWork + "\n");
                ResultTasksListTextBox.AppendText("Важность " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].Importance + "\n");
                ResultTasksListTextBox.AppendText("Порядковый номер " + (task + 1) + "-ой задачи - " + TaskManagerClass.ResultListTasks[task].IndexNumber + "\n");
            }
        }

        public void LeftMost(int next) // метод, возвращающий крайнюю левую задачу 
        {
            List<TaskClass> number = new List<TaskClass>();

            // next - количество задач в ResultTaskList и в тоже время номер очередной "пришедшей" задачи
            for (int i = 0; i < next; i++)
            {
                // пересечение слева
                bool intersectionRight = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.ListTasks[i].TimeToEnd) &&
                    (TaskManagerClass.ListTasks[next].TimeToStart >= TaskManagerClass.ListTasks[i].TimeToStart);
                // пересечение справа
                bool intersectionLeft = (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.ListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd <= TaskManagerClass.ListTasks[i].TimeToEnd);
                // пршедшая задача "поглощает" пересекаемую 
                bool intersectionRightLeft = (TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.ListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.ListTasks[i].TimeToEnd);

                if ( intersectionLeft || intersectionRight || intersectionRightLeft)
                {
                    number.Add(TaskManagerClass.ListTasks[i]); // в списке number храняться номера задач с которыми
                                                               // пересекается очередная "пришедшая" 
                }
            }

            //this._min = TaskManagerClass.ListTasks[next];
            if (number.Count != 0)
            {
                this._min = number[0];
                if (number.Count >= 2)
                {
                    for (int count = 1; count < number.Count; count++)
                    {
                        if (number[count].TimeToStart < _min.TimeToStart) _min = number[count];
                    }// в min хранится первая задача с которой пересекается очередная "пришедшая" задача

                }
            }
        }

        public bool InsertionCapability(SettingsParametrs settingsParametrs, int next)
        // метод, проверяющий возможность 
        // вставки очередной задачи
        {
            int x = settingsParametrs.DirectTime - _min.TimeToEnd;

            int y = x;
            for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
            {
                if (TaskManagerClass.ResultListTasks[i].TimeToStart > _min.TimeToStart)
                {
                    y -= TaskManagerClass.ResultListTasks[i].TimeToWork;
                }
            }

            bool firstCheck = x >= TaskManagerClass.ListTasks[next].TimeToWork;
            bool secondCheck = y > TaskManagerClass.ListTasks[next].TimeToWork;

            if (firstCheck && secondCheck)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        //public TaskClass ListTasksChanged()
        //{
        //    List<TaskClass> changedTasks = new List<TaskClass>(); // список задач, необходимых сдвивнуть
        //    for (int i = 0; i < TaskManagerClass.ResultListTasks.Count; i++)
        //    {
        //        if (TaskManagerClass.ResultListTasks[i].TimeToStart > _min.TimeToStart)
        //        {
        //            changedTasks.Add(TaskManagerClass.ResultListTasks[i]);
        //        }
        //    }
        //}
    }
}