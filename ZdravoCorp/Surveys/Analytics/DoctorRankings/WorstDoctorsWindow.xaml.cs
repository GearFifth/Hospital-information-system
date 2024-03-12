using System.Windows;

namespace ZdravoCorp.Surveys.Analytics.DoctorRankings
{
    /// <summary>
    /// Interaction logic for WorstDoctorsWindow.xaml
    /// </summary>
    public partial class WorstDoctorsWindow : Window
    {
        public WorstDoctorsWindow()
        {
            InitializeComponent();
            WorstDoctorsViewModel viewModel = new WorstDoctorsViewModel();
            DataContext = viewModel;
        }
    }
}
