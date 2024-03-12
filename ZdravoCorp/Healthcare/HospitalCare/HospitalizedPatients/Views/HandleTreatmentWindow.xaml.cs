using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.Views
{
    /// <summary>
    /// Interaction logic for HandleTreatmentWindow.xaml
    /// </summary>
    public partial class HandleTreatmentWindow : Window
    {
        public HandleTreatmentWindow(HospitalTreatment hospitalTreatment,
            HospitalizedPatientsWindow? hospitalizedPatientsWindow)
        {
            InitializeComponent();
            HandleTreatmentViewModel handleTreatmentViewModel = new(hospitalTreatment, this, hospitalizedPatientsWindow);
            this.DataContext = handleTreatmentViewModel;
        }
    }
}
