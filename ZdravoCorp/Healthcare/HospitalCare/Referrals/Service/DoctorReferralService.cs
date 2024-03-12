using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository;
using ZdravoCorp.Healthcare.Roles.Patient;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Service
{
    public class DoctorReferralService
    {
        public static IDoctorReferralRepository DoctorReferralRepository = new DoctorReferralRepository();
        public static void AddDoctorReferral(DoctorReferral referral)
        {
            DoctorReferralRepository.Add(referral);
        }

        public static void SaveDoctorReferrals()
        {
            DoctorReferralRepository.Save();
        }

        public static List<DoctorReferral> GetPatientsUnusedDoctorReferrals(string patientUsername)
        {
            return DoctorReferralRepository.GetUnusedPatientsReferrals(patientUsername);
        }
    }
}
