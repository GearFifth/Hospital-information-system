using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Surveys.HospitalSurveys
{
    public static class HospitalSurveyService
    {
        public static IHospitalSurveyRepository HospitalSurveyRepository = new HospitalSurveyRepository();

        public static void Add(HospitalSurvey hospitalSurvey)
        {
            HospitalSurveyRepository.Add(hospitalSurvey);
        }

        public static bool HasPatientAlreadySubmitted(string patientUsername)
        {
            return HospitalSurveyRepository.HasPatientAlreadySubmitted(patientUsername);
        }

        public static ObservableCollection<HospitalSurvey> GetAll()
        {
            return HospitalSurveyRepository.GetAll();
        }

        public static double GetAverageQualityOfService()
        {
            return HospitalSurveyRepository.GetAverageQualityOfService();
        }

        public static double GetAverageOverallSatisfaction()
        {
            return HospitalSurveyRepository.GetAverageOverallSatisfaction();
        }

        public static double GetAverageCleanness()
        {
            return HospitalSurveyRepository.GetAverageCleanness();
        }

        public static double GetAverageRecommendHospital()
        {
            return HospitalSurveyRepository.GetAverageRecommendHospital();
        }

        public static int GetQualityOfServiceCount()
        {
            return HospitalSurveyRepository.GetQualityOfServiceCount();
        }

        public static int GetOverallSatisfactionCount()
        {
            return HospitalSurveyRepository.GetOverallSatisfactionCount();
        }

        public static int GetCleannessCount()
        {
            return HospitalSurveyRepository.GetCleannessCount();
        }

        public static int GetRecommendHospitalCount()
        {
            return HospitalSurveyRepository.GetRecommendHospitalCount();
        }
    }


}
