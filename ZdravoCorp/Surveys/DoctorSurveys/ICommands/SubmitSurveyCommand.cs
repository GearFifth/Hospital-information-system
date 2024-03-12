using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Surveys.DoctorSurveys.ICommands
{
    class SubmitSurveyCommand : CommandBase
    {
        private DoctorSurveyViewModel _doctorSurveyViewModel;
        private string _doctorUsername;

        public SubmitSurveyCommand(DoctorSurveyViewModel doctorSurveyViewModel, string doctorUsername)
        {
            _doctorSurveyViewModel = doctorSurveyViewModel;
            _doctorUsername = doctorUsername;
        }

        public override void Execute(object? parameter)
        {
            DoctorSurvey doctorSurvey = _doctorSurveyViewModel.ParseSurveyFromView();
            DoctorSurveyService.Add(_doctorUsername, doctorSurvey);
            Notification.ShowSuccessDialog("Succesfully submitted a survey!");
            _doctorSurveyViewModel._doctorSurveyView.Close();
        }
    }
}
