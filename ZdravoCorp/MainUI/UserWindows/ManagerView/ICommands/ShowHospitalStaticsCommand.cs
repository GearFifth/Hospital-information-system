using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Surveys.Analytics;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.ManagerView.ICommands
{
    public class ShowHospitalStaticsCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            var managerWindow = parameter as ManagerWindow;
            HospitalStatisticsWindow hospitalStatisticsWindow = new HospitalStatisticsWindow
            {
                Owner = managerWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            hospitalStatisticsWindow.ShowDialog();
        }
    }
}
