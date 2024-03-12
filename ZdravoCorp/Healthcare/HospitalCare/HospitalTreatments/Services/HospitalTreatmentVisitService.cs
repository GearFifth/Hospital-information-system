using System;
using System.Collections.Generic;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services
{
    public class HospitalTreatmentVisitService
    {
        public static IHospitalTreatmentVisitRepository _hospitalTreatmentVisitRepository = new HospitalTreatmentVisitRepository();

        public static void Save()
        {
            _hospitalTreatmentVisitRepository.Save();
        }
        public static void Add(int bloodPressure, int temperature, string symptoms, int hospitalTreatmentId,
            DateTime visitDate, string patientUsername)
        {
            _hospitalTreatmentVisitRepository.Add(bloodPressure, temperature, symptoms, hospitalTreatmentId, visitDate, patientUsername);
            Save();
        }

        public static int GetNumberOfVisitsForPatientToday(string patientUsername, int HospitalTreatmentId)
        {
            return _hospitalTreatmentVisitRepository.GetNumberOfVisitsForPatientToday( HospitalTreatmentId);
        }
    }
}
