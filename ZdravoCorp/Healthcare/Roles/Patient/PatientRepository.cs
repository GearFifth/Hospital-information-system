using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Healthcare.Roles.Patient
{
    public class PatientRepository
    {
        public const string PatientsFilePath = "..\\..\\..\\Healthcare\\Roles\\Patient\\patients.csv";
         public  List<Patient> Patients = new();
        public  Serializer<Patient> PatientSerializer = new();

        public PatientRepository()
        {
            Patients = PatientSerializer.fromCSV(PatientsFilePath);
        }

        public  void SaveRepository()
        {
            PatientSerializer.toCSV(PatientsFilePath, Patients);
        }

        public  List<Patient> GetAllActivePatients()
        {
            return Patients.Where(patient => patient.Status == Patient.BlockStatus.Active).ToList();
        }

        public  Patient GetPatient(string username)
        {
            foreach (var patient in Patients)
            {
                if (patient.Username == username) return patient;
            }

            throw new ArgumentException("Patient has not been found.");
        }

        public  void EditPatient(string oldUsername, string newUsername, string firstName, string lastName,
            bool isChecked)
        {
            var patient = GetPatient(oldUsername);
            if (patient == null) return;

            patient!.Username = newUsername;
            patient.FirstName = firstName;
            patient.LastName = lastName;
            patient.Status = isChecked ? Patient.BlockStatus.Blocked : Patient.BlockStatus.Active;

            SaveRepository();
        }

        public  void DeletePatient(string username)
        {
            Patient patient = GetPatient(username)!;
            Patients.Remove(patient);
            SaveRepository();
        }

        public  void BlockPatient(string patientUsername)
        {
            Patient patient = GetPatient(patientUsername);
            patient.Status = Patient.BlockStatus.Blocked;
            SaveRepository();
        }
        public  bool IsPatientBlocked(string patientUsername)
        {
            Patient patient = GetPatient(patientUsername);
            return patient.Status == Patient.BlockStatus.Blocked;
        }
    }
}
