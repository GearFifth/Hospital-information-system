using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Surveys.DoctorSurveys
{
    public class DoctorSurvey
    {
        public int Id { get; set; }
        public string PatientUsername { get; set; }
        public int QualityOfService { get; set; }
        public int OverallSatisfaction { get; set; }
        public int RecommendDoctor { get; set; }
        public string Comment { get; set; }

        public DoctorSurvey(int id, string patientUsername, int qualityOfService, int overallSatisfaction,
            int recommendDoctor, string comment)
        {
            Id = id;
            PatientUsername = patientUsername;
            QualityOfService = qualityOfService;
            OverallSatisfaction = overallSatisfaction;
            RecommendDoctor = recommendDoctor;
            Comment = comment;
        }
    }
}
