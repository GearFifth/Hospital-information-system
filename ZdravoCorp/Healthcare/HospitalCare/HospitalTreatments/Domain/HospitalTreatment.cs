using System;
using System.Collections.Generic;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain
{
    public class HospitalTreatment
    {
        public enum TreatmentStatus
        {
            Active,
            Finished
        }
        public int Id { get; set; }
        public DateTime TreatmentBeginning { get; set; }
        public DateTime TreatmentEnding { get; set; }
        public string Treatment { get; set; }
        public string PatientUsername { get; set; }
        public string RoomName { get; set; }
        public TreatmentStatus Status { get; set; }

        public HospitalTreatment(DateTime treatmentBeginning, DateTime treatmentEnding, string treatment, string patientUsername, string roomName, int id = -1, TreatmentStatus status = TreatmentStatus.Active)
        {
            Id = id;
            TreatmentBeginning = treatmentBeginning;
            TreatmentEnding = treatmentEnding;
            Treatment = treatment;
            PatientUsername = patientUsername;
            RoomName = roomName;
            Status = status;
        }
        public void Finish()
        {
            if (Status != TreatmentStatus.Active)
            {
                throw new InvalidOperationException("This Treatment is already finished.");
            }

            TreatmentEnding = DateTime.Now;
            Status = TreatmentStatus.Finished;
        }

        public void Extend(int days)
        {
            TreatmentEnding = TreatmentEnding.AddDays(days);
        }

        public bool IsActive()
        {
            return Status == TreatmentStatus.Active;
        }
    }
}
