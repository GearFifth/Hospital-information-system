using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Vacations.VacationRequests;

namespace ZdravoCorp.Vacations.VacationManagment
{
    public class ApproveRequestCommand:CommandBase
    {
        private readonly ApproveVacationViewModel _approveVacationViewModel;
        public ApproveRequestCommand(ApproveVacationViewModel approveVacationViewModel)
        {
            _approveVacationViewModel = approveVacationViewModel;
        }

        public override void Execute(object? parameter)
        {
            VacationRequestService.ApproveRequest(_approveVacationViewModel.VacationRequest);
            _approveVacationViewModel.RefreshWindow();
        }
    }
}
