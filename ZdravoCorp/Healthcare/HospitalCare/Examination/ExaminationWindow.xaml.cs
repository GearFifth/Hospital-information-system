using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Presentation;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;
using ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords;
using ZdravoCorp.Healthcare.PatientHealthcare.ReportHistories;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.DoctorView;
using ZdravoCorp.Scheduling.Appointments;

namespace ZdravoCorp.Healthcare.HospitalCare.Examination
{
    /// <summary>
    /// Interaction logic for ExaminationWindow.xaml
    /// </summary>
    public partial class ExaminationWindow : Window
    {
        private readonly Appointment _appointment;
        private readonly DoctorWindow _doctorWindow;
        public ExaminationWindow(Appointment appointment, DoctorWindow dw)
        {
            InitializeComponent();
            this._appointment = appointment;
            this._doctorWindow = dw;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            ReportHistoryService.AddOrEditReport(_appointment.PatientUsername, _appointment.Id, reportTextBox.Text);
            AppointmentService.FinishAppointment(_appointment);

            _doctorWindow.UpdateAppointmentsTable();

            this.Close();

            DrugPrescriptionWindow drugPrescriptionWindow = new(_appointment);
            drugPrescriptionWindow.ShowDialog();
        }

        private void EditMedicalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            EditMedicalRecordWindow editMedicalRecordWindow =
                new(MedicalRecordService.GetMedicalRecord(_appointment.PatientUsername));
            editMedicalRecordWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MedicalRecordWindow medicalRecordWindow =
                new(MedicalRecordService.GetMedicalRecord(_appointment.PatientUsername));
            medicalRecordWindow.Show();
        }

        private void referDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            DoctorReferralWindow doctorReferralWindow = new DoctorReferralWindow(_appointment.PatientUsername);
            doctorReferralWindow.ShowDialog();
        }

        private void referTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            TreatmentReferralWindow treatmentReferralWindow = new TreatmentReferralWindow(_appointment.PatientUsername);
            treatmentReferralWindow.ShowDialog();
        }
    }
}
