using System;
using ZdravoCorp.Scheduling;

namespace ZdravoCorp.Vacations.VacationRequests
{
    public class VacationRequest
    {
        public enum VacationStatus
        {
            Pending,
            Approved,
            Denied,
            Finished
        }

        public int Id { get; set; }

        public string DoctorUsername { get; set; }
        public TimeSlot Period { get; set; }
        public string Reason { get; set; }
        public VacationStatus Status { get; set; }
        
        public VacationRequest() { }
        public VacationRequest(string doctorUsername, TimeSlot period, string reason, VacationStatus status)
        {
            Id = -1;
            DoctorUsername = doctorUsername;
            Period = period;
            Reason = reason;
            Status = status;
            checkReason();
        }

        public VacationRequest(int id, string doctorUsername, TimeSlot period, string reason, VacationStatus status)
        {
            Id = id;
            DoctorUsername = doctorUsername;
            Period = period;
            Reason = reason;
            Status = status;
            checkReason();
        }

        private void checkReason()
        {
            if (Reason.Length == 0)
            {
                throw new ArgumentException("Reason must be written.");
            }
        }

        public bool IsPending()
        {
            return Status == VacationStatus.Pending;
        }

        public bool IsApproved()
        {
            return Status == VacationStatus.Approved;
        }
        public bool IsDenied()
        {
            return Status == VacationStatus.Denied;
        }

        public bool IsFinished()
        {
            return Status == VacationStatus.Finished;
        }

        public string[] ToTable()
        {
            string[] tableValues =
            {
                Id.ToString(),
                Period.Start.ToString(),
                Period.End.ToString(),
                Reason,
                Status.ToString(),
            };
            return tableValues;
        }

        public void Deny()
        {
            Status = VacationStatus.Denied;
        }

        public void Approve()
        {
            Status = VacationStatus.Approved;
        }
    }
}
