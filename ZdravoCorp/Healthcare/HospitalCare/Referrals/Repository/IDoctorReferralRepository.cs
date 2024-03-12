using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.Roles.Patient;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository
{
    public interface IDoctorReferralRepository
    {
        public void Add(DoctorReferral doctorReferral);

        public void Save();

        public List<DoctorReferral> GetUnusedPatientsReferrals(string patientUsername);

    }
}
