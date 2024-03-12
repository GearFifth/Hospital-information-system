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

namespace ZdravoCorp.Surveys.Analytics
{
    /// <summary>
    /// Interaction logic for DoctorStatisticsWindow.xaml
    /// </summary>
    public partial class DoctorStatisticsWindow : Window
    {
        public DoctorStatisticsWindow(string doctorUsername)
        {
            InitializeComponent();
            DoctorStatisticsViewModel viewModel = new DoctorStatisticsViewModel(doctorUsername);
            DataContext = viewModel;
        }
    }
}
