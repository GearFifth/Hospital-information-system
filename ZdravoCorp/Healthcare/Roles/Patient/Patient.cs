using System;
using System.Collections.Generic;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Utils.Serializer;
using static ZdravoCorp.Scheduling.Appointments.Appointment;

namespace ZdravoCorp.Healthcare.Roles.Patient
{
    public class Patient : Serializable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public enum BlockStatus
        {
            Active,
            Blocked
        }

        public BlockStatus Status { get; set; }

        public TimeSpan NotificationLeadTime { get; set; }

        public Patient() { }
        public Patient(string firstName, string lastName, string username)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Status = BlockStatus.Active;
            NotificationLeadTime = TimeSpan.FromMinutes(30);
        }

        public Patient(string firstName, string lastName, string username, bool isChecked)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Status = isChecked ? BlockStatus.Blocked : BlockStatus.Active;
            NotificationLeadTime = TimeSpan.FromMinutes(30);
        }

        public bool IsAvailable(TimeSlot timeSlot)
        {
            foreach (Appointment appointment in AppointmentService.GetAllAppointments())
            {
                if (appointment.PatientUsername == Username &&
                    appointment.Status == AppointmentStatus.Active &&
                    appointment.TimeSlot.OverlapsWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public Appointment? GetPatientsAppointmentThatOverlapsWith(TimeSlot timeSlot)
        {
            foreach (Appointment appointment in AppointmentService.GetAllAppointments())
            {
                if (appointment.PatientUsername == Username &&
                    appointment.Status == AppointmentStatus.Active &&
                    appointment.TimeSlot.OverlapsWith(timeSlot))
                {
                    return appointment;
                }
            }
            return null;
        }

        public bool IsEditAvailable(Appointment editedAppointment)
        {
            foreach (Appointment appointment in AppointmentService.GetAllAppointments())
            {
                if (appointment.Id != editedAppointment.Id &&
                    appointment.PatientUsername == Username &&
                    appointment.Status == AppointmentStatus.Active &&
                    appointment.TimeSlot.OverlapsWith(editedAppointment.TimeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public void BlockAccess()
        {
            Status = BlockStatus.Blocked;
        }

        public void SetNotificationLeadTime(TimeSpan time)
        {
            NotificationLeadTime = time;
            PatientService.SaveRepository();
        }

        public string[] ToTable()
        {
            string[] tableValues =
            {
                Username,
                FirstName,
                LastName
            };
            return tableValues;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                FirstName,
                LastName,
                Username,
                Status.ToString(),
                NotificationLeadTime.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            FirstName = values[0];
            LastName = values[1];
            Username = values[2];
            Status = (BlockStatus)Enum.Parse(typeof(BlockStatus), values[3]);
            NotificationLeadTime = TimeSpan.Parse(values[4]);
        }
    }
}
