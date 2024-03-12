using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;

namespace ZdravoCorp.Scheduling.Appointments.Add
{
    /// <summary>
    /// Interaction logic for ChooseDoctorSpecializationDialog.xaml
    /// </summary>
    public partial class ChooseDoctorSpecializationDialog : Window
    {
        private readonly Patient _patient;
        private readonly DrugPrescription _drugPrescription;
        public ChooseDoctorSpecializationDialog(Patient patient, DrugPrescription drugPrescription)
        {
            _patient = patient;
            _drugPrescription = drugPrescription;
            InitializeComponent();
        }

        private void ChooseSpecializationBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedDoctorSpecialization = (DoctorSpecialization)specializationComboBox.SelectionBoxItem;
            List<Doctor> specializedDoctors = DoctorService.GetAllDoctorsWithSpecialization(selectedDoctorSpecialization);
            DateTime appointmentStartTime = Appointment.GetFirstPossibleAppointment();
            TimeSlot appointmentTimeSlot = new(appointmentStartTime, appointmentStartTime.AddMinutes(15));

            if (!AreConditionsForBookingMet(specializedDoctors)) return;

            SearchForFirstAvailableExamination(appointmentTimeSlot, specializedDoctors);
        }

        private void SearchForFirstAvailableExamination(TimeSlot appointmentTimeSlot, List<Doctor> specializedDoctors)
        {
            while (true)
            {
                if (TryToBookExamination(appointmentTimeSlot, specializedDoctors)) return;

                appointmentTimeSlot.Start = appointmentTimeSlot.Start.AddMinutes(15);
                appointmentTimeSlot.End = appointmentTimeSlot.End.AddMinutes(15);
            }
        }

        private bool AreConditionsForBookingMet(List<Doctor> specializedDoctors)
        {
            if (!CheckIfSpecializedDoctorsExist(specializedDoctors)) return false;

            if (!_drugPrescription.IsSentToExamination) return true;

            Notification.ShowErrorDialog("Examination after drug prescription was already scheduled for this patient.. ");
            return false;

        }

        private static bool CheckIfSpecializedDoctorsExist(List<Doctor> specializedDoctors)
        {
            if (specializedDoctors.Count != 0) return true;

            Notification.ShowErrorDialog("We apologize but currently we do not have doctors of this specialization.. ");
            return false;

        }

        private bool TryToBookExamination(TimeSlot appointmentTimeSlot, List<Doctor> specializedDoctors)
        {
            try
            {
                Room room = RoomService.GetFreeRoom(Room.RoomType.ExaminationRoom, appointmentTimeSlot);
                if (!IsAnySpecializedDoctorAvailable(appointmentTimeSlot, specializedDoctors, room)) return false;
                
                Notification.ShowSuccessDialog("Successfully scheduled appointment on: " + appointmentTimeSlot.Start + " in examination room: " + room.Name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsAnySpecializedDoctorAvailable(TimeSlot appointmentTimeSlot, List<Doctor> specializedDoctors, Room room)
        {
            foreach (var doctor in specializedDoctors)
            {
                if (!doctor.IsAvailable(appointmentTimeSlot) || !_patient.IsAvailable(appointmentTimeSlot)) continue;

                Appointment appointment = new Appointment(appointmentTimeSlot, doctor.Username, _patient.Username, Appointment.AppointmentType.Examination, Appointment.AppointmentStatus.Active, room.Name);
                AppointmentService.AddOrEditAppointment(appointment);

                _drugPrescription.IsSentToExamination = true;
                DrugPrescriptionService.SaveRepository();

                return true;
            }

            return false;
        }
    }
}
