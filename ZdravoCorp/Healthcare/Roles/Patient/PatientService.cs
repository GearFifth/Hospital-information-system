using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using static ZdravoCorp.Healthcare.Roles.Patient.Patient;

namespace ZdravoCorp.Healthcare.Roles.Patient
{
    public static class PatientService
    {
        public static PatientRepository PatientRepository = new();
        public static void SaveRepository()
        {
            PatientRepository.SaveRepository();
        }

        public static Patient? GetPatient(string username)
        {
            try
            {
                return PatientRepository.GetPatient(username);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Patient> GetAllPatients()
        {
            return PatientRepository.Patients;
        }

        public static List<Patient> GetAllActivePatients()
        {
            return PatientRepository.GetAllActivePatients();
        }

        public static void BlockPatient(string patientUsername)
        {
            PatientRepository.BlockPatient(patientUsername);
        }

        public static bool IsPatientBlocked(string patientUsername)
        {
            return PatientRepository.IsPatientBlocked(patientUsername);
        }

        public static void EditPatient(string oldUsername, string newUsername, string firstName, string lastName,
            bool isChecked)
        {
            PatientRepository.EditPatient(oldUsername, newUsername, firstName, lastName, isChecked);
        }
        public static void DeletePatient(string username)
        {
            PatientRepository.DeletePatient(username);
        }

        public static void AddPatient(Patient patient)
        {
            PatientRepository.Patients.Add(patient);
            PatientRepository.SaveRepository();
        }
    }


}
