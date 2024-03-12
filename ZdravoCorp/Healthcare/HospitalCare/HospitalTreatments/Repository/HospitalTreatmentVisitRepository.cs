using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository
{
    public class HospitalTreatmentVisitRepository : IHospitalTreatmentVisitRepository
    {
        public const string HospitalTreatmentFilePath = "..\\..\\..\\Healthcare\\HospitalCare\\HospitalTreatments\\hospital_treatment_visit.json";
        public static List<TreatmentVisit>? HospitalTreatmentsVisits = new();

        public HospitalTreatmentVisitRepository()
        {
            if (!File.Exists(HospitalTreatmentFilePath)) return;

            string json = File.ReadAllText(HospitalTreatmentFilePath);
            HospitalTreatmentsVisits = JsonConvert.DeserializeObject<List<TreatmentVisit>>(json)!;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(HospitalTreatmentsVisits, Formatting.Indented);
            File.WriteAllText(HospitalTreatmentFilePath, json);
        }

        public void Add(int bloodPressure, int temperature, string symptoms, int hospitalTreatmentId,
            DateTime visitDate, string patientUsername)
        {
            HospitalTreatmentsVisits.Add(new TreatmentVisit(bloodPressure, temperature, symptoms, hospitalTreatmentId, visitDate, patientUsername));
        }

        public int GetNumberOfVisitsForPatientToday(int hospitalTreatmentId)
        {
            int numberOfVisitsToday = 0;
            foreach (TreatmentVisit treatmentVisit in HospitalTreatmentsVisits ??= new List<TreatmentVisit>())
            {
                if (ValidateVisit( hospitalTreatmentId, treatmentVisit)) continue;
                numberOfVisitsToday++;
            }

            return numberOfVisitsToday;
        }

        private static bool ValidateVisit(int hospitalTreatmentId, TreatmentVisit treatmentVisit)
        {
            if (treatmentVisit.VisitDate.Date != DateTime.Today) return true;

            if (treatmentVisit.HospitalTreatmentId != hospitalTreatmentId) return true;

            return false;
        }

        public static List<TreatmentVisit> GetPatientsTreatmentVisits(string patientUsername)
        {
            List<TreatmentVisit> treatmentVisits = new List<TreatmentVisit>();
            foreach (var hospitalTreatmentsVisit in HospitalTreatmentsVisits ??= new List<TreatmentVisit>())
            {
                if (hospitalTreatmentsVisit.PatientUsername != patientUsername) continue;

                treatmentVisits.Add(hospitalTreatmentsVisit);
            }

            return treatmentVisits;
        }
    }
}
