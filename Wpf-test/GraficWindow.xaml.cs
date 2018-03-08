using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_test
{
    /// <summary>
    /// Логика взаимодействия для GraficWindow.xaml
    /// </summary>
    public partial class GraficWindow : Window
    {
        public GraficWindow()
        {
            InitializeComponent();

            Lines(UtilizedProc());

            var rectangle = new Rectangle();
            rectangle.Margin = new Thickness(10, 10, 10, 10);
            rectangle.Fill = Brushes.GreenYellow;
        }

        public void Lines(int countUtilizedProc)
        {
            for (int i = 0; i < countUtilizedProc; i ++)
            {
                var line = new Line
                {
                    X1 = 10,
                    Y1 = i + 10,
                    X2 = 100,
                    Y2 = 10
                };
            }
        }

        public int UtilizedProc()
        {
            int i = TaskManagerClass.ResultListTasks.Count;
            int utilizedProc = TaskManagerClass.ResultListTasks[i - 1].NumberProc;
            return utilizedProc;
        }
    }
}
