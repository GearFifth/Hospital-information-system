using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Surveys.DoctorSurveys;
using ZdravoCorp.Surveys.HospitalSurveys;

namespace ZdravoCorp.Surveys.Analytics
{
    public class DoctorStatisticsViewModel
    {
        private readonly string _doctorUsername;
        public double AverageQualityOfService { get; set; }
        public double AverageOverallSatisfaction { get; set; }

        public double AverageRecommendDoctor { get; set; }

        public int QualityOfServiceRatingCount { get; set; }
        

        public int OverallSatisfactionRatingCount { get; set; }
        public int RecommendDoctorRatingCount { get; set; }

        public DoctorStatisticsViewModel(string doctorUsername)
        {
            _doctorUsername = doctorUsername;
            SetAverages();
            SetCounts();
        }

        public void SetAverages()
        {
            AverageQualityOfService = DoctorSurveyService.GetAverageQualityOfService(_doctorUsername);
            AverageOverallSatisfaction = DoctorSurveyService.GetAverageOverallSatisfaction(_doctorUsername);
            AverageRecommendDoctor = DoctorSurveyService.GetAverageRecommendDoctor(_doctorUsername);
        }

        public void SetCounts()
        {
            QualityOfServiceRatingCount = DoctorSurveyService.GetQualityOfServiceCount(_doctorUsername);
            OverallSatisfactionRatingCount = DoctorSurveyService.GetOverallSatisfactionCount(_doctorUsername);
            RecommendDoctorRatingCount = DoctorSurveyService.GetRecommendDoctorCount(_doctorUsername);
        }
    }
}
