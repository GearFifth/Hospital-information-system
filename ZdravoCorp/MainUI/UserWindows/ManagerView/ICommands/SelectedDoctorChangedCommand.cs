using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Surveys.DoctorSurveys;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.ManagerView.ICommands
{
    public class SelectedDoctorChangedCommand : CommandBase
    {
        private readonly ManagerViewModel _managerViewModel;
        public SelectedDoctorChangedCommand(ManagerViewModel managerViewModel)
        {
            _managerViewModel = managerViewModel;
        }

        public override void Execute(object? parameter)
        {
            _managerViewModel.DoctorsSurveys.Clear();
            foreach (var doctorSurvey in DoctorSurveyService.GetAllSurveysForDoctor(_managerViewModel.SelectedDoctor.Username))
            {
                _managerViewModel.DoctorsSurveys.Add(doctorSurvey);
            }
        }
    }
}
