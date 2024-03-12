using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions
{
    public class DrugPrescriptionRepository : IDrugPrescriptionRepository
    {
        public const string PrescriptionsFilePath = "..\\..\\..\\Healthcare\\PatientHealthcare\\DrugPrescriptions\\drug_prescriptions.json";
        public List<DrugPrescription> DrugPrescriptions = new();

        public DrugPrescriptionRepository()
        {
            if (!File.Exists(PrescriptionsFilePath)) return;

            string json = File.ReadAllText(PrescriptionsFilePath);
            DrugPrescriptions = JsonConvert.DeserializeObject<List<DrugPrescription>>(json);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(DrugPrescriptions, Formatting.Indented);
            File.WriteAllText(PrescriptionsFilePath, json);
        }

        public void Add(DrugPrescription drugPrescription)
        {
            AssignId(drugPrescription);
            DrugPrescriptions.Add(drugPrescription);
            Save();
        }

        public bool Contains(int id)
        {
            return DrugPrescriptions.Any(prescription => prescription.Id == id);
        }

        public List<DrugPrescription> GetPatientsPrescriptions(string patientsUsername)
        {
            return DrugPrescriptions.Where(drugPrescription => drugPrescription.PatientUsername == patientsUsername).ToList();
        }

        public DrugPrescription? GetDrugPrescription(int id)
        {
            return DrugPrescriptions.FirstOrDefault(drugPrescription => drugPrescription.Id == id);
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        private void AssignId(DrugPrescription drugPrescription)
        {
            do
            {
                drugPrescription.Id = GenerateId();
            } while (Contains(drugPrescription.Id));
        }
    }
}
