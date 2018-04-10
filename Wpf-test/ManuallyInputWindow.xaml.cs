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
using System.Text.RegularExpressions;


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

            task.Mathematic = Convert.ToInt32(MathTextBox.Text);
            task.Dispr = Convert.ToInt32(DisprTextBox.Text);
            task.TimeToWork = Convert.ToInt32(TimeToWorkTextBox.Text);
            task.Importance = Convert.ToInt32(ImportanceTextBox.Text);

            TaskManagerClass.ListTasks.Add(task);
            this.Close();
        }

        private void MathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.MathTextBox.Text = Regex.Replace(this.MathTextBox.Text, "[^0-9]+", "");
        }


        private void DisprTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.DisprTextBox.Text = Regex.Replace(this.DisprTextBox.Text, "[^0-9]+", "");
        }

        private void TimeToWorkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TimeToWorkTextBox.Text = Regex.Replace(this.TimeToWorkTextBox.Text, "[^0-9]+", "");
        }

        private void ImportanceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ImportanceTextBox.Text = Regex.Replace(this.ImportanceTextBox.Text, "[^0-9]+", "");
        }
    }
}
