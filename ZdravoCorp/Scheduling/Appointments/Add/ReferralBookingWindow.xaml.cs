using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Service;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.NurseVIew;

namespace ZdravoCorp.Scheduling.Appointments.Add
{
    public partial class ReferralBookingWindow : Window
    {
        public ReferralBookingWindow()
        {
            InitializeComponent();
            loggedUsernameLabel.Content = Globals.LoggedUser!.Username;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NurseWindow nurseWindow = new();
            nurseWindow.Show();
            this.Close();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window == mainWindow) continue;
                
                window.Close();
            }
            Globals.LoggedUser = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeDataGrids();
        }

        private void InitializeDataGrids()
        {
            patientDataGrid.ItemsSource = PatientService.GetAllActivePatients();
            referralsDataGrid.ItemsSource = new List<DoctorReferral>();
        }
        
        private bool IsPatientSelected()
        {
            if (patientDataGrid.SelectedItem != null) return true;

            return false;
        }

        private bool IsReferralSelected()
        {
            if (referralsDataGrid.SelectedItem != null) return true;
            
            Notification.ShowErrorDialog("Please first select the  referral! ");
            return false;
        }

        private void PatientDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!IsPatientSelected()) return;

            var selectedPatient = (patientDataGrid.SelectedItem as Patient)!;
            var patientsReferrals = DoctorReferralService.GetPatientsUnusedDoctorReferrals(selectedPatient.Username);
            
            referralsDataGrid.ItemsSource = patientsReferrals;
        }

        private void BookExaminationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsReferralSelected()) return;

            DoctorReferral selectedReferral = (referralsDataGrid.SelectedItem as DoctorReferral)!;

            if (IsReferralAlreadyUsed(selectedReferral)) return;

            selectedReferral.BookExaminationAppointment();

            selectedReferral.IsUsed = true;
            DoctorReferralService.SaveDoctorReferrals();

            RefreshThisWindow();
        }

        private static bool IsReferralAlreadyUsed(DoctorReferral selectedReferral)
        {
            if (!selectedReferral.IsUsed) return false;

            Notification.ShowErrorDialog("The selected referral has already been used! ");
            return true;

        }

        private void RefreshThisWindow()
        {
            ReferralBookingWindow referralBookingWindow = new();
            referralBookingWindow.Show();
            this.Close();
        }
    }
}
