using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Vacations.VacationRequests.View;

namespace ZdravoCorp.MainUI.UserWindows.DoctorView
{
    public class ShowVacationRequestDialogCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            var doctorWindow = parameter as DoctorWindow;
            VacationRequestDialog vacationRequestDialog = new(doctorWindow);
            vacationRequestDialog.Owner = doctorWindow;
            vacationRequestDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            vacationRequestDialog.ShowDialog();
        }
    }
}
