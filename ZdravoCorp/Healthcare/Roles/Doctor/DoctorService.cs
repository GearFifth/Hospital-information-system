using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;

namespace ZdravoCorp.Healthcare.Roles.Doctor
{
    public static class DoctorService
    {
        public static DoctorRepository DoctorRepository = new();
        public static Doctor? GetDoctor(string username)
        {
            return DoctorRepository.Doctors.FirstOrDefault(doctor => doctor.Username == username);
        }

        public static List<Doctor> GetAllDoctors()
        {
            return DoctorRepository.GetAllDoctors();
        }

        public static ObservableCollection<Doctor> GetAllDoctorsObservableCollection()
        {
            return new ObservableCollection<Doctor>(DoctorRepository.GetAllDoctors());
        }

        public static List<Doctor> GetAllDoctorsExcept(string doctorUsername)
        {
            return DoctorRepository.GetAllDoctorsExcept(doctorUsername);
        }

        public static Doctor GetDoctorWithSpecializationExcept(DoctorSpecialization specialization, string doctorUsernameToIgnore)
        {
            return DoctorRepository.GetDoctorWithSpecializationExcept(specialization, doctorUsernameToIgnore);
        }

        public static List<Doctor> GetAllDoctorsWithSpecialization(DoctorSpecialization specialization)
        {
            return DoctorRepository.GetAllDoctorsWithSpecialization(specialization);
        }

        public static List<DoctorSpecialization> GetAllAvailableDoctorSpecializationsExcept(string doctorUsername)
        {
            return DoctorRepository.GetAllAvailableDoctorSpecializationsExcept(doctorUsername);
        }

        public static List<Doctor> GetAllSpecializedDoctors(DoctorSpecialization specialization)
        {
            return DoctorRepository.GetAllSpecializedDoctors(specialization);
        }
    }
}
