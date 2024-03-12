using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Vacations.VacationManagment;

namespace ZdravoCorp.MainUI.UserWindows.ManagerView.ICommands
{
    public class ShowApproveVacationDialogCommand : CommandBase
    {
        private readonly ManagerViewModel _managerViewModel;
        public ShowApproveVacationDialogCommand(ManagerViewModel managerViewModel)
        {
            _managerViewModel = managerViewModel;
        }

        public override void Execute(object? parameter)
        {
            var managerWindow = parameter as ManagerWindow;
            ApproveVacationDialog approveVacationDialog = new(_managerViewModel.SelectedVacation, managerWindow)
            {
                Owner = managerWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            approveVacationDialog.ShowDialog();
        }
    }
}
