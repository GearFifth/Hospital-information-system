using System;
using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Service;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Presentation
{
    /// <summary>
    /// Interaction logic for DoctorReferralWindow.xaml
    /// </summary>
    public partial class DoctorReferralWindow : Window
    {
        private readonly string _patientUsername;
        public DoctorReferralWindow(string patientUsername)
        {
            InitializeComponent();
            _patientUsername = patientUsername;
        }

        private void referButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Doctor chosenDoctor = GetSelectedDoctor();
                DoctorReferralService.AddDoctorReferral(new DoctorReferral(_patientUsername, chosenDoctor.Username));
                Notification.ShowSuccessDialog("Successfully created referral.");
                this.Close();
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDoctorComboBox();
            FillSpecializationComboBox();
        }

        private Doctor GetSelectedDoctor()
        {
            if (specializationRadioButton.IsChecked == true)
            {
                DoctorSpecialization specialization = (DoctorSpecialization)Enum.Parse(typeof(DoctorSpecialization), specializationComboBox.Text);
                return DoctorService.GetDoctorWithSpecializationExcept(specialization, Globals.LoggedUser.Username);
            }
            else if (doctorRadioButton.IsChecked == true)
            {
                return DoctorService.GetDoctor(doctorComboBox.Text);
            }

            throw new ArgumentException("Doctor has not been chosen.");
        }

        private void FillDoctorComboBox()
        {
            foreach (Doctor doctor in DoctorService.GetAllDoctorsExcept(Globals.LoggedUser.Username))
            {
                doctorComboBox.Items.Add(doctor.Username);
            }
        }

        private void FillSpecializationComboBox()
        {
            foreach (var specialization in DoctorService.GetAllAvailableDoctorSpecializationsExcept(Globals.LoggedUser.Username))
            {
                specializationComboBox.Items.Add(specialization);
            }
        }

        private void specializationRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            specializationComboBox.IsEnabled = true;
        }

        private void specializationRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            specializationComboBox.IsEnabled = false;
        }

        private void doctorRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            doctorComboBox.IsEnabled = true;
        }

        private void doctorRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            doctorComboBox.IsEnabled = false;
        }
    }
}
