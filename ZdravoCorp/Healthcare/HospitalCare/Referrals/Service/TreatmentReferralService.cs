using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Service
{
    public class TreatmentReferralService
    {
        public static ITreatmentReferralRepository TreatmentReferralRepository = new TreatmentReferralRepository();

        public static void AddReferral(TreatmentReferral referral)
        {
            TreatmentReferralRepository.Add(referral);
        }
        public static ObservableCollection<TreatmentReferral> GetPatientsTreatmentReferrals(string patientUsername)
        {
            return TreatmentReferralRepository.GetPatientsTreatmentReferrals(patientUsername);
        }

        public static void Save()
        {
            TreatmentReferralRepository.Save();
        }
    }
}
