using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.Views;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ICommands
{
    public class ShowHandleTreatmentWindowCommand : CommandBase
    {
        private HospitalizedPatientsViewModel _hospitalizedPatientsViewModel;
        public ShowHandleTreatmentWindowCommand(HospitalizedPatientsViewModel hospitalizedPatientsViewModel)
        {
            _hospitalizedPatientsViewModel = hospitalizedPatientsViewModel;
        }

        public override void Execute(object? parameter)
        {
            var hospitalizedPatientsWindow = parameter as HospitalizedPatientsWindow;
            if (isSelectedTreatmentValid())
            {
                HandleTreatmentWindow handleTreatmentWindow = new HandleTreatmentWindow(_hospitalizedPatientsViewModel.SelectedTreatment, hospitalizedPatientsWindow);
                handleTreatmentWindow.Owner = hospitalizedPatientsWindow;
                handleTreatmentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                handleTreatmentWindow.ShowDialog();
                return;
            }
            Notification.ShowErrorDialog("Please select active treatment.");
        }

        private bool isSelectedTreatmentValid()
        {
            return _hospitalizedPatientsViewModel.SelectedTreatment is not null &&
                   _hospitalizedPatientsViewModel.SelectedTreatment.IsActive();
        }
    }
}
