using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.View
{
    /// <summary>
    /// Interaction logic for NurseWindow.xaml
    /// </summary>
    public partial class HospitalTreatmentCheckInView : Window
    {
        public HospitalTreatmentCheckInView()
        {
            DataContext = new HospitalTreatmentCheckInViewModel(this);
            InitializeComponent();
            
        }


        
    }
}
