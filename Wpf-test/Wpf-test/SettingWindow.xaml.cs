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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateRandValueButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsParametrs = GetSettingsParametrs(Enums.InputType.Random);

            var parametesWindow = new ParametersWindow(settingsParametrs); // Создание объекта нового окна с параметрами

            parametesWindow.Show(); // Открытие окна с параметрами

            this.Close(); //ЗАкрытие окна с настройками 
            
        }

        private void EnterValueManuallyButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsParametrs = GetSettingsParametrs(Enums.InputType.Manually);

            var parametesWindow = new ParametersWindow(settingsParametrs); // Создание объекта нового окна с параметрами

            parametesWindow.Show(); // Открытие окна с параметрами

            this.Close(); //ЗАкрытие окна с настройками 

        }

        private SettingsParametrs GetSettingsParametrs(Enums.InputType inputType)
        {
            var taskCountTextBoxValue = Convert.ToInt16(TaskCountTextBox.Text); // ввод только цифр

            var procCountTextBoxValue = Convert.ToInt16(ProcCountTextBox.Text); // ввод только цыфр

            var directTimeTextBoxValue = Convert.ToDouble(DirectTimeTextBox.Text);

            var settingParametrs = new SettingsParametrs();
            settingParametrs.TaskCounts = taskCountTextBoxValue;
            settingParametrs.ProcCount = procCountTextBoxValue;
            settingParametrs.DirectTime = directTimeTextBoxValue;
            settingParametrs.InputType = inputType;

            return settingParametrs;
        }
    }
}
