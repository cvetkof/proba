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

namespace Wpf_test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int _openedManuallyWindows = 0; // счетчик окон
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateRandValueButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsParametrs = GetSettingsParametrs(Enums.InputType.Random);
            var parametesWindow = new ParametersWindow(settingsParametrs);
            this.Close();
            parametesWindow.Show();
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
            }

        }

        private SettingsParametrs GetSettingsParametrs(Enums.InputType inputType) // метод, присваивающий полям объкта значения,
                                                                                  // введеные в начальном окне  
        {
            int taskCountTextBoxValue = Convert.ToInt32(TaskCountTextBox.Text);

            int procCountTextBoxValue = Convert.ToInt32(ProcCountTextBox.Text);

            double directTimeTextBoxValue = Convert.ToDouble(DirectTimeTextBox.Text);

            var settingParametrs = new SettingsParametrs()
            {
                ProcCount = procCountTextBoxValue, //количество процессоров
                DirectTime = directTimeTextBoxValue, //директивное время
                InputType = inputType, // тип ввода значений матрицы
                TaskCounts = taskCountTextBoxValue // количество задач
            };
            

            return settingParametrs;
        }
    }
}
