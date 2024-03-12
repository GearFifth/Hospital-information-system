using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Surveys.Analytics.DoctorRankings
{
    public class DoctorRanking
    {
        public string Username { get; set; }
        public double Ranking { get; set; }
        public DoctorRanking() { }

        public DoctorRanking(string username, double ranking)
        {
            Username = username;
            Ranking = ranking;
        }

    }
}
