using System.Windows.Input;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Vacations.VacationRequests;

namespace ZdravoCorp.Vacations.VacationManagment
{
    public class ApproveVacationViewModel
    {
        private readonly ApproveVacationDialog _approveVacationDialog;
        public VacationRequest VacationRequest { get; set; }

        private ManagerWindow _managerWindow;

        public ICommand ApproveRequestCommand
        {
            get
            {
                return _approveRequestCommand ??= new ApproveRequestCommand(this);
            }
        }

        private ICommand _approveRequestCommand;

        public ICommand DenyRequestCommand
        {
            get
            {
                return _denyRequestCommand ??= new DenyRequestCommand(this);
            }
        }

        private ICommand _denyRequestCommand;

        public ApproveVacationViewModel(VacationRequest vacationRequest,ApproveVacationDialog approveVacationDialog, ManagerWindow managerWindow)
        {
            VacationRequest = vacationRequest;
            _approveVacationDialog = approveVacationDialog;
            _managerWindow = managerWindow;
        }


        public void RefreshWindow()
        {
            ManagerWindow managerWindow = new();
            managerWindow.Vacations.IsSelected = true;
            managerWindow.Show();
            _approveVacationDialog.Close();
            _managerWindow.Close();
        }


    }
}
