using System;
using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Service;
using ZdravoCorp.MainUI.NotificationDialogs;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Presentation
{
    /// <summary>
    /// Interaction logic for TreatmentReferralWindow.xaml
    /// </summary>
    public partial class TreatmentReferralWindow : Window
    {
        private readonly string _patientUsername;
        public TreatmentReferralWindow(string patientUsername)
        {
            InitializeComponent();
            _patientUsername = patientUsername;
        }

        private void referButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreatmentReferral referral = ParseReferralFromDialog();
                TreatmentReferralService.AddReferral(referral);
                Notification.ShowSuccessDialog("Successfully created referral.");
                this.Close();
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private TreatmentReferral ParseReferralFromDialog()
        {
            int numOfDays = int.Parse(numberOfDaysTextBox.Text);
            string therapy = therapyTextBox.Text;
            string additionalExaminations = additionalExaminationsTextBox.Text;
            return new TreatmentReferral(_patientUsername, numOfDays, therapy, additionalExaminations);
        }
    }
}
