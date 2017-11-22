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
        public SheduleWindow()
        {
            InitializeComponent();
            FindRelativityImportance();
            SortRelativityImportance();
            //FirstInsert();
        }

        public void FindRelativityImportance()
        {
            foreach(var task in TaskManagerClass.ListTasks)
            {
                task.RelativityImportance = Math.Round((Math.Pow(task.Importance, 2) / task.TimeToWork),3);
            }          
        }

        public void SortRelativityImportance()
        {

            TaskManagerClass.ListTasks = TaskManagerClass.ListTasks.OrderByDescending(l => l.RelativityImportance).ToList();

            //for (int i = 0; i < TaskManagerClass.ListTasks.Count; i++)
            //    ParametrsSecondResult.AppendText("удельная важность " + TaskManagerClass.ListTasks[i].IndexNumber + "-ой задачи - " + TaskManagerClass.ListTasks[i].RelativityImportance + "\n\n");

            //ParametrsSecondResult.AppendText("\n\n");

            //for (int i = 0; i < TaskManagerClass.ListTasks.Count; i++)
            //{
            //    ParametrsSecondResult.AppendText("важность " + TaskManagerClass.ListTasks[i].IndexNumber + "-ой задачи - " + TaskManagerClass.ListTasks[i].Importance + "\n\n");
            //}
        }

        //public void FillingResultListTasks()
        //{
        //    ResultShedule.InitialezeResultListTasks();
        //}

        //public void FirstInsert() // вставка первой задачи в результат
        //{
        //    ResultShedule.ResultListTasks[0] = TaskManagerClass.ListTasks[0];
        //}

        public void NextInsert(int next)
        {
            List<int> number = new List<int>();

            for (int i = 0; i < next; i++)
            {
                if (((TaskManagerClass.ListTasks[next].TimeToStart < TaskManagerClass.ListTasks[i].TimeToEnd) &&
                    (TaskManagerClass.ListTasks[next].TimeToStart > TaskManagerClass.ListTasks[i].TimeToStart)) ||
                    ((TaskManagerClass.ListTasks[next].TimeToEnd > TaskManagerClass.ListTasks[i].TimeToStart) &&
                    (TaskManagerClass.ListTasks[next].TimeToEnd < TaskManagerClass.ListTasks[i].TimeToEnd)))
                {
                    number.Add(i);
                }
            }
        }

        //public void P1() // поиск крайней левой задачи с которой он пересекается
        //{
        //    for (int i = 0; i < ResultShedule.ResultListTasks.Count; i++)
        //    {

        //    }
        //}
    }
}