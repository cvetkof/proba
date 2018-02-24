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
        public int TimeToStart
        {
            get; set;
            //get
            //{
            //    return this._timeToStart;
            //}
            //set
            //{
            //    this._timeToStart = value;
            //}
        }

        public int TimeToWork { get; set; }

        public int TimeToEnd => TimeToStart + TimeToWork;

        public int Importance { get; set; }

        public int NumberProc { get; set; }

        public Guid Guid { get; set; }

        public double RelativityImportance => Math.Round((Math.Pow(Importance, 2) / TimeToWork), 3);

        public int IndexNumber { get; set; }

        public int ResultIndexNumber { get; set; }

        public int Mathematic { get; set; }

        public int Dispr { get; set; }

        private int _timeToStart;
    }
}
