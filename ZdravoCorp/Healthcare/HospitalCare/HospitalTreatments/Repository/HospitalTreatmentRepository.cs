using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository
{
    public class HospitalTreatmentRepository : IHospitalTreatmentRepository
    {
        public const string HospitalTreatmentFilePath = "..\\..\\..\\Healthcare\\HospitalCare\\HospitalTreatments\\hospital_treatment.json";
        public static List<HospitalTreatment> HospitalTreatments = new List<HospitalTreatment>();

        public HospitalTreatmentRepository()
        {
            if (!File.Exists(HospitalTreatmentFilePath)) return;

            string json = File.ReadAllText(HospitalTreatmentFilePath);
            HospitalTreatments = JsonConvert.DeserializeObject<List<HospitalTreatment>>(json)!;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(HospitalTreatments, Formatting.Indented);
            File.WriteAllText(HospitalTreatmentFilePath, json);
        }

        public void Update(HospitalTreatment hospitalTreatment)
        {
            int index = HospitalTreatments.FindIndex(treatment => treatment.Id == hospitalTreatment.Id);

            if (index == -1)
            {
                throw new ArgumentException("Hospital treatment with id " + hospitalTreatment.Id + " does not exist.");
            }
            HospitalTreatments[index] = hospitalTreatment;
            Save();
        }

        public HospitalTreatment? GetHospitalTreatment(int id)
        {
            foreach (var hospitalTreatment in HospitalTreatments)
            {
                if (id == hospitalTreatment.Id) return hospitalTreatment;
            }

            return null;
        }

        public List<HospitalTreatment> GetAllHospitalTreatments()
        {
            return HospitalTreatments;
        }

        public bool IsPatientAlreadyOnTherapy(TreatmentReferral treatmentReferral)
        {
            foreach (HospitalTreatment hospitalTreatment in HospitalTreatments)
            {
                if (treatmentReferral.PatientUsername != hospitalTreatment.PatientUsername) continue;

                if (OverlapsWithPatientsHospitalTreatment(treatmentReferral.NumOfDays, hospitalTreatment.TreatmentBeginning, hospitalTreatment.TreatmentEnding)) 
                    return true;
            }

            return false;
        }

        private static bool OverlapsWithPatientsHospitalTreatment(int numberOfDays, DateTime treatmentBeginning, DateTime treatmentEnding)
        {
            return (DateTime.Now >= treatmentBeginning && DateTime.Now <= treatmentEnding) 
                   || (treatmentBeginning >= DateTime.Now && treatmentBeginning <= DateTime.Now.AddDays(numberOfDays));
        }


        public void Add(DateTime treatmentBeginning, DateTime treatmentEnding, string treatment, string patientUsername, string roomName, int id = -1)
        {

            if (id == -1) id = AssignId();

            HospitalTreatment hospitalTreatment = new(treatmentBeginning, treatmentEnding, treatment, patientUsername, roomName, id);

            HospitalTreatments.Add(hospitalTreatment);

            Save();
        }

        private static int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        private int AssignId()
        {
            int id;
            while (true)
            {
                id = GenerateId();

                bool validId = HospitalTreatments.All(hospitalTreatment => id != hospitalTreatment.Id);

                if (validId) break;
            }

            return id;
        }
    }
}
