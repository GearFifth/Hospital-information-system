using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ICommands
{
    public class FinishTreatmentCommand : CommandBase
    {
        private HandleTreatmentViewModel _handleTreatmentViewModel;
        public FinishTreatmentCommand(HandleTreatmentViewModel handleTreatmentViewModel)
        {
            _handleTreatmentViewModel = handleTreatmentViewModel;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                if (_handleTreatmentViewModel.SelectedAppointCheckUp)
                {
                    AppointCheckUp();
                }

                _handleTreatmentViewModel.HospitalTreatment.Finish();
                HospitalTreatmentService.Update(_handleTreatmentViewModel.HospitalTreatment);

                Notification.ShowSuccessDialog("Successfully Finished treatment");
                _handleTreatmentViewModel.HandleTreatmentWindow.Close();
                _handleTreatmentViewModel.ReloadHospitalizedPatientsWindow();
            }
            catch (Exception e)
            {
                Notification.ShowErrorDialog(e.Message);
            }
        }

        private void AppointCheckUp()
        {
            AppointmentService.AppointCheckUp(Globals.LoggedUser.Username, _handleTreatmentViewModel.PatientUsername, 10);
            Notification.ShowSuccessDialog("Successfully appointed check up");
        }
    }
}
