using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using static ZdravoCorp.Scheduling.Appointments.Appointment;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.Notices;
using ZdravoCorp.Scheduling.Appointments.AdvancedAdd;

namespace ZdravoCorp.Scheduling.Appointments
{
    public static class AppointmentService
    {
        public static AppointmentRepository AppointmentRepository = new();
        public static Appointment GetAppointment(int id)
        {
            return AppointmentRepository.GetAppointment(id);
        }

        public static void CancelAppointment(int id)
        {
            AppointmentRepository.CancelAppointment(id);
        }

        public static void PatientCancelAppointment(int id)
        {
            AppointmentRepository.PatientCancelAppointment(id);
        }

        public static void FinishAppointment(Appointment appointment)
        {
            AppointmentRepository.FinishAppointment(appointment);
        }

        public static List<Appointment> GetAppointmentsForNextThreeDays(DateTime date)
        {
            return AppointmentRepository.GetAppointmentsForNextThreeDays(date);
        }

        public static List<Appointment> GetAppointmentsInRange(TimeSlot timeSlot)
        {
            return AppointmentRepository.Appointments.Values.Where(appointment => appointment.IsInRange(timeSlot.Start, timeSlot.End)).ToList();
        }

        public static List<Appointment> GetAllAppointments()
        {
            return AppointmentRepository.GetAllAppointments();
        }

        public static List<Appointment> GetAllAppointmentsForPatient(string username)
        {
            return AppointmentRepository.GetAllAppointmentsForPatient(username);
        }

        public static bool HasPatientBeenToDoctor(string patientUsername, string doctorUsername)
        {
            return AppointmentRepository.HasPatientBeenToDoctor(patientUsername, doctorUsername);
        }
        public static void AddOrEditAppointment(Appointment appointment)
        {
            AppointmentRepository.AddOrEditAppointment(appointment);
        }

        public static int CountPatientCanceledAppointments(string patientUsername)
        {
            return AppointmentRepository.CountPatientCanceledAppointments(patientUsername);
        }

        public static int CountPatientAddedAppointments(string patientUsername)
        {
            return AppointmentRepository.CountPatientAddedAppointments(patientUsername);
        }

        public static int CountPatientEditedAppointments(string patientUsername)
        {
            return AppointmentRepository.CountPatientEditedAppointments(patientUsername);
        }

        public static List<Appointment> GetActiveOverlappingAppointments(TimeSlot timeslot)
        {
            return AppointmentRepository.Appointments.Values.Where(appointment => appointment.TimeSlot.OverlapsWith(timeslot)).ToList();
        }

        public static List<Appointment> GetAppointmentsInNextFifteenMinutes()
        {
            return AppointmentRepository.GetAppointmentsInNextFifteenMinutes();
        }

        public static List<Appointment> GetAppointmentsInNextTwoHours()
        {
            return AppointmentRepository.GetAppointmentsInNextTwoHours();
        }

        public static void CancelAppointmentsInRange(TimeSlot timeslot, string doctorUsername)
        {
            var cancelAppointments=AppointmentRepository.CancelAppointmentsInRange(timeslot,doctorUsername);
            foreach (var appointment in cancelAppointments)
            {
                NoticeService.Add(new Notice(DateTime.Now, "Your appointment has been cancelled!",appointment.PatientUsername));
            }
        }

        public static void AppointCheckUp(string doctorUsername, string patientUsername, int days = 10)
        {
            DateTime startDate = DateTime.Today.AddDays(days).AddHours(8);
            DateTime endDate = DateTime.Today.AddDays(days + 1);
            Appointment app = SmartSchedule.GetAvailableAppointmentsInDateRange(DoctorService.GetDoctor(doctorUsername),
                PatientService.GetPatient(patientUsername), endDate, new TimeSlot(startDate, endDate))[0];
            AddOrEditAppointment(app);
        }
        public static void ValidateAddAppointment(Appointment appointment)
        {
            if (!appointment.TimeSlot.IsInFuture())
            {
                throw new ArgumentException("Time must be in future.");
            }
            else if (!PatientService.GetPatient(appointment.PatientUsername).IsAvailable(appointment.TimeSlot))
            {
                throw new ArgumentException("Patient is not available at given time.");
            }
            else if (!DoctorService.GetDoctor(appointment.DoctorUsername).IsAvailable(appointment.TimeSlot))
            {
                throw new ArgumentException("Doctor is not available at given time.");
            }
        }
        public static void ValidateEditAppointment(Appointment appointment)
        {
            if (!appointment.TimeSlot.IsInFuture())
            {
                throw new ArgumentException("Time must be in future.");
            }
            else if (!PatientService.GetPatient(appointment.PatientUsername).IsEditAvailable(appointment))
            {
                throw new ArgumentException("Patient is not available at given time.");
            }
            else if (!DoctorService.GetDoctor(appointment.DoctorUsername).IsEditAvailable(appointment))
            {
                throw new ArgumentException("Doctor is not available at given time.");
            }
        }
        public static void ValidateBeforeEditOrCancel(Appointment appointment)
        {
            if (appointment.Status != AppointmentStatus.Active)
            {
                throw new InvalidOperationException("You can't edit non-active appointments.");
            }
            if (DateTime.Now.AddDays(1) > appointment.TimeSlot.Start)
            {
                throw new ArgumentException("Minimum 1 day difference is required in order to edit or cancel an appointment!");
            }
        }
        public static void ValidateAppointmentBeforeExamination(Appointment appointment)
        {
            if (!appointment.IsActive())
            {
                throw new InvalidOperationException("This appointment is not active.");
            }
            else if (DateTime.Now < appointment.TimeSlot.Start.AddMinutes(-15))
            {
                throw new InvalidOperationException("Examination can only be started 15 minutes prior.");
            }
            else if (appointment.PatientUsername == "")
            {
                throw new InvalidOperationException("Patient has no account.");
            }
        }
    }
}
