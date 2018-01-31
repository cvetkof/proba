using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_test
{
    /// <summary>
    /// класс задач
    /// </summary>
    public class TaskClass 
    {
        public int TimeToStart { get; set; }

        public int TimeToWork { get; set; }

        public int TimeToEnd => TimeToStart + TimeToWork;

        public int Importance { get; set; }

        public int NumberProc { get; set; }

        public double RelativityImportance => Math.Round((Math.Pow(Importance, 2) / TimeToWork), 3);

        public int IndexNumber { get; set; }
    }
}
