using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ICommands
{
    public class EditTreatmentCommand : CommandBase
    {
        private HandleTreatmentViewModel _handleTreatmentViewModel;

        public EditTreatmentCommand(HandleTreatmentViewModel handleTreatmentViewModel)
        {
            _handleTreatmentViewModel = handleTreatmentViewModel;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                _handleTreatmentViewModel.EditTreatment();
                Notification.ShowSuccessDialog("Successfully edited treatment");

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
