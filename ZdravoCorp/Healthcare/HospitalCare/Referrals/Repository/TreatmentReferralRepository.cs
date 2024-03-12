using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository
{
    public class TreatmentReferralRepository : ITreatmentReferralRepository
    {
        public const string DoctorReferralFilePath = "..\\..\\..\\Healthcare\\HospitalCare\\Referrals\\Data\\treatment_referrals.json";
        public List<TreatmentReferral> Referrals = new();

        public TreatmentReferralRepository()
        {
            if (!File.Exists(DoctorReferralFilePath)) return;

            string json = File.ReadAllText(DoctorReferralFilePath);
            Referrals = JsonConvert.DeserializeObject<List<TreatmentReferral>>(json);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(Referrals, Formatting.Indented);
            File.WriteAllText(DoctorReferralFilePath, json);
        }

        public void Add(TreatmentReferral treatmentReferral)
        {
            AssignId(treatmentReferral);
            Referrals.Add(treatmentReferral);
            Save();
        }

        public ObservableCollection<TreatmentReferral> GetPatientsTreatmentReferrals(string patientUsername)
        {
            ObservableCollection<TreatmentReferral> patientsReferrals = new();
            foreach (var treatmentReferral in Referrals)
            {
                if (patientUsername == treatmentReferral.PatientUsername && !treatmentReferral.IsUsed) patientsReferrals.Add(treatmentReferral);
            }
            return patientsReferrals;
        }

        public bool Contains(int id)
        {
            return Referrals.Any(treatmentReferral => treatmentReferral.Id == id);
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        private void AssignId(TreatmentReferral treatmentReferral)
        {
            do
            {
                treatmentReferral.Id = GenerateId();
            } while (Contains(treatmentReferral.Id));
        }
    }
}
