using System;
using System.Windows;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.PatientView;
using static ZdravoCorp.Scheduling.Appointments.Appointment;

namespace ZdravoCorp.Scheduling.Appointments.Add
{
    /// <summary>
    /// Interaction logic for AddPatientAppointmentDialog.xaml
    /// </summary>
    public partial class AddPatientAppointmentDialog : Window
    {
        private PatientWindow patientWindow;
        private string doctorUsername;
        public AddPatientAppointmentDialog(PatientWindow patientWindow, string doctorUsername)
        {
            InitializeComponent();
            this.patientWindow = patientWindow;
            this.doctorUsername = doctorUsername;
            InitializeDefaultValues();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = ParseAppointmentFromDialog();
                AppointmentService.ValidateAddAppointment(appointment);
                AppointmentService.AddOrEditAppointment(appointment);

                patientWindow.UpdateAppointmentsTable();

                if (AppointmentService.CountPatientAddedAppointments(Globals.LoggedUser.Username) >= 8)
                {
                    PatientService.BlockPatient(Globals.LoggedUser.Username);
                    patientWindow.LogOutBlockedPatient();
                    patientWindow.Close();
                }
                else
                {
                    Notification.ShowSuccessDialog("You have successfully created an appointment!");
                    this.Close();
                }
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitializeDefaultValues()
        {
            foreach (Doctor doctor in DoctorService.GetAllDoctors())
            {
                doctorPickerComboBox.Items.Add(doctor.Username);
            }

            datePicker.SelectedDate = DateTime.Now;
            timePickerTextBox.Text = "08:00";

            if (doctorUsername != null)
            {
                doctorPickerComboBox.SelectedValue = doctorUsername;
            }
        }

        private TimeSlot ParseDatesFromDialog()
        {

            DateTime startDate = datePicker.SelectedDate.Value.Date;
            DateTime endDate = datePicker.SelectedDate.Value.Date;

            try
            {
                TimeOnly startTime = TimeOnly.Parse(timePickerTextBox.Text);

                startDate = startDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);
                endDate = startDate.AddMinutes(15);
            }
            catch
            {
                return null;
            }
           
            return new TimeSlot(startDate, endDate);
        }

        private Appointment ParseAppointmentFromDialog()
        {
            TimeSlot timeSlot = ParseDatesFromDialog();

            return new Appointment(timeSlot, doctorPickerComboBox.Text, Globals.LoggedUser.Username, AppointmentType.Examination, AppointmentStatus.Active, "",  true, false, false);
        }

        private void AddAppointmentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            patientWindow.IsEnabled = true;
        }
    }
}
