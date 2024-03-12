using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using ZdravoCorp.Scheduling.Appointments;
using static ZdravoCorp.Scheduling.Appointments.Appointment;
using static ZdravoCorp.MainUI.Users.User;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Healthcare.Roles.Doctor
{
    public enum DoctorSpecialization
    {
        Cardiology,
        Oncology,
        Neurology,
        Pediatrics,
        Gynecology,
        Surgeon,
        Physician,
        None    //Predstavlja doktora opste prakse
    }
    public class Doctor : Serializable
    {

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public float Rating { get; set; }

        public DoctorSpecialization Specialization;

        public Doctor() { }
        public Doctor(string username, string firstName, string lastName)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Specialization = DoctorSpecialization.None;
        }

        public bool IsAvailable(TimeSlot timeSlot)
        {
            foreach (Appointment appointment in AppointmentService.GetAllAppointments())
            {
                if (appointment.DoctorUsername == Username &&
                    appointment.Status == AppointmentStatus.Active &&
                    appointment.TimeSlot.OverlapsWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsEditAvailable(Appointment editedAppointment)
        {
            foreach (Appointment appointment in AppointmentService.GetAllAppointments())
            {
                if (appointment.Id != editedAppointment.Id &&
                    appointment.DoctorUsername == Username &&
                    appointment.Status == AppointmentStatus.Active &&
                    appointment.TimeSlot.OverlapsWith(editedAppointment.TimeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public string[] ToTable()
        {
            string[] tableValues =
            {
                Username,
                FirstName,
                LastName,
                Specialization.ToString(),
                Rating.ToString(),
            };
            return tableValues;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Username,
                FirstName,
                LastName,
                Specialization.ToString(),
                Rating.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Username = values[0];
            FirstName = values[1];
            LastName = values[2];
            Specialization = ParseDoctorSpecialization(values[3]);
            Rating = float.Parse(values[4]);
        }
        public DoctorSpecialization ParseDoctorSpecialization(string specializationString)
        {
            return Enum.Parse<DoctorSpecialization>(specializationString);
        }
    }
}
