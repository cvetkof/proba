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
                Mathematic = Convert.ToInt16(this.MathTextBox.Text),
                Dispr = Convert.ToInt16(this.DisprTextBox.Text),
                TimeToWork = Convert.ToInt16(this.TimeToWorkTextBox.Text),
                Importance = Convert.ToInt16(this.ImportanceTextBox.Text),
                IndexNumber = Convert.ToInt16(NumberTask.Text),
                Guid = Guid.NewGuid()
            };

            TaskManagerClass.ListTasks.Add(task);
            this.Close();
        }
    }
}
