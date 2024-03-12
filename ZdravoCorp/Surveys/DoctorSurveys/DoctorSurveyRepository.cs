using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.Surveys.Analytics.DoctorRankings;

namespace ZdravoCorp.Surveys.DoctorSurveys
{
    public class DoctorSurveyRepository : IDoctorSurveyRepository
    {
        public const string doctorRepositoryFilePath = "..\\..\\..\\Surveys\\DoctorSurveys\\doctor_surveys.json";
        public Dictionary<string, ObservableCollection<DoctorSurvey>> doctorSurveyRepository = new();

        public DoctorSurveyRepository()
        {
            if (!File.Exists(doctorRepositoryFilePath)) return;

            string json = File.ReadAllText(doctorRepositoryFilePath);

            doctorSurveyRepository = JsonConvert.DeserializeObject<Dictionary<string, ObservableCollection<DoctorSurvey>>>(json);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(doctorSurveyRepository, Formatting.Indented);
            File.WriteAllText(doctorRepositoryFilePath, json);
        }
        public void Add(string doctorUsername, DoctorSurvey doctorSurvey)
        {
            doctorSurveyRepository[doctorUsername].Add(doctorSurvey);
            Save();
        }

        public ObservableCollection<DoctorSurvey> GetAllSurveysForDoctor(string doctorUsername)
        {
            return doctorSurveyRepository.ContainsKey(doctorUsername) ? doctorSurveyRepository[doctorUsername] : new ObservableCollection<DoctorSurvey>();
        }

        public bool AlreadyExists(int appointmentId, string doctorUsername)
        {
            ObservableCollection<DoctorSurvey> surveys = GetAllSurveysForDoctor(doctorUsername);

            foreach (DoctorSurvey survey in surveys)
            {
                if (appointmentId == survey.Id) return true;
            }

            return false;
        }

        public double GetAverageQualityOfService(string doctorUsername)
        {
            return GetAllSurveysForDoctor(doctorUsername).Any() ? GetAllSurveysForDoctor(doctorUsername).Select(survey=>survey.QualityOfService).Average() : 0;
        }

        public double GetAverageRecommendDoctor(string doctorUsername)
        {
            return GetAllSurveysForDoctor(doctorUsername).Any() ? GetAllSurveysForDoctor(doctorUsername).Select(survey => survey.RecommendDoctor).Average() : 0;
        }

        public double GetAverageOverallSatisfaction(string doctorUsername)
        {
            return GetAllSurveysForDoctor(doctorUsername).Any() ? GetAllSurveysForDoctor(doctorUsername).Select(survey => survey.OverallSatisfaction).Average() : 0;
        }

        public int GetQualityOfServiceCount(string doctorUsername)
        {
            return GetAllSurveysForDoctor(doctorUsername).Any() ? GetAllSurveysForDoctor(doctorUsername).Select(survey=>survey.QualityOfService).Count() : 0;
        }

        public int GetRecommendDoctorCount(string doctorUsername)
        {
            return GetAllSurveysForDoctor(doctorUsername).Any() ? GetAllSurveysForDoctor(doctorUsername).Select(survey => survey.RecommendDoctor).Count() : 0;
        }

        public int GetOverallSatisfactionCount(string doctorUsername)
        {
            return GetAllSurveysForDoctor(doctorUsername).Any() ? GetAllSurveysForDoctor(doctorUsername).Select(survey => survey.OverallSatisfaction).Count() : 0;
        }

        public ObservableCollection<DoctorRanking> GetDoctorsSortedByRanking()
        {
            var doctorUsernames = doctorSurveyRepository.Keys;
            var doctorRanking = new List<DoctorRanking>();
            foreach (var doctorUsername in doctorUsernames)
            {
                var value = GetAverageRating(doctorUsername);
                doctorRanking.Add(new DoctorRanking(doctorUsername,value));
            }

            return new ObservableCollection<DoctorRanking>(doctorRanking.OrderBy(doctorRanking => doctorRanking.Ranking).Reverse());
        }

        private double GetAverageRating(string doctorUsername)
        {
            return GetAllSurveysForDoctor(doctorUsername).Any() ? Math.Round(GetAllSurveysForDoctor(doctorUsername).Select(survey =>
                Convert.ToDouble(survey.OverallSatisfaction + survey.QualityOfService + survey.RecommendDoctor)/3).Average(),2) : 0;
        }
    }
}
