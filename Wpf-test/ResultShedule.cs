using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_test
{
    public static class ResultShedule
    {
        public static List<TaskClass> ResultListTasks { get; set; }

        public static void InitialezeResultListTasks()
        {
            ResultListTasks = new List<TaskClass>();
        }
    }
}
