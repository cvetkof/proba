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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Wpf_test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int _openedManuallyWindows = 0; // счетчик окон
        public int _countTasks = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateRandValueButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsParametrs = GetSettingsParametrs(Enums.InputType.Random);

            if ((settingsParametrs.DirectTime != 0) && (settingsParametrs.ProcCount != 0) && (settingsParametrs.TaskCounts != 0))
            {
                var parametesWindow = new ParametersWindow(settingsParametrs);
                parametesWindow.Show();
                this.Close();
            }
        }

        private void EnterValueManuallyButton_Click(object sender, RoutedEventArgs e)
        {
            TaskManagerClass.InitializeListTasks();

            var manuallyInpunWindow = new ManuallyInputWindow();
            this._openedManuallyWindows++;
            manuallyInpunWindow.NumberTask.Text = Convert.ToString(this._openedManuallyWindows);
            manuallyInpunWindow.NumberTask_Copy.Text = "-ая задача:";
            manuallyInpunWindow.Show();
            manuallyInpunWindow.Closed += ManuallyInpunWindow_Closed;
            
            this.Close();
        }

        private void ManuallyInpunWindow_Closed(object sender, EventArgs e)
        {
            var settingsParametrs = GetSettingsParametrs(Enums.InputType.Manually);

            this._countTasks++;
            if (this._countTasks == TaskManagerClass.ListTasks.Count)
            {
                if (this._openedManuallyWindows < settingsParametrs.TaskCounts)
                {
                    var manuallyInpunWindow = new ManuallyInputWindow();
                    this._openedManuallyWindows++;

                    manuallyInpunWindow.NumberTask.Text = Convert.ToString(this._openedManuallyWindows);
                    manuallyInpunWindow.NumberTask_Copy.Text = "-ая задача:";
                    manuallyInpunWindow.Show();
                    manuallyInpunWindow.Closed += ManuallyInpunWindow_Closed;
                }
                else
                {
                    var parametesWindow = new ParametersWindow(settingsParametrs);
                    parametesWindow.Show();
                    this.Close();
                }
            }
        }

        private SettingsParametrs GetSettingsParametrs(Enums.InputType inputType) // метод, присваивающий полям объкта значения,
                                                                                  // введеные в начальном окне  
        {
            var settingParametrs = new SettingsParametrs()
            {
                InputType = inputType, // тип ввода значений матрицы
            };

                settingParametrs.TaskCounts = Convert.ToInt32(TaskCountTextBox.Text); // количество задач
                settingParametrs.ProcCount = Convert.ToInt32(ProcCountTextBox.Text); //количество процессоров
                settingParametrs.DirectTime = Convert.ToInt32(DirectTimeTextBox.Text); //директивное время

            return settingParametrs;
        }

        private void TaskCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TaskCountTextBox.Text = Regex.Replace(this.TaskCountTextBox.Text, "[^0-9]+", "");
        }

        private void ProcCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ProcCountTextBox.Text = Regex.Replace(this.ProcCountTextBox.Text, "[^0-9]+", "");
        }

        private void DirectTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.DirectTimeTextBox.Text = Regex.Replace(this.DirectTimeTextBox.Text, "[^0-9]+", "");
        }
    }
}
