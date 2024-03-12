using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository
{
    public interface IHospitalTreatmentVisitRepository
    {
        void Save();
        void Add(int bloodPressure, int temperature, string symptoms, int hospitalTreatmentId, DateTime visitDate, string patientUsername);
        int GetNumberOfVisitsForPatientToday( int hospitalTreatmentId);
    }
}
