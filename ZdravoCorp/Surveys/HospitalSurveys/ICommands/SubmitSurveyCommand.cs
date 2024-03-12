using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Surveys.HospitalSurveys.ICommands
{
    class SubmitSurveyCommand : CommandBase
    {
        private HospitalSurveyViewModel _hospitalSurveyViewModel;

        public SubmitSurveyCommand(HospitalSurveyViewModel hospitalSurveyViewModel)
        {
            _hospitalSurveyViewModel = hospitalSurveyViewModel;
        }

        public override void Execute(object? parameter)
        {
            HospitalSurvey hospitalSurvey = _hospitalSurveyViewModel.ParseSurveyFromView();
            HospitalSurveyService.Add(hospitalSurvey);
            Notification.ShowSuccessDialog("Succesfully submitted a survey!");
            _hospitalSurveyViewModel._hospitalSurveyView.Close();
        }
    }
}
