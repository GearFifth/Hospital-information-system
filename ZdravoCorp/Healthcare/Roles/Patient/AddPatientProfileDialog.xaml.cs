using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords;
using ZdravoCorp.Healthcare.PatientHealthcare.ReportHistories;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.Users;
using static ZdravoCorp.MainUI.Users.User;

namespace ZdravoCorp.Healthcare.Roles.Patient
{
    /// <summary>
    /// Interaction logic for AddDoctorAppointmentDialog.xaml
    /// </summary>
    public partial class AddPatientProfileDialog : Window
    {
        private readonly bool _mustCreate;
        private readonly int _appointmentId;
        public AddPatientProfileDialog(bool mustCreate = false, int appointmentId = -1)
        {
            _mustCreate = mustCreate;
            _appointmentId = appointmentId;

            InitializeComponent();
            DisableCancelButton();
            this.ShowDialog();
        }

        private void DisableCancelButton()
        {
            if (_mustCreate) cancelButton.IsEnabled = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsValid())
            {
                Notification.ShowErrorDialog("Username or password are not entered correctly! ");
                
            }

            CreateNewUser();
            this.Close();
        }

        private void CreateNewUser()
        {
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Password.Trim();

            UserService.AddUser(new User(username, password, UserRole.Patient));

            ReportHistoryService.InitializeReportHistory(username);
            ReportHistoryService.AddOrEditReport(username, _appointmentId, "");

            PatientService.AddPatient(new Patient("","",username));

            MedicalRecordService.AddMedicalRecord(new MedicalRecord(0.0, 0.0, username, DateOnly.MaxValue, new List<string>()));

            OpenMedicalRecordWindow(username, _appointmentId);
        }

        

        private static void OpenMedicalRecordWindow(string username, int appointmentId)
        {
            EditMedicalRecordWindow editMedicalRecordWindow = new (MedicalRecordService.GetMedicalRecord(username)!, appointmentId);
            editMedicalRecordWindow.ShowDialog();
        }

        private bool AreFieldsValid()
        {
            string username = usernameTextBox.Text.Trim();
            if (username.Length < 3 && UserService.DoesUsernameAlreadyExist(username))
            {
                return false;
            }

            string password = passwordTextBox.Password.Trim();
            if (password.Length < 3)
            {
                return false;
            }
            return true;
        }


        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
