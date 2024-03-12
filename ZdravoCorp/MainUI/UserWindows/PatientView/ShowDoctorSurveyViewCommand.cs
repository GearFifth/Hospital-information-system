using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Surveys.DoctorSurveys;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.PatientView
{
    class ShowDoctorSurveyViewCommand : CommandBase
    {
        public ShowDoctorSurveyViewCommand()
        {
        }

        public override void Execute(object? Parameter)
        {
            if (Parameter is Appointment appointment)
            {
                int id = appointment.Id;
                string doctorUsername = appointment.DoctorUsername;

                if (DoctorSurveyService.AlreadyExists(id, doctorUsername))
                {
                    Notification.ShowErrorDialog("You already reviewed this doctor after this appointment!");
                }
                else if (!AppointmentService.GetAppointment(id).IsFinished())
                {
                    Notification.ShowErrorDialog("This appointment is not finished!");
                }
                else
                {
                    DoctorSurveyView doctorSurveyView = new DoctorSurveyView(id, doctorUsername);
                    doctorSurveyView.ShowDialog();
                }
            }
        }
    }
}
