using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_test
{
    public static class TaskManagerClass
    {
        public static List<TaskClass> ListTasks { get; set; } // список первоначального расположения задач
        
        public static List<TaskClass> ResultListTasks { get; set; } // результирующий список

        public static void InitializeListTasks() // метод, инициализирующий элементы списка
        {
            ListTasks = new List<TaskClass>();
        }

        public static void InitializeResultListTasks()
        {
            ResultListTasks = new List<TaskClass>();
        }
    }
}
