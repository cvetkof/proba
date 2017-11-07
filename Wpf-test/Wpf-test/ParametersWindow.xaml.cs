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

        public void GenerateTable(SettingsParametrs settingsParametrs)
        {
            if (settingsParametrs.InputType == Enums.InputType.Manually)
            {
                ParametrsFirstResults.Text = "В ручную";
            }
            else
            {
                Random rand = new Random();
                TaskClass[] taskArray = new TaskClass[settingsParametrs.TaskCounts]; // массив объектов, каждый из которых представляет задачу
                
                for (int count = 0; count < settingsParametrs.TaskCounts; count++)
                {
                    taskArray[count] = new TaskClass(); //создаю объкты
                }

                for(int count = 0; count < settingsParametrs.TaskCounts; count++)
                {
                    taskArray[count].TimeToStart = rand.Next(1,5400);// полю объекта (TimeToStart) присваиваю рандомное значение от 1с до 1ч30мин
                    taskArray[count].TimeToWork = rand.Next(1, 420);// полю объекта (TimeToWork) присваиваю рандомное значение от 1c до 7мин
                    taskArray[count].Importance = rand.Next(1, 100);// полю объекта (Impotyance) присваиваю рандомное значение от 1 до 100
                    
                    ParametrsFirstResults.AppendText("время поступления " + (count + 1) + "-ой задачи - " + Convert.ToString(taskArray[count].TimeToStart) + "\n");
                    ParametrsFirstResults.AppendText("время обработки " + (count + 1) + "-ой задачи    - " + Convert.ToString(taskArray[count].TimeToWork) + "\n");
                    ParametrsFirstResults.AppendText("важность " + (count + 1) + "-ой задачи                  - " + Convert.ToString(taskArray[count].Importance) + "\n\n");                        
                }
            }
        }

        public void GoBack(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void MakeShedule(object sender, RoutedEventArgs e)
        {
            var sheduleWindow = new SheduleWindow();
            sheduleWindow.ShowDialog();
        }
    }
}
