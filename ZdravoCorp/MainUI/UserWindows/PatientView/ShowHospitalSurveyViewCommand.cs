using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Surveys.HospitalSurveys;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.PatientView
{
    class ShowHospitalSurveyViewCommand : CommandBase
    {

        public ShowHospitalSurveyViewCommand()
        {

        }

        public override bool CanExecute(object? Parameter)
        {
            return !HospitalSurveyService.HasPatientAlreadySubmitted(Globals.LoggedUser.Username);
        }
        public override void Execute(object? Parameter)
        {
            HospitalSurveyView hospitalSurveyView = new HospitalSurveyView();
            hospitalSurveyView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            hospitalSurveyView.ShowDialog();
        }
    }
}
