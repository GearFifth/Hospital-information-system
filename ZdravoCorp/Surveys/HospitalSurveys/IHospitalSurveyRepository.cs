using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Surveys.HospitalSurveys
{
    public interface IHospitalSurveyRepository
    {
        public void Add(HospitalSurvey hospitalSurvey);
        public bool HasPatientAlreadySubmitted(string patientUsername);
        public ObservableCollection<HospitalSurvey> GetAll();
        public double GetAverageQualityOfService();
        public double GetAverageOverallSatisfaction();
        public double GetAverageCleanness();
        public double GetAverageRecommendHospital();
        public int GetQualityOfServiceCount();
        public int GetOverallSatisfactionCount();
        public int GetCleannessCount();
        public int GetRecommendHospitalCount();
    }
}
