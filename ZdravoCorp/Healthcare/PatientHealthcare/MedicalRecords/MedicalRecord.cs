using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords
{
    public class MedicalRecord : Serializable
    {
        public string PatientUsername { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Dictionary<DateTime, string> ReportHistory { get; set; }
        public List<string> DiseaseHistory { get; set; }

        public List<string> Allergies { get; set; }

        public List<int> TreatmentHistory { get; set; }

        public MedicalRecord() { }
        public MedicalRecord(double weight, double height, string username, DateOnly dateOfBirth, List<string> diseaseHistory, List<string>? allergies = default)
        {
            PatientUsername = username;
            Weight = weight;
            Height = height;
            DateOfBirth = dateOfBirth;
            DiseaseHistory = diseaseHistory;
            Allergies = allergies?.ToList() ?? new List<string>();
        }

        private string DiseaseHistoryToString()
        {
            bool skipComma = true;
            string stringBuilder = string.Empty;

            foreach (var disease in DiseaseHistory)
            {
                if (!skipComma)
                {
                    stringBuilder += "," + disease;
                }
                else
                {
                    stringBuilder += disease;
                    skipComma = false;
                }
            }
            return stringBuilder;
        }

        private void StringToAllergies(string diseaseHistoryString)
        {
            Allergies = new List<string>(diseaseHistoryString.Split(','));
        }

        private string AllergiesToString()
        {
            bool skipComma = true;
            string stringBuilder = string.Empty;

            if (Allergies == null) return stringBuilder;
            foreach (var allergy in Allergies)
            {
                if (!skipComma)
                {
                    stringBuilder += "," + allergy;
                }
                else
                {
                    stringBuilder += allergy;
                    skipComma = false;
                }
            }

            return stringBuilder;
        }

        private void StringToDiseaseHistory(string diseaseHistoryString)
        {
            DiseaseHistory = new List<string>(diseaseHistoryString.Split(','));
        }

        public void AddDisease(string disease)
        {
            DiseaseHistory.Add(disease);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                PatientUsername,
                Math.Round(Weight, 2).ToString(),
                Math.Round(Height, 2).ToString(),
                DateOfBirth.ToString("O"),
                DiseaseHistoryToString(),
                AllergiesToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PatientUsername = values[0];
            Weight = double.Parse(values[1]);
            Height = double.Parse(values[2]);
            DateOfBirth = DateOnly.ParseExact(values[3], "O", null);
            StringToDiseaseHistory(values[4]);
            StringToAllergies(values[5]);
        }

    }
}
