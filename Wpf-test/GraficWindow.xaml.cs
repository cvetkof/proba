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
        private SettingsParametrs _settingsParametrs;

        public GraficWindow(SettingsParametrs settingsParametrs)
        {
            this._settingsParametrs = settingsParametrs;

            InitializeComponent();

            Lines(UtilizedProc());

            Rectangle();
        }

        /// <summary>
        /// отрсовка осей графика
        /// </summary>
        /// <param name="countUtilizedProc"> количество задейстованных процессоров </param>
        public void Lines(int countUtilizedProc)
        {
            int k = 0;
            
            for (int i = 0; i < countUtilizedProc; i ++)
            {
                var line = new Line
                {
                    X1 = 10,
                    Y1 = 50 + k,
                    X2 = this._settingsParametrs.DirectTime + 30,
                    Y2 = 50 + k
                };
                k += 30;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                GraficGrid.Children.Add(line);
            }

            var verticalLine = new Line
            {
                X1 = 10,
                Y1 = 15,
                X2 = 10,
                Y2 = k + 20
            };

            var dirLine = new Line
            {
                X1 = this._settingsParametrs.DirectTime + 10,
                Y1 = 15,
                X2 = this._settingsParametrs.DirectTime + 10,
                Y2 = k + 20 
            };

            dirLine.Stroke = Brushes.Black;
            dirLine.StrokeThickness = 1;
            //dirLine.StrokeDashArray;
            GraficGrid.Children.Add(dirLine);

            verticalLine.Stroke = Brushes.Black;
            verticalLine.StrokeThickness = 1;
            GraficGrid.Children.Add(verticalLine);

        }

        public void Rectangle()
        {
            var x = new TaskClass();
            var right = this._settingsParametrs.DirectTime + 30 + 10;
            int k = 0;

            for ( int i = 0; i < TaskManagerClass.ResultListTasks.Count; i ++)
            {
                for (int j = 0; j < TaskManagerClass.ResultListTasks.Count; j ++)
                {
                    if (TaskManagerClass.ResultListTasks[j].NumberProc == i + 1)
                    {
                        x = TaskManagerClass.ResultListTasks[j];

                        var rectangle = new Rectangle();
                        rectangle.Margin = new Thickness(x.TimeToStart + 10, 20 + k, right - x.TimeToWork - x.TimeToStart, 76);
                        rectangle.Fill = Brushes.GreenYellow;
                        rectangle.Stroke = Brushes.Black;
                        GraficGrid.Children.Add(rectangle);
                    }
                }

                k += 30;
            }
        }

        public int UtilizedProc()
        {
            int i = TaskManagerClass.ResultListTasks.Count;
            int utilizedProc = 0;

            if (i > 0)
            {
                utilizedProc = TaskManagerClass.ResultListTasks[i - 1].NumberProc;
            }

            return utilizedProc;
        }
    }
}
