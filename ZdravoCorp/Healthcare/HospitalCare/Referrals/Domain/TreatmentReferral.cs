using System;
using System.Collections.Generic;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository;

namespace ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain
{
    public class TreatmentReferral : Referral
    {
        public int NumOfDays;
        public string Therapy;
        public string AdditionalExaminations;


        public TreatmentReferral(string patientUsername, int numOfDays, string therapy, string additionalExaminations) : base(patientUsername)
        {
            if (numOfDays <= 0)
            {
                throw new ArgumentException("Number of days must be at least 1.");
            }
            else if (therapy.Length == 0)
            {
                throw new ArgumentException("Therapy must be written.");
            }

            NumOfDays = numOfDays;
            Therapy = therapy;
            AdditionalExaminations = additionalExaminations;
        }
    }
}
