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
    public class ShowBestDoctorsCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            var managerWindow = parameter as ManagerWindow;
            BestDoctorsWindow bestDoctorsWindow = new BestDoctorsWindow
            {
                Owner = managerWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            bestDoctorsWindow.ShowDialog();
        }
    }
}
