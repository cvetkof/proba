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
    /// Логика взаимодействия для SheduleWindow.xaml
    /// </summary>
    public partial class SheduleWindow : Window
    {
        public SheduleWindow()
        {
            InitializeComponent();

            var list = TaskManagerClass.ListTasks;
           
        }

        public void FindImportance()
        {
            var tasksList = TaskManagerClass.ListTasks;

            foreach(var task in tasksList)
            {
                task.RelativityImportance = Math.Pow(task.Importance, 2) / task.TimeToWork;
            }
            
        }

        public void Method2()
        {

        }



    }

    

}
