using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain
{
    public class TreatmentVisit
    {
        public int HospitalTreatmentId { get; set; }
        public string PatientUsername { get; set; }
        public DateTime VisitDate { get; set; }
        public int BloodPressure { get; set; }
        public int Temperature { get; set; }
        public string Symptoms { get; set; }

        public TreatmentVisit(int bloodPressure, int temperature, string symptoms, int hospitalTreatmentId, DateTime visitDate, string patientUsername)
        {
            BloodPressure=bloodPressure;
            Temperature=temperature;
            Symptoms=symptoms;
            HospitalTreatmentId=hospitalTreatmentId;
            VisitDate=visitDate;
            PatientUsername = patientUsername;
        }
    }
}
