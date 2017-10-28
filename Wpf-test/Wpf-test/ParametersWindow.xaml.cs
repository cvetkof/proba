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
            ProcCountTextBlock.Text = settingsParametrs.ProcCount.ToString();
            DirectTimeTextBlock.Text = settingsParametrs.DirectTime.ToString();

            GenerateTable(settingsParametrs);
        }

        private void GenerateTable(SettingsParametrs settingsParametrs)
        {
            if (settingsParametrs.InputType == Enums.InputType.Manually)
            {

            }
            else
            {

            }






        }
    }
}
