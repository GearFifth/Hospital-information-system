using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Vacations.VacationRequests;

namespace ZdravoCorp.Vacations.VacationManagment
{
    public class DenyRequestCommand:CommandBase
    {
        private readonly ApproveVacationViewModel _approveVacationViewModel;

        public DenyRequestCommand(ApproveVacationViewModel approveVacationViewModel)
        {
            _approveVacationViewModel = approveVacationViewModel;
        }

        public override void Execute(object? parameter)
        {
            VacationRequestService.DenyRequest(_approveVacationViewModel.VacationRequest);
            _approveVacationViewModel.RefreshWindow();
        }
    }
}
