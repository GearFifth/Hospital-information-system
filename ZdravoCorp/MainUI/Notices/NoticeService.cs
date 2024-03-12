using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;

namespace ZdravoCorp.MainUI.Notices
{
    public static class NoticeService
    {
        public static NoticeRepository NoticeRepository = new();
        public static List<Notice> GetAllUsersNotices(string username)
        {
            return NoticeRepository.GetAllUsersNotices(username);
        }
        public static List<Notice> GetAllUsersNoticesInTimeRange(string username, DateTime leadTime)
        {
            return NoticeRepository.GetAllUsersNoticesInTimeRange(username, leadTime);
        }

        public static void GenerateNoticesForPatient(DrugPrescription drugPrescription)
        {
            NoticeRepository.GenerateNoticesForPatient(drugPrescription);
        }

        public static void Add(Notice notice)
        {
            NoticeRepository.Add(notice);
        }
    }
}
