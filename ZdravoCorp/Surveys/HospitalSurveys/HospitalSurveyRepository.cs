using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;
using ZdravoCorp.Scheduling.Appointments;

namespace ZdravoCorp.Surveys.HospitalSurveys
{
    public class HospitalSurveyRepository : IHospitalSurveyRepository
    {
        public const string HospitalSurveysFilePath = "..\\..\\..\\Surveys\\HospitalSurveys\\hospital_surveys.json";
        public ObservableCollection<HospitalSurvey> HospitalSurveys = new();

        public HospitalSurveyRepository()
        {
            if (!File.Exists(HospitalSurveysFilePath)) return;

            string json = File.ReadAllText(HospitalSurveysFilePath);
            HospitalSurveys = JsonConvert.DeserializeObject<ObservableCollection<HospitalSurvey>>(json);
        }

        private void Save()
        {
            string json = JsonConvert.SerializeObject(HospitalSurveys, Formatting.Indented);
            File.WriteAllText(HospitalSurveysFilePath, json);
        }

        public void Add(HospitalSurvey hospitalSurvey)
        {
            if (hospitalSurvey.Id == -1)
            {
                AssignId(hospitalSurvey);
            }
            HospitalSurveys.Add(hospitalSurvey);
            Save();
        }

        public ObservableCollection<HospitalSurvey> GetAll()
        {
            return HospitalSurveys;
        }

        public bool HasPatientAlreadySubmitted(string patientUsername)
        {
            foreach (HospitalSurvey hospitalSurvey in HospitalSurveys)
            {
                if (hospitalSurvey.PatientUsername == patientUsername) return true;
            }

            return false;
        }

        private void AssignId(HospitalSurvey hospitalSurvey)
        {
            do
            {
                hospitalSurvey.Id = GenerateId();
            } while (ContainsId(hospitalSurvey.Id));
        }

        private bool ContainsId(int id)
        {
            foreach (HospitalSurvey hospitalSurvey in HospitalSurveys)
            {
                if (id == hospitalSurvey.Id) return true;
            }

            return false;
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        public double GetAverageQualityOfService()
        {
            return HospitalSurveys.Select(survey => survey.QualityOfService).Average();
        }

        public double GetAverageCleanness()
        {
            return HospitalSurveys.Select(survey=> survey.Cleanness).Average();
        }

        public double GetAverageOverallSatisfaction()
        {
            return HospitalSurveys.Select(survey=>survey.OverallSatisfaction).Average();
        }

        public double GetAverageRecommendHospital()
        {
            return HospitalSurveys.Select(survey => survey.RecommendHospital).Average();
        }

        public int GetQualityOfServiceCount()
        {
            return HospitalSurveys.Select(survey=>survey.QualityOfService).Count();
        }

        public int GetCleannessCount()
        {
            return HospitalSurveys.Select(survey=>survey.Cleanness).Count();
        }

        public int GetOverallSatisfactionCount()
        {
            return HospitalSurveys.Select(survey=>survey.OverallSatisfaction).Count();
        }

        public int GetRecommendHospitalCount()
        {
            return HospitalSurveys.Select(survey=>survey.RecommendHospital).Count();
        }
    }
}
