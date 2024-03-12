using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.Examination;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.View;
using ZdravoCorp.Healthcare.Pharmacy.Selling;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.Scheduling.Appointments.Add;
using ZdravoCorp.Scheduling.EmergencyAppointments;

namespace ZdravoCorp.MainUI.UserWindows.NurseVIew
{
    /// <summary>
    /// Interaction logic for NurseWindow.xaml
    /// </summary>
    public partial class NurseWindow : Window
    {
        public NurseWindow()
        {
            InitializeComponent();
            NurseWindowViewModel nurseWindowViewModel = new NurseWindowViewModel(this);
            this.DataContext = nurseWindowViewModel;
            loggedUsernameLabel.Content = Globals.LoggedUser.Username;
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();

            Globals.LoggedUser = null;
            this.Close();
        }

        private void UpdatePatientBtn_Click(object sender, RoutedEventArgs e)
        {
            CRUDPatientsWindow crudPatientsWindow = new();
            crudPatientsWindow.Show();

            this.Close();
        }

        private void CheckInPatientBtn_Click(object sender, RoutedEventArgs e)
        {
            PatientCheckInWindow patientCheckInWindow = new();
            patientCheckInWindow.Show();

            this.Close();
        }

        private void EmergencyAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            EmergencyAppointmentWindow emergencyAppointmentWindow = new();
            emergencyAppointmentWindow.Show();
            this.Close();
        }

        private void ReferralBookingBtn_Click(object sender, RoutedEventArgs e)
        {
            ReferralBookingWindow referralBookingWindow = new();
            referralBookingWindow.Show();
            this.Close();
        }

        private void DrugPrescriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            SupplyingPrescribedMedicationWindow supplyingMedicationWindow = new();
            supplyingMedicationWindow.Show();
            this.Close();
        }

        private void HospitalTreatmentCheckInBtn_Click(object sender, RoutedEventArgs e)
        {
            HospitalTreatmentCheckInView hospitalTreatmentCheckInWindow = new();
            hospitalTreatmentCheckInWindow.Show();
            this.Close();
        }

        private void hospitalTreatmentVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            HospitalTreatmentVisitView hospitalTreatmentVisitView = new();
            hospitalTreatmentVisitView.Show();
            this.Close();
        }
    }
}
