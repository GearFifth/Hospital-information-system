using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Surveys.DoctorSurveys;

namespace ZdravoCorp.Surveys.Analytics.DoctorRankings
{
    public class BestDoctorsViewModel
    {
        public ObservableCollection<DoctorRanking> DoctorRankings { get; set; }

        public BestDoctorsViewModel()
        {
            DoctorRankings = DoctorSurveyService.GetBestDoctors(3);
        }
    }
}
