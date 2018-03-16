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
    /// Логика взаимодействия для ManuallyInpunWindow.xaml
    /// </summary>
    public partial class ManuallyInputWindow : Window
    {
        public ManuallyInputWindow()
        {
            InitializeComponent();
        }
        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            var task = new TaskClass
            {
                IndexNumber = Convert.ToInt32(NumberTask.Text),
                Guid = Guid.NewGuid()
            };

            if((Int32.TryParse(this.MathTextBox.Text, out int math)) && (Double.TryParse(this.DisprTextBox.Text, out double dispr)) &&
                (Int32.TryParse(this.TimeToWorkTextBox.Text, out int twork)) && (Int32.TryParse(this.ImportanceTextBox.Text, out int imp)))
            {
                task.Mathematic = math;
                task.Dispr = dispr;
                task.TimeToWork = twork;
                task.Importance = imp;

                TaskManagerClass.ListTasks.Add(task);
                this.Close();
            }
            else
            {
                var exceptionWindow = new ExceptionWindow();
                exceptionWindow.Show();
                this.Close();
            }
        }
    }
}
