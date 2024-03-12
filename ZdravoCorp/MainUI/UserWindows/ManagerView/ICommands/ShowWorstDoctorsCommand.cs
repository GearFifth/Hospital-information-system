using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Surveys.Analytics.DoctorRankings;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.ManagerView.ICommands
{
    public class ShowWorstDoctorsCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            var managerWindow = parameter as ManagerWindow;
            WorstDoctorsWindow worstDoctorsWindow = new WorstDoctorsWindow();
            worstDoctorsWindow.Owner = managerWindow;
            worstDoctorsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            worstDoctorsWindow.ShowDialog();
        }
    }
}
