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
        private int _openedManuallyWindows = 0;
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
            TaskManagerClass.InitializeTaskArray();


            var manuallyInpunWindow = new ManuallyInputWindow();
            this._openedManuallyWindows++;
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
            int taskCountTextBoxValue = Convert.ToInt16(TaskCountTextBox.Text);

            int procCountTextBoxValue = Convert.ToInt16(ProcCountTextBox.Text);

            double directTimeTextBoxValue = Convert.ToDouble(DirectTimeTextBox.Text);

            var settingParametrs = new SettingsParametrs();
            settingParametrs.ProcCount = procCountTextBoxValue; //количество процессоров
            settingParametrs.DirectTime = directTimeTextBoxValue; //директивное время
            settingParametrs.InputType = inputType; // тип ввода значений матрицы
            settingParametrs.TaskCounts = taskCountTextBoxValue; // количество задач

            return settingParametrs;
        }
    }
}
