﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_test
{
    public class TaskClass //класс задач
    {
        public int TimeToStart { get; set; }

        public int TimeToWork { get; set; }

        public int TimeToEnd { get; set; }

        public double Importance { get; set; }

        public double RelativityImportance { get; set; }

        public double IndexNumber { get; set; }
    }
}
