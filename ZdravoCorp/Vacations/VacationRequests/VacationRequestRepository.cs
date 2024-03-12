using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Scheduling;

namespace ZdravoCorp.Vacations.VacationRequests
{
    public class VacationRequestRepository
    {
        public const string VacationRequestFilePath = "..\\..\\..\\Vacations\\VacationRequests\\vacation_requests.json";
        public ObservableCollection<VacationRequest> Requests = new();

        public VacationRequestRepository()
        {
            if (!File.Exists(VacationRequestFilePath)) return;

            string json = File.ReadAllText(VacationRequestFilePath);
            
            Requests = JsonConvert.DeserializeObject<ObservableCollection<VacationRequest>>(json);
            UpdateRequestsStatus();
        }

        public  void Save()
        {
            string json = JsonConvert.SerializeObject(Requests, Formatting.Indented);
            File.WriteAllText(VacationRequestFilePath, json);
        }

        public ObservableCollection<VacationRequest> GetAll()
        {
            return Requests;
        }

        public void Add(VacationRequest vacationRequest)
        {
            AssignId(vacationRequest);
            Requests.Add(vacationRequest);
            Save();
        }

        private void UpdateRequestsStatus()
        {
            foreach (var request in Requests.Where(request => request.Period.End < DateTime.Now && request.IsApproved()))
            {
                request.Status = VacationRequest.VacationStatus.Finished;
            }
            Save();
        }

        public  bool IsOnVacation(string doctorUsername, TimeSlot period)
        {
            return Requests.Any(request => request.DoctorUsername == doctorUsername && request.IsApproved() && request.Period.OverlapsWith(period));
        }

        public ObservableCollection<VacationRequest> GetAllDoctorVacationRequests(string doctorUsername)
        {
            return new ObservableCollection<VacationRequest>(Requests.Where(request => request.DoctorUsername == doctorUsername));
        }

        public void DenyRequest(VacationRequest vacationRequest)
        {
            vacationRequest.Deny();
            Save();
        }

        public void ApproveRequest(VacationRequest vacationRequest)
        {
            vacationRequest.Approve();
            Save();
        }

        public  bool Contains(int id)
        {
            return Requests.Any(request => request.Id == id);
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        private void AssignId(VacationRequest vacationRequest)
        {
            do
            {
                vacationRequest.Id = GenerateId();
            } while (Contains(vacationRequest.Id));
        }
    }
}
