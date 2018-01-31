using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_test.Enums;

namespace Wpf_test
{
    /// <summary>
    /// класс количесва задач, процессоров, директивного времени и типа ввода параметров задач
    /// </summary>
    public class SettingsParametrs
    {
        public int TaskCounts { get; set; }

        public int ProcCount { get; set; }

        public double DirectTime { get; set; }

        public InputType InputType { get; set; }
    }
}
