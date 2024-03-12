using System;
using System.Collections.Generic;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services
{
    public class HospitalTreatmentService
    {
        public static IHospitalTreatmentRepository _hospitalTreatmentRepository = new HospitalTreatmentRepository();

        public static void Save()
        {
            _hospitalTreatmentRepository.Save();
        }
        public static void Add(DateTime treatmentBeginning, DateTime treatmentEnding, string treatment, string patientUsername, string roomName)
        {
            _hospitalTreatmentRepository.Add(treatmentBeginning, treatmentEnding, treatment, patientUsername, roomName);
        }
        public static void Update(HospitalTreatment hospitalTreatment)
        {
            _hospitalTreatmentRepository.Update(hospitalTreatment);
        }

        public static List<HospitalTreatment> GetAllHospitalTreatments()
        {
            return _hospitalTreatmentRepository.GetAllHospitalTreatments();
        }

        public static bool IsPatientAlreadyOnTherapy(TreatmentReferral selectedTreatmentReferral)
        {
            return _hospitalTreatmentRepository.IsPatientAlreadyOnTherapy(selectedTreatmentReferral);
        }
    }
}
