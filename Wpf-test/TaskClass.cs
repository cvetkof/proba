using System;
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

        public int TimeToEnd => TimeToStart + TimeToWork;

        public int Importance { get; set; }

        public double RelativityImportance => Math.Round((Math.Pow(Importance, 2) / TimeToWork), 3);

        public int IndexNumber { get; set; }
    }
}
