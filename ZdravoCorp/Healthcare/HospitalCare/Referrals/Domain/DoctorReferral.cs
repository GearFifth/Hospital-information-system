using System;
using System.Windows.Automation.Peers;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Scheduling.Appointments;
using static ZdravoCorp.Scheduling.Appointments.Appointment;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain
{
    public class DoctorReferral : Referral
    {
        public string DoctorUsername { get; set; }

        public DoctorReferral(string patientUsername, string doctorUsername) : base(patientUsername)
        {
            DoctorUsername = doctorUsername;
            
        }

        public void BookExaminationAppointment()
        {
            Doctor referredDoctor = DoctorService.GetDoctor(DoctorUsername)!;
            Patient patient = PatientService.GetPatient(PatientUsername)!;

            Appointment appointment = FindProperExaminationAppointment(referredDoctor, patient);

            AppointmentService.AddOrEditAppointment(appointment);
            Notification.ShowSuccessDialog("Examination appointment scheduled for: " + appointment.TimeSlot.Start);
        }

        private Appointment FindProperExaminationAppointment(Doctor referredDoctor, Patient patient)
        {
            DateTime appointmentTime = GetFirstPossibleAppointment();

            while (true)
            {
                TimeSlot appointmentTimeSlot = new(appointmentTime, appointmentTime.AddMinutes(15));

                if (IsAppointmentAvailable(referredDoctor, patient, appointmentTimeSlot, out var appointment)) return appointment;

                appointmentTime = appointmentTime.AddMinutes(15);
            }
        }

        private bool IsAppointmentAvailable(Doctor referredDoctor, Patient patient, TimeSlot appointmentTimeSlot, out Appointment? appointment)
        {
            var room = GetRoom(appointmentTimeSlot);
            if (referredDoctor.IsAvailable(appointmentTimeSlot) && patient.IsAvailable(appointmentTimeSlot) && room != null)
            {
                appointment = new Appointment(appointmentTimeSlot, DoctorUsername, PatientUsername, AppointmentType.Examination, AppointmentStatus.Active, room.Name);
                return true;
            }

            appointment = null;
            return false;
        }

        private static Room? GetRoom(TimeSlot appointmentTimeSlot)
        {
            try
            {
                return RoomService.GetFreeRoom(Room.RoomType.ExaminationRoom, appointmentTimeSlot);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
