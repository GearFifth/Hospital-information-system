using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.ManagerView.ICommands
{
    public class EnableApproveVacationButtonCommand : CommandBase
    {
        private readonly ManagerViewModel _managerViewModel;
        public EnableApproveVacationButtonCommand(ManagerViewModel managerViewModel)
        {
            _managerViewModel = managerViewModel;
        }

        public override void Execute(object? parameter)
        {
            _managerViewModel.HasSelectedVacation = true;
        }
    }
}
