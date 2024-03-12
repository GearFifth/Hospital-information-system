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
    public class ShowDoctorStatisticsCommand : CommandBase
    {
        private readonly ManagerViewModel _managerViewModel;
        public ShowDoctorStatisticsCommand(ManagerViewModel managerViewModel)
        {
            _managerViewModel = managerViewModel;
        }

        public override void Execute(object? parameter)
        {
            var managerWindow = parameter as ManagerWindow;
            DoctorStatisticsWindow doctorStatisticsWindow = new DoctorStatisticsWindow(_managerViewModel.SelectedDoctor.Username)
            {
                Owner = managerWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            doctorStatisticsWindow.ShowDialog();
        }
    }
}
