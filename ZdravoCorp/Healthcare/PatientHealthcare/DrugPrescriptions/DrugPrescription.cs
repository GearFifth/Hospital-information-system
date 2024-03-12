using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords;
using ZdravoCorp.Healthcare.Pharmacy.Drugs;
using ZdravoCorp.Scheduling;

namespace ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions
{
    public class DrugPrescription
    {
        public enum ConsumeTime
        {
            BeforeMeal,
            DuringMeal,
            AfterMeal,
            NotImportant
        }

        public int Id { get; set; }
        public string PatientUsername { get; set; }
        public string DrugName { get; set; }
        public TimeSlot Period { get; set; }
        public int DailyDose { get; set; }
        public ConsumeTime consumeTime { get; set; }
        public bool IsSentToExamination { get; set; }
        public DateTime NextDoseDate { get; set; }

        public DrugPrescription(string patientUsername, string drugName, TimeSlot period, int dailyDose, ConsumeTime consumeTime, DateTime nextDoseDate = default, bool isSentToExamination = false)
        {
            if (dailyDose <= 0)
            {
                throw new ArgumentException("Daily dose must be at least 1.");
            }

            CheckAllergy(patientUsername, drugName);

            Id = -1;
            PatientUsername = patientUsername;
            DrugName = drugName;
            Period = period;
            DailyDose = dailyDose;
            this.consumeTime = consumeTime;
            IsSentToExamination = isSentToExamination;
            NextDoseDate = nextDoseDate;
        }

        public static void CheckAllergy(string patientUsername, string drugName)
        {
            foreach (string allergy in MedicalRecordService.GetMedicalRecord(patientUsername)!.Allergies)
            {
                if (DrugService.GetDrug(drugName).Contains(allergy))
                {
                    throw new Exception("Patient has an allergy to ingredients in a specific drug");
                }
            }
        }

        public void CalculateNextDoseDate()
        {
            Drug drug = DrugService.GetDrug(DrugName)!;

            if (DateTime.Now < NextDoseDate) throw new Exception("Next purchase can be done after: " + NextDoseDate);

            if (DateTime.Now > Period.End) throw new Exception("Medication prescription has expired. ");

            CheckNextDoseDateAndCurrentDate(drug);
        }

        private void CheckNextDoseDateAndCurrentDate(Drug drug)
        {
            double numberOfDays = (double)drug.NumberOfPills / DailyDose;
            if (NextDoseDate < Period.Start && DateTime.Now < Period.Start)
            {
                NextDoseDate = Period.Start.AddDays(Math.Ceiling(numberOfDays - 1));
            }
            else if (NextDoseDate < Period.Start && DateTime.Now > Period.Start)
            {
                NextDoseDate = DateTime.Now.AddDays(Math.Ceiling(numberOfDays - 1));
            }
            else if (NextDoseDate > Period.Start && DateTime.Now > NextDoseDate)
            {
                NextDoseDate = DateTime.Now.AddDays(Math.Ceiling(numberOfDays - 1));
            }
            else if (NextDoseDate > Period.Start && DateTime.Now < NextDoseDate)
            {
                NextDoseDate = NextDoseDate.AddDays(Math.Ceiling(numberOfDays));
            }
        }
    }
}
