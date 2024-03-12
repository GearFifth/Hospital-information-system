using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ZdravoCorp.MainUI;
using ZdravoCorp.Surveys.DoctorSurveys.ICommands;

namespace ZdravoCorp.Surveys.DoctorSurveys
{
    public class DoctorSurveyViewModel
    {
        public readonly DoctorSurveyView _doctorSurveyView;
        private int _appointmentId;
        private string _doctorUsername;
        public ICommand _SubmitSurveyCommand { get; set; }
        public ICommand SubmitSurveyCommand
        {
            get { return _SubmitSurveyCommand ??= new SubmitSurveyCommand(this, _doctorUsername); }
        }

        public DoctorSurveyViewModel(DoctorSurveyView doctorSurveyView, int appointmentId, string doctorUsername)
        {
            _doctorSurveyView = doctorSurveyView;
            _appointmentId = appointmentId;
            _doctorUsername = doctorUsername;
        }

        public DoctorSurvey ParseSurveyFromView()
        {
            int qualityOfService = int.Parse(_doctorSurveyView.qualityOfServiceComboBox.Text);
            int overallSatisfaction = int.Parse(_doctorSurveyView.OverallSatisfactionComboBox.Text);
            int recommendation = int.Parse(_doctorSurveyView.RecommendationComboBox.Text);
            string comment = _doctorSurveyView.commentTextBox.Text;

            return new DoctorSurvey(_appointmentId, Globals.LoggedUser.Username, qualityOfService, overallSatisfaction,
                recommendation, comment);
        }
    }
}
