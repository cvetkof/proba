using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_test
{
    public static class TaskManagerClass
    {
        public static List<TaskClass> ListTasks { get; set; } // static список типа TaskArray

        public static void InitializeTaskArray() // метод, инициализирующий элементы списка
        {
            ListTasks = new List<TaskClass>();
        }
    }
}
