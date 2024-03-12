using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ZdravoCorp.Scheduling.Appointments.Appointment;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;

namespace ZdravoCorp.Scheduling.Appointments.AdvancedAdd
{
    public class SmartSchedule
    {
        public static List<Appointment> GetAvailableAppointmentsInDateRange(Doctor doctor, Patient patient, DateTime endDate, TimeSlot timeslot)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            TimeSlot iterativeTimeSlot = new();
            iterativeTimeSlot.Start = timeslot.Start;
            iterativeTimeSlot.End = timeslot.End;

            while (iterativeTimeSlot.Start.Date <= endDate)
            {
                List<Appointment> appointmentsForDay = GetAvailableAppointmentsInTimeRange(doctor, patient, iterativeTimeSlot);
                iterativeTimeSlot.Start = iterativeTimeSlot.Start.AddDays(1);
                iterativeTimeSlot.End = iterativeTimeSlot.End.AddDays(1);

                availableAppointments.AddRange(appointmentsForDay);
            }

            return availableAppointments;
        }

        public static List<Appointment> GetAvailableAppointmentsInTimeRange(Doctor doctor, Patient patient, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime startTime = timeRange.Start;

            while (startTime < timeRange.End)
            {
                try
                {
                    string roomName = RoomService.GetFreeRoom(Room.RoomType.ExaminationRoom, timeRange).Name;

                    TimeSlot timeSlot = new();
                    timeSlot.Start = startTime;
                    timeSlot.End = startTime.AddMinutes(15);

                    if (doctor.IsAvailable(timeSlot) && patient.IsAvailable(timeSlot) && timeSlot.Start > DateTime.Now)
                    {
                        Appointment appointment = new Appointment(timeSlot, doctor.Username, patient.Username, AppointmentType.Examination, AppointmentStatus.Active, roomName, true, false, false);
                        availableAppointments.Add(appointment);
                    }
                }
                catch
                {
                }
                startTime = startTime.AddMinutes(15);
            }

            return availableAppointments;
        }

        public static List<Appointment> GetAvailableAppointmentsPriorityTime(Doctor doctor, Patient patient, DateTime lastDate, TimeSlot timeslot)
        {
            List<Appointment> availableAppointments = new();
            foreach (Doctor doc in DoctorService.GetAllDoctors())
            {
                if (doc == doctor) //skip already occupied doctor
                {
                    continue;
                }
                availableAppointments = GetAvailableAppointmentsInDateRange(doc, patient, lastDate, timeslot);
                if (availableAppointments.Count != 0)
                {
                    break;
                }
            }
            return availableAppointments;
        }

        public static List<Appointment> GetAvailableAppointmentsPriorityDoctor(Doctor doctor, Patient patient, DateTime lastDate, TimeSlot timeslot)
        {
            List<Appointment> availableAppointments = new();

            if (lastDate < timeslot.Start.Date)
            {
                lastDate = timeslot.Start.Date;
            }

            for (int i = 0; i < 7; i++)
            {
                availableAppointments = GetAvailableAppointmentsInDateRange(doctor,patient, lastDate, timeslot);

                if (availableAppointments.Count > 0)
                {
                    return availableAppointments;
                }

                lastDate = lastDate.AddDays(1);
                timeslot.Start = timeslot.Start.AddDays(1);
                timeslot.End = timeslot.End.AddDays(1);
            }
            return availableAppointments;

        }

        public static List<Appointment> GetThreeClosestAppointments(Patient patient, DateTime lastDate, TimeSlot timeslot)
        {
            List<Appointment> availableAppointments = new();
            if (lastDate < timeslot.Start.Date)
            {
                lastDate = timeslot.Start.Date;
            }

            while (availableAppointments.Count <= 3)
            {
                foreach (Doctor doctor in DoctorService.GetAllDoctors())
                {
                    availableAppointments.AddRange(GetAvailableAppointmentsInDateRange(doctor, patient,lastDate, timeslot));
                }
                lastDate = lastDate.AddDays(1);
                timeslot.Start = timeslot.Start.AddDays(1);
                timeslot.End = timeslot.End.AddDays(1);
            }
            return availableAppointments.GetRange(0, 3);
        }
    }
}
