using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.MainUI.Notices
{
    public class NoticeRepository
    {
        public const string NoticeFilePath = "..\\..\\..\\MainUI\\Notices\\notice.csv";

        public  List<Notice> Notices = new();
        public  Serializer<Notice> NoticeSerializer = new();

        public NoticeRepository()
        {
            Notices = NoticeSerializer.fromCSV(NoticeFilePath);
        }

        public  void SaveRepository()
        {
            NoticeSerializer.toCSV(NoticeFilePath, Notices);
        }

        public  void Add(Notice notice)
        {
            Notices.Add(notice);
            SaveRepository();
        }

        public  List<Notice> GetAllUsersNotices(string username)
        {
            return Notices.Where(notice => notice.NotifiedUserUsername == username).ToList();
        }

        public  void GenerateNoticesForPatient(DrugPrescription drugPrescription)
        {
            DateTime noticeTime = drugPrescription.Period.Start;
            string content = "Take prescribed medicine - " + drugPrescription.DrugName;
            for (int i = 0; i < (drugPrescription.Period.End - drugPrescription.Period.Start).Days; i++)
            {
                int index = 24 / drugPrescription.DailyDose;
                for (int j = 1; j <= drugPrescription.DailyDose; j++)
                {
                    noticeTime = noticeTime.AddHours(index);
                    Notice notice = new Notice(noticeTime, content, drugPrescription.PatientUsername);
                    Add(notice);
                }
            }
        }

        public  List<Notice> GetAllUsersNoticesInTimeRange(string username, DateTime leadTime)
        {
            List<Notice> filteredNotices = new List<Notice>();
            foreach (Notice notice in GetAllUsersNotices(username))
            {
                if (notice.TimeOfNotice < leadTime && notice.TimeOfNotice > DateTime.Now)
                {
                    filteredNotices.Add(notice);
                }
            }
            return filteredNotices;
        }
    }
}
