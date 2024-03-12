using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ICommands
{
    public class ExtendTreatmentCommand : CommandBase
    {
        private HandleTreatmentViewModel _handleTreatmentViewModel;
        public ExtendTreatmentCommand(HandleTreatmentViewModel handleTreatmentViewModel)
        {
            _handleTreatmentViewModel = handleTreatmentViewModel;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                int days = _handleTreatmentViewModel.parseDaysToExtend();
                _handleTreatmentViewModel.HospitalTreatment.Extend(days);
                HospitalTreatmentService.Update(_handleTreatmentViewModel.HospitalTreatment);
                Notification.ShowSuccessDialog("Successfully extended treatment for " + days + " days");

                _handleTreatmentViewModel.HandleTreatmentWindow.Close();
                _handleTreatmentViewModel.ReloadHospitalizedPatientsWindow();
            }
            catch (Exception e)
            {
                Notification.ShowErrorDialog(e.Message);
            }
        }
    }
}
