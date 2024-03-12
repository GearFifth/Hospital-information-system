using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository
{
    public interface IHospitalTreatmentRepository
    {
        void Save();
        void Add(DateTime treatmentBeginning, DateTime treatmentEnding, string treatment, string patientUsername, string roomName, int id = -1);
        void Update(HospitalTreatment hospitalTreatment);
        List<HospitalTreatment> GetAllHospitalTreatments();
        bool IsPatientAlreadyOnTherapy(TreatmentReferral treatmentReferral);
    }
}
