using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.MainUI.Notices;

namespace ZdravoCorp.Surveys.HospitalSurveys
{
    public class HospitalSurvey
    {
        public int Id { get; set; }
        public string PatientUsername { get; set; }
        public int QualityOfService { get; set; }
        public int Cleanness { get; set; }
        public int OverallSatisfaction { get; set; }
        public int RecommendHospital { get; set; }
        public string Comment { get; set; }

        public HospitalSurvey(string patientUsername, int qualityOfService, int cleanness, int overallSatisfaction,
            int recommendHospital, string comment)
        {
            Id = -1;
            PatientUsername = patientUsername;
            QualityOfService = qualityOfService;
            Cleanness = cleanness;
            OverallSatisfaction = overallSatisfaction;
            RecommendHospital = recommendHospital;
            Comment = comment;
        }
    }
}
