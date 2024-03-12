using System.Windows;

namespace ZdravoCorp.Surveys.Analytics.DoctorRankings
{
    /// <summary>
    /// Interaction logic for BestDoctorsWindow.xaml
    /// </summary>
    public partial class BestDoctorsWindow : Window
    {
        public BestDoctorsWindow()
        {
            InitializeComponent();
            BestDoctorsViewModel viewModel = new BestDoctorsViewModel();
            DataContext = viewModel;
        }
    }
}
