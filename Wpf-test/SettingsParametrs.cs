using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_test.Enums;

namespace Wpf_test
{

    public class SettingsParametrs // класс количесва задач, процессоров, директивного времени и типа ввода параметров задач
    {
        public int TaskCounts { get; set; }

        public int ProcCount { get; set; }

        public int DirectTime { get; set; }

        public InputType InputType { get; set; }
    }
}
