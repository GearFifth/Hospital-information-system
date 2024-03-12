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
using ZdravoCorp.MainUI.UserWindows;

namespace ZdravoCorp.Surveys.Analytics
{
    /// <summary>
    /// Interaction logic for HospitalStatisticsWindow.xaml
    /// </summary>
    public partial class HospitalStatisticsWindow : Window
    {
        public HospitalStatisticsWindow()
        {
            InitializeComponent();
            HospitalStatisticsViewModel viewModel = new HospitalStatisticsViewModel();
            this.DataContext = viewModel;
        }
    }
}
