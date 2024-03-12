using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.MainUI;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Scheduling.Appointments
{
    public class AppointmentRepository
    {
        public const string AppointmentsFilePath = "..\\..\\..\\Scheduling\\Appointments\\appointments.csv";
        public Dictionary<int, Appointment> Appointments = new();
        public Serializer<Appointment> AppointmentSerializer = new();

        public AppointmentRepository()
        {
            List<Appointment> parsedAppointments = AppointmentSerializer.fromCSV(AppointmentsFilePath);
            Appointments = parsedAppointments.ToDictionary(app => app.Id, app => app);
            UpdateAppointmentsStatus();
        }

        public List<Appointment> GetAllAppointments()
        {
            return Appointments.Values.ToList();
        }
        public  Appointment GetAppointment(int id)
        {
            return Appointments[id];
        }

        public  void CancelAppointment(int id)
        {
            GetAppointment(id).Cancel();
            Save();
        }

        public  void PatientCancelAppointment(int id)
        {
            GetAppointment(id).PatientCancel();
            Save();
        }

        public  void FinishAppointment(Appointment appointment)
        {
            appointment.Finish();
            Save();
        }

        public  List<Appointment> GetAllAppointmentsForPatient(string username)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in Appointments.Values)
            {
                if (appointment.PatientUsername == username)
                {
                    appointments.Add(appointment);
                }
            }

            return appointments;
        }

        public  bool HasPatientBeenToDoctor(string patientUsername, string doctorUsername)
        {
            foreach (Appointment appointment in Appointments.Values)
            {
                if (appointment.IsFinished() &&
                    appointment.DoctorUsername == doctorUsername &&
                    appointment.PatientUsername == patientUsername)
                {
                    return true;
                }
            }
            return false;
        }
        public  void AddOrEditAppointment(Appointment appointment)
        {
            appointment.AssignRoom();
            if (appointment.Id == -1)
            {
                AssignId(appointment);
            }
            Appointments[appointment.Id] = appointment;
            Save();
        }

        public  List<Appointment> GetAppointmentsForNextThreeDays(DateTime date)
        {
            List<Appointment> appointments = new();
            foreach (Appointment appointment in Appointments.Values)
            {
                if (appointment.IsInRange(date, date.AddDays(3)) && appointment.DoctorUsername == Globals.LoggedUser.Username)
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        public  List<Appointment> GetAppointmentsInNextFifteenMinutes()
        {
            List<Appointment> appointments = new();
            foreach (Appointment appointment in Appointments.Values)
            {
                if (!appointment.IsInNextFifteenMinutes()) continue;
                appointments.Add(appointment);
            }
            return appointments;
        }

        public  List<Appointment> GetAppointmentsInNextTwoHours()
        {
            List<Appointment> appointments = new();
            foreach (Appointment appointment in Appointments.Values)
            {
                if (!appointment.IsInNextTwoHours()) continue;
                appointments.Add(appointment);
            }
            return appointments;
        }

        public  int CountPatientCanceledAppointments(string patientUsername)
        {
            int appointments = 0;
            foreach (Appointment appointment in Appointments.Values)
            {
                if (appointment.PatientUsername == patientUsername && appointment.HasPatientCanceled == true &&
                    appointment.TimeSlot.Start.AddDays(30) > DateTime.Now)
                {
                    appointments++;
                }
            }
            return appointments;
        }

        public  int CountPatientAddedAppointments(string patientUsername)
        {
            int appointments = 0;
            foreach (Appointment appointment in Appointments.Values)
            {
                if (appointment.PatientUsername == patientUsername && appointment.HasPatientAppointed == true &&
                    appointment.TimeSlot.Start.AddDays(30) > DateTime.Now)
                {
                    appointments++;
                }
            }
            return appointments;
        }

        public  int CountPatientEditedAppointments(string patientUsername)
        {
            int appointments = 0;
            foreach (Appointment appointment in Appointments.Values)
            {
                if (appointment.PatientUsername == patientUsername && appointment.HasPatientEdited == true &&
                    appointment.TimeSlot.Start.AddDays(30) > DateTime.Now)
                {
                    appointments++;
                }
            }
            return appointments;
        }

        public  void Save()
        {
            AppointmentSerializer.toCSV(AppointmentsFilePath, Appointments.Values.ToList());
        }


        private void UpdateAppointmentsStatus()
        {
            foreach (Appointment appointment in Appointments.Values)
            {
                if (appointment.TimeSlot.End < DateTime.Now && !appointment.IsCanceled())
                {
                    appointment.Status = Appointment.AppointmentStatus.Finished;
                }
            }
            Save();
        }

        public List<Appointment> CancelAppointmentsInRange(TimeSlot timeSlot, string doctorUsername)
        {
            List<Appointment> appointments = Appointments.Values.ToList().FindAll(appointment => appointment.DoctorUsername == doctorUsername && appointment.IsInRange(timeSlot.Start,timeSlot.End));
            foreach (var appointment in appointments)
            {
                appointment.Cancel();
            }
            Save();
            return appointments;
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        private void AssignId(Appointment appointment)
        {
            do
            {
                appointment.Id = GenerateId();
            } while (Appointments.ContainsKey(appointment.Id));
        }
    }
}
