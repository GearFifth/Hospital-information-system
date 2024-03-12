using System;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.DoctorView;
using static ZdravoCorp.Scheduling.Appointments.Appointment;

namespace ZdravoCorp.Scheduling.Appointments.Edit
{
    /// <summary>
    /// Interaction logic for DoctorAddAppointmentForm.xaml
    /// </summary>
    public partial class DoctorEditAppointmentForm : Window
    {
        private DoctorWindow DoctorWindow;
        private Appointment SelectedAppointment;
        public DoctorEditAppointmentForm(DoctorWindow dw, Appointment selectedAppointment)
        {
            DoctorWindow = dw;
            SelectedAppointment = selectedAppointment;
            InitializeComponent();
            FillPatientsCombobox();
            FillFormWithSelectedValues();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newAppointment = ParseAppointmentFromForm();
                AppointmentService.ValidateEditAppointment(newAppointment);
                AppointmentService.AddOrEditAppointment(newAppointment);
                DoctorWindow.UpdateAppointmentsTable();

                Notification.ShowSuccessDialog("You have successfully edited appointment!");
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private TimeSlot ParseDatesFromForm()
        {

            DateTime startDate = datePicker.SelectedDate.Value.Date;
            DateTime endDate = datePicker.SelectedDate.Value.Date;

            TimeOnly startTime = TimeOnly.Parse(startTimeTextbox.Text);
            TimeOnly endTime = TimeOnly.Parse(endTimeTextbox.Text);

            startDate = startDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);

            if (typeCombobox.Text == "Operation")
            {
                endDate = endDate.AddHours(endTime.Hour).AddMinutes(endTime.Minute);
            }
            else
            {
                endDate = startDate.AddMinutes(15);
            }

            return new TimeSlot(startDate, endDate);
        }

        private Appointment ParseAppointmentFromForm()
        {
            AppointmentType type = (AppointmentType)Enum.Parse(typeof(AppointmentType), typeCombobox.Text);
            TimeSlot timeSlot = ParseDatesFromForm();

            return new Appointment(SelectedAppointment.Id, timeSlot, Globals.LoggedUser.Username, PatientPickerCombobox.Text, type, AppointmentStatus.Active, SelectedAppointment.RoomName, false, false, false);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void typeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch (typeCombobox.Text)
            {
                case "Examination":
                {
                    endTimeTextbox.IsEnabled = true;
                    break;
                }
                case "Operation":
                {
                    endTimeTextbox.IsEnabled = false;
                    break;
                }
            }
        }

        private void FillPatientsCombobox()
        {
            foreach (var patient in PatientService.GetAllPatients())
            {
                PatientPickerCombobox.Items.Add(patient.Username);
            }
        }

        private void FillFormWithSelectedValues()
        {
            datePicker.SelectedDate = SelectedAppointment.TimeSlot.Start.Date;
            typeCombobox.Text = SelectedAppointment.Type.ToString();
            startTimeTextbox.Text = SelectedAppointment.TimeSlot.Start.ToString("HH:mm");
            endTimeTextbox.Text = SelectedAppointment.TimeSlot.End.ToString("HH:mm");
            PatientPickerCombobox.Text = SelectedAppointment.PatientUsername;

            if (SelectedAppointment.Type == AppointmentType.Operation)
            {
                endTimeTextbox.IsEnabled = true;
            }
        }

    }
}
