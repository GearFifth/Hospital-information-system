using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.Surveys.HospitalSurveys.ICommands;

namespace ZdravoCorp.Surveys.HospitalSurveys
{
    public class HospitalSurveyViewModel
    {

        public readonly HospitalSurveyView _hospitalSurveyView;
        public ICommand _SubmitSurveyCommand { get; set; }
        public ICommand SubmitSurveyCommand
        {
            get { return _SubmitSurveyCommand ??= new SubmitSurveyCommand(this); }
        }

        public HospitalSurveyViewModel(HospitalSurveyView hospitalSurveyView)
        {
            _hospitalSurveyView = hospitalSurveyView;
        }

        public HospitalSurvey ParseSurveyFromView()
        {
            int qualityOfService = int.Parse(_hospitalSurveyView.qualityOfServiceComboBox.Text);
            int cleannes = int.Parse(_hospitalSurveyView.CleannesComboBox.Text);
            int overallSatisfaction = int.Parse(_hospitalSurveyView.OverallSatisfactionComboBox.Text);
            int recommendation = int.Parse(_hospitalSurveyView.RecommendationComboBox.Text);
            string comment = _hospitalSurveyView.commentTextBox.Text;

            return new HospitalSurvey(Globals.LoggedUser.Username, qualityOfService, cleannes, overallSatisfaction,
                recommendation, comment);
        }
    }
}
