using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Surveys.HospitalSurveys;

namespace ZdravoCorp.Surveys.Analytics
{
    public class HospitalStatisticsViewModel
    {
        public double AverageQualityOfService { get; set; }
        public double AverageCleanness { get; set; }
        public double AverageOverallSatisfaction { get; set; }

        public double AverageRecommendHospital { get; set; }
        
        public int QualityOfServiceRatingCount { get; set; }

        public int CleannessRatingCount { get; set; }

        public int OverallSatisfactionRatingCount { get; set; }
        public int RecommendHospitalRatingCount { get; set; }

        public HospitalStatisticsViewModel()
        {
            SetAverages();
            SetCounts();
        }

        public void SetAverages()
        {
            AverageQualityOfService = HospitalSurveyService.GetAverageQualityOfService();
            AverageCleanness = HospitalSurveyService.GetAverageCleanness();
            AverageOverallSatisfaction = HospitalSurveyService.GetAverageOverallSatisfaction();
            AverageRecommendHospital = HospitalSurveyService.GetAverageRecommendHospital();
        }

        public void SetCounts()
        {
            QualityOfServiceRatingCount = HospitalSurveyService.GetQualityOfServiceCount();
            CleannessRatingCount = HospitalSurveyService.GetCleannessCount();
            OverallSatisfactionRatingCount = HospitalSurveyService.GetOverallSatisfactionCount();
            RecommendHospitalRatingCount = HospitalSurveyService.GetRecommendHospitalCount();
        }
    }
}
