using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Formatting = Newtonsoft.Json.Formatting;

namespace ZdravoCorp.Healthcare.PatientHealthcare.ReportHistories
{
    public class ReportHistoryRepository
    {
        public const string ReportHistoryFilePath = "..\\..\\..\\Healthcare\\PatientHealthcare\\ReportHistories\\report_history.json";
        public  Dictionary<string, Dictionary<int, string>> PatientsReportHistory = new();

        public ReportHistoryRepository()
        {
            if (!File.Exists(ReportHistoryFilePath)) return;

            string json = File.ReadAllText(ReportHistoryFilePath);
            PatientsReportHistory = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, string>>>(json);
        }

        public  void Save()
        {
            string json = JsonConvert.SerializeObject(PatientsReportHistory, Formatting.Indented);
            File.WriteAllText(ReportHistoryFilePath, json);
        }

        public  void AddReportHistory(string username)
        {
            PatientsReportHistory[username] = new Dictionary<int, string>();
            Save();
        }

        public  void UpdateReport(string username, int id, string reportContent)
        {
            PatientsReportHistory[username][id] = reportContent;
            Save();
        }

        public  void UpdateReportHistory(string username, Dictionary<int, string> reportHistory)
        {
            PatientsReportHistory[username] = reportHistory;
            Save();
        }
        private  DateTime RoundDate(DateTime reportCreationTime)
        {
            return new DateTime(reportCreationTime.Year, reportCreationTime.Month,
                reportCreationTime.Day, reportCreationTime.Hour, reportCreationTime.Minute, reportCreationTime.Second);
        }


    }
}
