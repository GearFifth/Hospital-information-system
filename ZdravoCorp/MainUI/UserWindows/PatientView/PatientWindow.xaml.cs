using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords;
using ZdravoCorp.Healthcare.PatientHealthcare.ReportHistories;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI.Notices;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Scheduling.Appointments.Add;
using ZdravoCorp.Scheduling.Appointments.AdvancedAdd;
using ZdravoCorp.Scheduling.Appointments.Edit;

namespace ZdravoCorp.MainUI.UserWindows.PatientView
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        public PatientWindow()
        {
            InitializeComponent();

            PatientViewModel patientViewModel = new();
            this.DataContext = patientViewModel;

            loggedUsernameLabel.Content = Globals.LoggedUser.Username;
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
            AddPatientAppointmentDialog addPatientAppointmentDialog = new(this, null);
            addPatientAppointmentDialog.Show();
            this.IsEnabled = false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = AppointmentService.GetAppointment(GetSelectedAppointmentId());
                AppointmentService.ValidateBeforeEditOrCancel(appointment);
                OpenEditAppointmentForm(appointment);
            }
            catch(Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = GetSelectedAppointmentId();
                Appointment appointment = AppointmentService.GetAppointment(id);

                AppointmentService.ValidateBeforeEditOrCancel(appointment);
                AppointmentService.PatientCancelAppointment(id);
                UpdateAppointmentsTable();

                if (AppointmentService.CountPatientCanceledAppointments(Globals.LoggedUser.Username) >= 5)
                {
                    PatientService.BlockPatient(Globals.LoggedUser.Username);
                    LogOutBlockedPatient();
                }
                else
                {
                    Notification.ShowSuccessDialog("Succesfully cancelled appointment!");
                }
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        public void LogOutBlockedPatient()
        {
            Globals.LoggedUser = null;
            MainWindow mainWindow = new();
            mainWindow.Show();

            Notification.ShowErrorDialog("Your account has been permanently disabled for canceling 5 or more appointments.");

            this.Close();
        }

        private void OpenEditAppointmentForm(Appointment selectedAppointment)
        {
            EditPatientAppointmentDialog editPatientAppointmentDialog = new(this, selectedAppointment);
                editPatientAppointmentDialog.Show();
                this.IsEnabled = false;
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

        private void patientWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*UpdateAppointmentsTable();*/
            UpdateReportHistoryTable(false, null);
            LoadNotificationLeadTime();
            UpdateNoticeTable();
            UpdateDoctorsTable(false, "");
            NewNoticePopUp();
        }

        private void LoadNotificationLeadTime()
        {
            Patient patient = PatientService.GetPatient(Globals.LoggedUser.Username);
            notificationLeadTimeTextBox.Text = patient.NotificationLeadTime.ToString();
        }

        private void UpdateNoticeTable()
        {
            TimeSpan notificationLeadTime = TimeSpan.Parse(notificationLeadTimeTextBox.Text);
            noticeDataGrid.ItemsSource = NoticeService.GetAllUsersNoticesInTimeRange(Globals.LoggedUser.Username, DateTime.Now + notificationLeadTime);
        }

        private void NewNoticePopUp()
        {
            TimeSpan notificationLeadTime = TimeSpan.Parse(notificationLeadTimeTextBox.Text);
            List<Notice> patientNotices = NoticeService.GetAllUsersNoticesInTimeRange(Globals.LoggedUser.Username, DateTime.Now + notificationLeadTime);
            if (patientNotices.Count > 0)
            {
                Notification.ShowWarningDialog("You have new notices!");
            }
        }


        public void UpdateAppointmentsTable()
        {
            DataTable dataTable = InitAppointmentTableColumns();

            foreach (Appointment app in AppointmentService.GetAllAppointments())
            {
                if (app.PatientUsername == Globals.LoggedUser.Username)
                {
                    dataTable.Rows.Add(app.ToTable());
                }
            }

            appointmentsDataGrid.ItemsSource = new DataView(dataTable);
        }

        private static DataTable InitAppointmentTableColumns()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("Start", typeof(string));
            dataTable.Columns.Add("End", typeof(string));
            dataTable.Columns.Add("Doctor username", typeof(string));
            dataTable.Columns.Add("Patient username", typeof(string));
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("Room name", typeof(string));

            return dataTable;
        }

        public void UpdateReportHistoryTable(bool filter, string searchText)
        {
            DataTable dataTable = InitReportHistoryTableColumns();

            Dictionary<int, string> reportHistory = ReportHistoryService.GetReportHistory(Globals.LoggedUser.Username);
            foreach (KeyValuePair<int, string> report in reportHistory)
            {
                Appointment appointment = AppointmentService.GetAppointment(report.Key);
                string[] tableValues =
                {
                    report.Key.ToString(),
                    appointment.TimeSlot.Start.ToString(),
                    appointment.DoctorUsername,
                    DoctorService.GetDoctor(appointment.DoctorUsername).Specialization.ToString(),
                    report.Value
                };
                if (!filter || (filter && report.Value.Contains(searchText)))
                {
                    dataTable.Rows.Add(tableValues);
                }
            }
            reportHistoryDataGrid.ItemsSource = new DataView(dataTable);
        }

        private static DataTable InitReportHistoryTableColumns()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("Start", typeof(string));
            dataTable.Columns.Add("Doctor", typeof(string));
            dataTable.Columns.Add("Doctor specialization", typeof(string));
            dataTable.Columns.Add("Medical report", typeof(string));

            return dataTable;
        }

        private void advancedSearchButton_Click(object sender, RoutedEventArgs e)
        {
            AdvancedAddPatientAppointmentForm advancedAddAppointment = new AdvancedAddPatientAppointmentForm(this);
            advancedAddAppointment.ShowDialog();
        }

        private void AppointmentShowMedicalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordWindow medicalRecordWindow =
                new MedicalRecordWindow(MedicalRecordService.GetMedicalRecord(Globals.LoggedUser.Username));
            medicalRecordWindow.ShowDialog();
        }

        private void searchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateReportHistoryTable(true, searchTextBox.Text);
        }

        public void UpdateDoctorsTable(bool filter, string searchText)
        {
            DataTable dataTable = InitDoctorsTableColumns();

            foreach (Doctor doctor in DoctorService.GetAllDoctors())
            {
                if (!filter || (filter && doctor.FirstName.Contains(searchText) || doctor.LastName.Contains(searchText) || doctor.Specialization.ToString().Contains(searchText)))
                {
                    dataTable.Rows.Add(doctor.ToTable());
                }
            }

            doctorsDataGrid.ItemsSource = new DataView(dataTable);
        }

        private static DataTable InitDoctorsTableColumns()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("First name", typeof(string));
            dataTable.Columns.Add("Last name", typeof(string));
            dataTable.Columns.Add("Specialization", typeof(string));
            dataTable.Columns.Add("Rating", typeof(string));

            return dataTable;
        }

        private void searchDoctorsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateDoctorsTable(true, searchDoctorsTextBox.Text);
        }

        private void MakeAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string doctorUsername = GetSelectedDoctorUsername();
                AddPatientAppointmentDialog addAppointmentDialog =
                    new AddPatientAppointmentDialog(this, doctorUsername);
                addAppointmentDialog.ShowDialog();

            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }

        }

        private string GetSelectedDoctorUsername()
        {
            var selectedRow = doctorsDataGrid.SelectedItem as DataRowView;
            if (selectedRow == null)
            {
                throw new InvalidOperationException("Please select a row.");
            }
            return selectedRow.Row.ItemArray[0].ToString();
        }
        
        private void SaveNotificationLeadTimeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan time = TimeSpan.Parse(notificationLeadTimeTextBox.Text);
                Patient patient = PatientService.GetPatient(Globals.LoggedUser.Username);
                patient.SetNotificationLeadTime(time);
                UpdateNoticeTable();
                Notification.ShowSuccessDialog("Succesfully changed notification lead time");
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog("Invalid format. Please use 'HH:mm:ss'");
            }
        }

        private void AddNotice_Click(object sender, RoutedEventArgs e)
        {
            AddCustomNoticeWindow addCustomNoticeWindow = new AddCustomNoticeWindow();
            addCustomNoticeWindow.ShowDialog();
        }
    }
}