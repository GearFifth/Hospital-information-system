using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Surveys.Analytics.DoctorRankings;

namespace ZdravoCorp.Surveys.DoctorSurveys
{
    public static class DoctorSurveyService
    {
        public static IDoctorSurveyRepository DoctorSurveyRepository = new DoctorSurveyRepository();
        static DoctorSurveyService()
        {

        }
        public static ObservableCollection<DoctorSurvey> GetAllSurveysForDoctor(string doctorUsername="")
        {
            return DoctorSurveyRepository.GetAllSurveysForDoctor(doctorUsername);
        }

        public static bool AlreadyExists(int appointmentId, string doctorUsername)
        {
            return DoctorSurveyRepository.AlreadyExists(appointmentId, doctorUsername);
        }

        public static void Add(string doctorUsername, DoctorSurvey doctorSurvey)
        {
            DoctorSurveyRepository.Add(doctorUsername, doctorSurvey);
        }

        public static double GetAverageQualityOfService(string doctorUsername)
        {
            return DoctorSurveyRepository.GetAverageQualityOfService(doctorUsername);
        }

        public static double GetAverageRecommendDoctor(string doctorUsername)
        {
            return DoctorSurveyRepository.GetAverageRecommendDoctor(doctorUsername);
        }

        public static double GetAverageOverallSatisfaction(string doctorUsername)
        {
            return DoctorSurveyRepository.GetAverageOverallSatisfaction(doctorUsername);
        }

        public static int GetQualityOfServiceCount(string doctorUsername)
        {
            return DoctorSurveyRepository.GetQualityOfServiceCount(doctorUsername);
        }

        public static int GetRecommendDoctorCount(string doctorUsername)
        {
            return DoctorSurveyRepository.GetRecommendDoctorCount(doctorUsername);
        }

        public static int GetOverallSatisfactionCount(string doctorUsername)
        {
            return DoctorSurveyRepository.GetOverallSatisfactionCount(doctorUsername);
        }

        public static ObservableCollection<DoctorRanking> GetDoctorsSortedByRanking()
        {
            return DoctorSurveyRepository.GetDoctorsSortedByRanking();
        }

        public static ObservableCollection<DoctorRanking> GetBestDoctors(int number)
        {
            return new ObservableCollection<DoctorRanking>(GetDoctorsSortedByRanking().Take(number));
        }

        public static ObservableCollection<DoctorRanking> GetWorstDoctors(int number)
        {
            return new ObservableCollection<DoctorRanking>(GetDoctorsSortedByRanking().Reverse().Take(number));
        }
    }
}
