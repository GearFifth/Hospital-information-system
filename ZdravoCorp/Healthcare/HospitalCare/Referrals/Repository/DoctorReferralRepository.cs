using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.Roles.Patient;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository
{
    public class DoctorReferralRepository : IDoctorReferralRepository
    {
        public const string DoctorReferralFilePath = "..\\..\\..\\Healthcare\\HospitalCare\\Referrals\\Data\\doctor_referrals.json";
        public List<DoctorReferral> Referrals = new();

        public DoctorReferralRepository()
        {
            if (!File.Exists(DoctorReferralFilePath)) return;

            string json = File.ReadAllText(DoctorReferralFilePath);
            Referrals = JsonConvert.DeserializeObject<List<DoctorReferral>>(json);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(Referrals, Formatting.Indented);
            File.WriteAllText(DoctorReferralFilePath, json);
        }

        public void Add(DoctorReferral doctorReferral)
        {
            AssignId(doctorReferral);
            Referrals.Add(doctorReferral);
            Save();
        }

        public bool Contains(int id)
        {
            return Referrals.Any(doctorReferral => doctorReferral.Id == id);
        }

        public List<DoctorReferral> GetUnusedPatientsReferrals(string patientUsername)
        {
            return Referrals.Where(doctorReferral => doctorReferral.PatientUsername.Equals(patientUsername)).ToList();
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        private void AssignId(DoctorReferral doctorReferral)
        {
            do
            {
                doctorReferral.Id = GenerateId();
            } while (Contains(doctorReferral.Id));
        }
    }
}
