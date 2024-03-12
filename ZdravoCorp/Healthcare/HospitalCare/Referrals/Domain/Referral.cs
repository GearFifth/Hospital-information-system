using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain
{
    public abstract class Referral
    {
        public int Id { get; set; }
        public string PatientUsername { get; set; }
        public bool IsUsed { get; set; }

        protected Referral(string patientUsername)
        {
            Id = -1;
            PatientUsername = patientUsername;
            IsUsed = false;
        }

        public void Use()
        {
            IsUsed = true;
        }
    }
}
