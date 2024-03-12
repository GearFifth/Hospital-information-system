using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Healthcare.HospitalCare.Examination;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.Views;
using ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI.Notices;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Scheduling.Appointments.Add;
using ZdravoCorp.Scheduling.Appointments.Edit;

namespace ZdravoCorp.MainUI.UserWindows.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {

        public DoctorWindow()
        {
            InitializeComponent();
            DoctorWindowViewModel doctorWindowViewModel = new(this);
            this.DataContext = doctorWindowViewModel;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Globals.LoggedUser = null;
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DoctorAddAppointmentForm doctorAddAppointmentForm = new(this);
            doctorAddAppointmentForm.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = AppointmentService.GetAppointment(GetSelectedAppointmentId());

                if (appointment.Status != Appointment.AppointmentStatus.Active)
                {
                    throw new InvalidOperationException("You can't edit non-active appointment.");
                }

                DoctorEditAppointmentForm doctorEditAppointmentForm = new(this, appointment);
                doctorEditAppointmentForm.ShowDialog();
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = GetSelectedAppointmentId();

                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    AppointmentService.CancelAppointment(id);
                    UpdateAppointmentsTable();
                }
            }
            catch (InvalidOperationException error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private int GetSelectedAppointmentId()
        {
            var selectedRow = appointmentsDataGrid.SelectedItem as DataRowView;
            if (selectedRow == null)
            {
                throw new InvalidOperationException("Please select a row.");
            }
            return int.Parse(selectedRow.Row.ItemArray[0].ToString());
        }

        private string GetSelectedPatientUsername()
        {
            var selectedRow = patientsDataGrid.SelectedItem as DataRowView;
            if (selectedRow == null)
            {
                throw new InvalidOperationException("Please select a row.");
            }

            return selectedRow.Row.ItemArray[0].ToString();
        }

        private void DoctorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loggedUsernameLabel.Content = Globals.LoggedUser.Username;
            selectedDatePicker.SelectedDate = DateTime.Today;
            UpdateAppointmentsTable();
            UpdatePatientsTable();
            UpdateNoticeTable();
        }

        private void UpdateNoticeTable()
        {
            noticeDataGrid.ItemsSource = NoticeService.GetAllUsersNotices(Globals.LoggedUser.Username);
        }

        public void UpdateAppointmentsTable()
        {
            DataTable dataTable = InitAppointmentsTableColumns();

            DateTime pickedDate = selectedDatePicker.SelectedDate.Value.Date;
            foreach (Appointment app in AppointmentService.GetAppointmentsForNextThreeDays(pickedDate))
            {
                dataTable.Rows.Add(app.ToTable());
            }

            appointmentsDataGrid.ItemsSource = new DataView(dataTable);
        }

        public void UpdatePatientsTable()
        {
            DataTable dataTable = InitPatientsTableColumns();

            foreach (Patient patient in PatientService.GetAllPatients())
            {
                dataTable.Rows.Add(patient.ToTable());
            }

            patientsDataGrid.ItemsSource = new DataView(dataTable);
        }


        private static DataTable InitAppointmentsTableColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Start", typeof(string));
            dt.Columns.Add("End", typeof(string));
            dt.Columns.Add("Doctor username", typeof(string));
            dt.Columns.Add("Patient username", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Room name", typeof(string));
            return dt;
        }

        private static DataTable InitPatientsTableColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            return dt;
        }


        private void SelectedDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAppointmentsTable();
        }

        private void AppointmentShowMedicalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment selectedAppointment = AppointmentService.GetAppointment(GetSelectedAppointmentId());
                MedicalRecordWindow medicalRecordWindow = new(MedicalRecordService.GetMedicalRecord(selectedAppointment.PatientUsername));
                medicalRecordWindow.ShowDialog();
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private void PatientShowMedicalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordWindow medicalRecordWindow = new(MedicalRecordService.GetMedicalRecord(GetSelectedPatientUsername()));
            medicalRecordWindow.ShowDialog();
        }

        private void PatientEditMedicalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            EditMedicalRecordWindow editMedicalRecordWindow = new(MedicalRecordService.GetMedicalRecord(GetSelectedPatientUsername()));
            editMedicalRecordWindow.ShowDialog();
        }

        private void PatientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentService.HasPatientBeenToDoctor(GetSelectedPatientUsername(), Globals.LoggedUser.Username))
            {
                patientShowMedicalRecordButton.IsEnabled = true;
                patientEditMedicalRecordButton.IsEnabled = true;
                return;
            }
            patientShowMedicalRecordButton.IsEnabled = false;
            patientEditMedicalRecordButton.IsEnabled = false;
        }
        private void ExamineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = AppointmentService.GetAppointment(GetSelectedAppointmentId());
                AppointmentService.ValidateAppointmentBeforeExamination(appointment);

                ExaminationWindow examinationWindow = new(appointment, this);
                examinationWindow.ShowDialog();
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private void viewHospitalizedPatientsButton_Click(object sender, RoutedEventArgs e)
        {
            HospitalizedPatientsWindow hospitalizedPatientsWindow = new();
            hospitalizedPatientsWindow.ShowDialog();
        }
    }
}
