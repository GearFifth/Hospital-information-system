using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Healthcare.Roles.Doctor
{
    public class DoctorRepository
    {
        public const string DoctorsFilePath = "..\\..\\..\\HealthCare\\Roles\\Doctor\\doctors.csv";
        public  List<Doctor> Doctors = new();
        public  Serializer<Doctor> DoctorSerializer = new();

        public DoctorRepository()
        {
            Doctors = DoctorSerializer.fromCSV(DoctorsFilePath);
        }

        public  List<Doctor> GetAllDoctors()
        {
            return Doctors;
        }

        public  List<Doctor> GetAllDoctorsExcept(string doctorUsername)
        {
            return Doctors.Where(doctor => doctor.Username != doctorUsername).ToList();
        }

        public  List<Doctor> GetAllSpecializedDoctors(DoctorSpecialization specialization)
        {
            return Doctors.Where(doctor => doctor.Specialization == specialization).ToList();
        }


        public  List<Doctor> GetAllDoctorsWithSpecialization(DoctorSpecialization specialization)
        {
            return Doctors.Where(doctor => doctor.Specialization == specialization).ToList();
        }

        public  List<DoctorSpecialization> GetAllAvailableDoctorSpecializationsExcept(string doctorUsername)
        {
            Dictionary<DoctorSpecialization, int> availableSpecializations = new Dictionary<DoctorSpecialization, int>();
            foreach (Doctor doctor in Doctors)
            {
                if (doctor.Username != doctorUsername)
                {
                    availableSpecializations[doctor.Specialization] = 1;
                }
            }
            return availableSpecializations.Keys.ToList();
        }

        public  Doctor GetDoctorWithSpecializationExcept(DoctorSpecialization specialization, string doctorUsernameToIgnore)
        {
            return Doctors.FirstOrDefault(doctor => doctor.Specialization == specialization && doctor.Username != doctorUsernameToIgnore);
        }

        public  void Save()
        {
            DoctorSerializer.toCSV(DoctorsFilePath, Doctors);
        }
    }
}
