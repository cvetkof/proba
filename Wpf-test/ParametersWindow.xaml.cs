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
    /// Логика взаимодействия для ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        public ParametersWindow(SettingsParametrs settingsParametrs)
        {
            InitializeComponent();
            FillSettingsValues(settingsParametrs);
        }

        private void FillSettingsValues(SettingsParametrs settingsParametrs)
        {
            ProcCountTextBox.Text = settingsParametrs.ProcCount.ToString();
            DirectTimeTextBox.Text = settingsParametrs.DirectTime.ToString();

            GenerateTable(settingsParametrs);
        }

        private void GenerateTable(SettingsParametrs settingsParametrs)
        {

                if(settingsParametrs.InputType == Enums.InputType.Random)
                {
                    Random rand = new Random();

                    TaskManagerClass.InitializeTaskArray();

                    for (int count = 0; count < settingsParametrs.TaskCounts; count++)
                    {
                        var task = new TaskClass
                        {
                            TimeToStart = rand.Next(1, 5400), //полю (TimeToStart) объекта присваиваю рандомное значение от 1с до 1ч30мин
                            TimeToWork = rand.Next(1, 420), //полю (TimeToWork) объекта присваиваю рандомное значение от 1c до 7мин
                            Importance = rand.Next(1, 100), //полю (Impotyance) объекта присваиваю рандомное значение от 1 до 100
                            IndexNumber = count + 1
                        };

                        TaskManagerClass.ListTasks.Add(task);

                        ParametrsFirstResults.AppendText("время поступления " + (count + 1) + "-ой задачи - " + TaskManagerClass.ListTasks[count].TimeToStart + "\n");
                        ParametrsFirstResults.AppendText("время обработки " + (count + 1) + "-ой задачи    - " + TaskManagerClass.ListTasks[count].TimeToWork + "\n");
                        ParametrsFirstResults.AppendText("важность " + (count + 1) + "-ой задачи                  - " + TaskManagerClass.ListTasks[count].Importance + "\n\n");
                        //ParametrsFirstResults.AppendText("порядковый номер - " + TaskManagerClass.ListTasks[count].IndexNumber + "\n\n");
                    }
                }
                else
                {
                    for (int count = 0; count < settingsParametrs.TaskCounts; count++)
                    {
                    
                        ParametrsFirstResults.AppendText("время поступления " + (count + 1) + "-ой задачи - " + TaskManagerClass.ListTasks[count].TimeToStart + "\n");
                        ParametrsFirstResults.AppendText("время обработки " + (count + 1) + "-ой задачи    - " + TaskManagerClass.ListTasks[count].TimeToWork + "\n");
                        ParametrsFirstResults.AppendText("важность " + (count + 1) + "-ой задачи                  - " + TaskManagerClass.ListTasks[count].Importance + "\n\n");
                        //ParametrsFirstResults.AppendText("порядковый номер - " + TaskManagerClass.ListTasks[count].IndexNumber + "\n\n");
                    }
                }

                
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            
        }

        private void MakeShedule(object sender, RoutedEventArgs e)
        {
            var sheduleWindow = new SheduleWindow();
            //this.WindowState = WindowState.Maximized;
            sheduleWindow.Show();
        }
    }
}
