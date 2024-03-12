using System;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Vacations.VacationRequests.ViewModel;

namespace ZdravoCorp.Vacations.VacationRequests.ICommands
{
    public class CreateVacationRequestCommand : CommandBase
    {
        private VacationRequestDialogViewModel _vacationRequestDialogViewModel;
        public CreateVacationRequestCommand(VacationRequestDialogViewModel vacationRequestDialogViewModel)
        {
            _vacationRequestDialogViewModel = vacationRequestDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                VacationRequestService.AddVacationRequest(ParseVacationRequestFromDialog());
                Notification.ShowSuccessDialog("Successfully requested vacation.");

                _vacationRequestDialogViewModel.VacationRequestDialog.Close();
                _vacationRequestDialogViewModel.ReloadParentWindow();
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        private VacationRequest ParseVacationRequestFromDialog()
        {
            if (_vacationRequestDialogViewModel.StartDate < DateTime.Today.AddDays(2))
            {
                throw new ArgumentException("Vacation needs to be requested at least 2 days before.");
            }

            string doctorUsername = Globals.LoggedUser.Username;
            TimeSlot period = new(_vacationRequestDialogViewModel.StartDate.Date, _vacationRequestDialogViewModel.EndDate.Date);
            return new VacationRequest(doctorUsername, period, _vacationRequestDialogViewModel.Reason, VacationRequest.VacationStatus.Pending);
        }
    }
}
