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
        public double TimeToStart { get; set; }

        public int TimeToWork { get; set; }

        public double TimeToEnd => TimeToStart + TimeToWork;

        public int Importance { get; set; }

        public int NumberProc { get; set; }

        public Guid Guid { get; set; }

        public double RelativityImportance => Math.Round((Math.Pow(Importance, 2) / TimeToWork), 3);

        public int IndexNumber { get; set; }

        public int ResultIndexNumber { get; set; }

        public double Mathematic { get; set; }

        public double Dispr { get; set; }
    }
}
