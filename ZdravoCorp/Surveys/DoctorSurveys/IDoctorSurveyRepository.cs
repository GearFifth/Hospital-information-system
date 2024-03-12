using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Surveys.Analytics.DoctorRankings;

namespace ZdravoCorp.Surveys.DoctorSurveys
{
    public interface IDoctorSurveyRepository
    {
        public ObservableCollection<DoctorSurvey> GetAllSurveysForDoctor(string doctorUsername = "");
        public bool AlreadyExists(int appointmentId, string doctorUsername);
        public void Add(string doctorUsername, DoctorSurvey doctorSurvey);
        public double GetAverageQualityOfService(string doctorUsername);
        public double GetAverageRecommendDoctor(string doctorUsername);
        public double GetAverageOverallSatisfaction(string doctorUsername);
        public int GetQualityOfServiceCount(string doctorUsername);
        public int GetRecommendDoctorCount(string doctorUsername);
        public int GetOverallSatisfactionCount(string doctorUsername);
        public ObservableCollection<DoctorRanking> GetDoctorsSortedByRanking();
    }
}
