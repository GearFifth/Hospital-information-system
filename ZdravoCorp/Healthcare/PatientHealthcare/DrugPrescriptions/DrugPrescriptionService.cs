using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.MainUI.Notices;

namespace ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions
{
    public class DrugPrescriptionService
    {
        public static IDrugPrescriptionRepository DrugPrescriptionRepository = new DrugPrescriptionRepository();
        public static void AddPrescription(DrugPrescription drugPrescription)
        {
            DrugPrescriptionRepository.Add(drugPrescription);
            NoticeService.GenerateNoticesForPatient(drugPrescription);
        }

        public static List<DrugPrescription> GetPatientsUnusedPrescriptions(string patientsUsername)
        {
            return DrugPrescriptionRepository.GetPatientsPrescriptions(patientsUsername);
        }

        public static DrugPrescription? GetDrugPrescription(int id)
        {
            return DrugPrescriptionRepository.GetDrugPrescription(id);
        }

        public static void SaveRepository()
        {
            DrugPrescriptionRepository.Save();
        }
    }
}
