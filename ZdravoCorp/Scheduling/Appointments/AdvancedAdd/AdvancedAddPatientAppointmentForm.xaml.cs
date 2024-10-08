﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.PatientView;
using static ZdravoCorp.Scheduling.Appointments.Appointment;

namespace ZdravoCorp.Scheduling.Appointments.AdvancedAdd
{
    /// <summary>
    /// Interaction logic for AdvancedAddPatientAppointmentForm.xaml
    /// </summary>
    public partial class AdvancedAddPatientAppointmentForm : Window
    {
        private PatientWindow patientWindow;
        public AdvancedAddPatientAppointmentForm(PatientWindow patientWindow)
        {
            this.patientWindow = patientWindow;
            InitializeComponent();
            InitializeDefaultValues();
        }

        private void InitializeDefaultValues()
        {
            foreach (Doctor doctor in DoctorService.GetAllDoctors())
            {
                doctorPickerComboBox.Items.Add(doctor.Username);
            }
            datePicker.SelectedDate = DateTime.Now;
            timeFromTextBox.Text = "08:00";
            timeToTextBox.Text = "20:00";
            timeRadioButton.IsChecked = true;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime lastDate = datePicker.SelectedDate.Value.Date;
                TimeSlot timeslot = ParseTimesFromDialog();
                Doctor doctor = DoctorService.GetDoctor(doctorPickerComboBox.Text);
                Patient patient = PatientService.GetPatient(Globals.LoggedUser.Username);

                List<Appointment> availableAppointments = SmartSchedule.GetAvailableAppointmentsInDateRange(doctor, patient, lastDate, timeslot);


                while (availableAppointments.Count == 0 && doctorRadioButton.IsChecked == true)
                {
                   availableAppointments = SmartSchedule.GetAvailableAppointmentsPriorityDoctor(doctor, patient, lastDate, timeslot);
                }

                if (availableAppointments.Count == 0 && timeRadioButton.IsChecked == true)
                {
                    availableAppointments = SmartSchedule.GetAvailableAppointmentsPriorityTime(doctor, patient, lastDate, timeslot);
                }

                if (availableAppointments.Count == 0)
                {
                    availableAppointments = SmartSchedule.GetThreeClosestAppointments(patient,lastDate, timeslot);
                }

                AddAppointmentsToDataGrid(availableAppointments);
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }



        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = GetSelectedAppointment();
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

                    Notification.ShowSuccessDialog("You have successfully edited an appointment!");

                    this.Close();
                }
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }
        private TimeSlot ParseTimesFromDialog()
        {
            DateTime startTime = DateTime.Today;
            DateTime endTime = DateTime.Today;
            try
            {
                TimeOnly timeFrom = TimeOnly.Parse(timeFromTextBox.Text);
                TimeOnly timeTo = TimeOnly.Parse(timeToTextBox.Text);

                startTime = startTime.AddHours(timeFrom.Hour).AddMinutes(timeFrom.Minute);
                endTime = endTime.AddHours(timeTo.Hour).AddMinutes(timeTo.Minute);
            } catch
            {
                throw new Exception("Invalid time input");
            }
            return new TimeSlot(startTime, endTime);
        }

        public void AddAppointmentsToDataGrid(List<Appointment> appointments)
        {
            DataTable dataTable = InitAppointmentTableColumns();

            foreach (Appointment app in appointments)
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

        private Appointment GetSelectedAppointment()
        {
            var selectedRow = appointmentsDataGrid.SelectedItem as DataRowView;
            if (selectedRow == null)
            {
                throw new InvalidOperationException("Please select a row.");
            }

            int id = int.Parse(selectedRow.Row.ItemArray[0].ToString());
            DateTime startTime = DateTime.Parse(selectedRow.Row.ItemArray[1].ToString());
            DateTime endTime = DateTime.Parse(selectedRow.Row.ItemArray[2].ToString());
            TimeSlot timeslot = new TimeSlot(startTime, endTime);

            string doctorUsername = selectedRow.Row.ItemArray[3].ToString();
            string patientUsername = selectedRow.Row.ItemArray[4].ToString();

            AppointmentType type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedRow.Row.ItemArray[5].ToString());
            AppointmentStatus status = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), selectedRow.Row.ItemArray[6].ToString());
            string roomName = selectedRow.Row.ItemArray[7].ToString();

            return new Appointment(id, timeslot, doctorUsername, patientUsername, type, status, roomName, true, false, false);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
