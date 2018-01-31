using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_test
{
    public static class TaskManagerClass
    {
        /// <summary>
        /// список первоначального расположения задач
        /// </summary>
        public static List<TaskClass> ListTasks { get; set; } 

        /// <summary>
        /// результирующий список
        /// </summary>
        public static List<TaskClass> ResultListTasks { get; set; }

        /// <summary>
        /// промежуточный список
        /// </summary>
        public static List<TaskClass> MiddleResultListTasks { get; set; }

        /// <summary>
        /// инициализация элементов списка
        /// </summary>
        public static void InitializeListTasks()
        {
            ListTasks = new List<TaskClass>();
        }

        /// <summary>
        /// инициализация элементов списка
        /// </summary>
        public static void InitializeResultListTasks()
        {
            ResultListTasks = new List<TaskClass>();
        }

        /// <summary>
        /// инициализация элементов списка
        /// </summary>
        public static void InitializeMiddleResultListTasks()
        {
            MiddleResultListTasks = new List<TaskClass>();
        }
    }
}
