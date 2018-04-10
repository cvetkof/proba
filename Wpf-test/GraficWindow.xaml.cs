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
        private Brush[] _brushes;
        private Random _randColor;

        public GraficWindow(SettingsParametrs settingsParametrs)
        {
            this._settingsParametrs = settingsParametrs;
            this._randColor = new Random();

            this._brushes = new Brush[]
            {
                Brushes.AliceBlue,
                Brushes.AntiqueWhite,
                Brushes.Aqua,
                Brushes.Aquamarine,
                Brushes.Beige,
                Brushes.Bisque,
                Brushes.BlanchedAlmond,
                Brushes.BurlyWood,
                Brushes.CadetBlue,
                Brushes.CornflowerBlue,
                Brushes.Cornsilk,
                Brushes.Cyan,
                Brushes.DarkGoldenrod,
                Brushes.DarkKhaki,
                Brushes.DarkSalmon,
                Brushes.DarkSeaGreen,
                Brushes.DarkTurquoise,
                Brushes.DeepSkyBlue,
                Brushes.Khaki,
                Brushes.LemonChiffon,
                Brushes.LightBlue,
                Brushes.LightGoldenrodYellow,
                Brushes.LightGreen,
                Brushes.LightSeaGreen
            };

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
                    X2 = 10 + this._settingsParametrs.DirectTime + 40,
                    Y2 = 50 + k
                };

                var lable = new Label();
                lable.Margin = new Thickness(30 + this._settingsParametrs.DirectTime, 25 + k, 0, 600 - (60 + k));
                lable.Content = "t";
                GraficGrid.Children.Add(lable);

                k += 50;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                GraficGrid.Children.Add(line);


            }

            var verticalLine = new Line
            {
                X1 = 10,
                Y1 = 10,
                X2 = 10,
                Y2 = k
            };

            var dirLine = new Line
            {
                X1 = this._settingsParametrs.DirectTime + 10,
                Y1 = 10,
                X2 = this._settingsParametrs.DirectTime + 10,
                Y2 = k 
            };

            dirLine.Stroke = Brushes.Black;
            dirLine.StrokeThickness = 1;
            dirLine.StrokeDashArray = new DoubleCollection() { 2, 1 };
            GraficGrid.Children.Add(dirLine);

            verticalLine.Stroke = Brushes.Black;
            verticalLine.StrokeThickness = 1;
            GraficGrid.Children.Add(verticalLine);
        }

        public void Rectangle()
        {
            var x = new TaskClass();
            var dirt = this._settingsParametrs.DirectTime;
            int k = 0;

            for ( int i = 0; i < TaskManagerClass.ResultListTasks.Count; i ++)
            {

                for (int j = 0; j < TaskManagerClass.ResultListTasks.Count; j ++)
                {
                    if (TaskManagerClass.ResultListTasks[j].NumberProc == i + 1)
                    {
                        x = TaskManagerClass.ResultListTasks[j];

                        var rectangle = new Rectangle();
                        rectangle.Margin = new Thickness(x.TimeToStart + 10, 20 + k, dirt - x.TimeToEnd + 40, 600 - (58 + k));
                        rectangle.Fill = this.GetRandomColor();
                        rectangle.Stroke = Brushes.Black;
                        GraficGrid.Children.Add(rectangle);

                        var label = new Label();

                        //label.Margin = new Thickness(x.TimeToStart + 10, 20 + k, dirt - (x.TimeToEnd + 20), 600 - (55 + k));

                        if (x.TimeToWork < 21)
                        {
                            label.Margin = new Thickness(x.TimeToStart + 4, 1 + k, dirt - (x.TimeToEnd + 40), 600 - (55 + k));
                        }
                        else
                        {
                            label.Margin = new Thickness(x.TimeToStart + 8, 20 + k, dirt - (x.TimeToEnd + 40), 600 - (55 + k));
                        }

                        label.Content = x.IndexNumber;                        
                        GraficGrid.Children.Add(label);
                    }
                }

                k += 50;
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


        public Brush GetRandomColor()
        {
            var index = this._randColor.Next(this._brushes.Length);

            return this._brushes[index];
        }
    }
}
