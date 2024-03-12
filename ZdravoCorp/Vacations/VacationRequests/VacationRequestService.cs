using System.Collections.ObjectModel;
using ZdravoCorp.Scheduling.Appointments;

namespace ZdravoCorp.Vacations.VacationRequests
{
    public class VacationRequestService
    {
        public static VacationRequestRepository VacationRequestRepository = new();
        public static void AddVacationRequest(VacationRequest vacationRequest)
        {
            VacationRequestRepository.Add(vacationRequest);
        }

        public static ObservableCollection<VacationRequest> GetAllDoctorVacationRequests(string doctorUsername)
        {
            return VacationRequestRepository.GetAllDoctorVacationRequests(doctorUsername);
        }

        public static ObservableCollection<VacationRequest> GetAll()
        {
            return VacationRequestRepository.GetAll();
        }

        public static void DenyRequest(VacationRequest vacationRequest)
        {
            VacationRequestRepository.DenyRequest(vacationRequest);
        }

        public static void ApproveRequest(VacationRequest vacationRequest)
        {
            VacationRequestRepository.ApproveRequest(vacationRequest);
            AppointmentService.CancelAppointmentsInRange(vacationRequest.Period,vacationRequest.DoctorUsername);
        }
    }
}
