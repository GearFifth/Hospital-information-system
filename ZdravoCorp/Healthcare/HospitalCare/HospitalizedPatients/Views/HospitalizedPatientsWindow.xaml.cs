using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.Views
{
    /// <summary>
    /// Interaction logic for HospitalizedPatientsWindow.xaml
    /// </summary>
    public partial class HospitalizedPatientsWindow : Window
    {
        public HospitalizedPatientsWindow()
        {
            InitializeComponent();
            HospitalizedPatientsViewModel hospitalizedPatientsViewModel = new();
            this.DataContext = hospitalizedPatientsViewModel;
        }
    }
}
