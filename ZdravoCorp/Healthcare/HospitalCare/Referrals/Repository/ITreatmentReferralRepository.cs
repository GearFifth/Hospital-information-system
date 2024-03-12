using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository
{
    public interface ITreatmentReferralRepository
    {
        public void Add(TreatmentReferral treatmentReferral);
        public ObservableCollection<TreatmentReferral> GetPatientsTreatmentReferrals(string patientUsername);
        public void Save();
    }
}
