using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands
{
    internal class ReportVisitCommand : CommandBase
    {
        private readonly HospitalTreatmentVisitViewModel _hospitalTreatmentVisitViewModel;
        public ReportVisitCommand(HospitalTreatmentVisitViewModel hospitalTreatmentVisitViewModel)
        {
            _hospitalTreatmentVisitViewModel = hospitalTreatmentVisitViewModel;
            _hospitalTreatmentVisitViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(HospitalTreatmentVisitViewModel.BodyTemperature):
                case nameof(HospitalTreatmentVisitViewModel.BloodPressure):
                case nameof(HospitalTreatmentVisitViewModel.SelectedHospitalTreatment):
                    OnCanExecutedChanged();
                    break;
            }
        }

        public override void Execute(object? parameter)
        {
            var patientUsername = _hospitalTreatmentVisitViewModel.SelectedHospitalTreatment!.PatientUsername;
            var hospitalTreatmentId = _hospitalTreatmentVisitViewModel.SelectedHospitalTreatment!.Id;
            int bodyTemperature = int.Parse(_hospitalTreatmentVisitViewModel.BodyTemperature!);
            int bloodPressure = int.Parse(_hospitalTreatmentVisitViewModel.BloodPressure!);
            string symptoms = _hospitalTreatmentVisitViewModel.Symptoms!;


            RecordVisit(patientUsername, hospitalTreatmentId, bloodPressure, bodyTemperature, symptoms);
        }

        private static void RecordVisit(string patientUsername, int hospitalTreatmentId, int bloodPressure, int bodyTemperature, string symptoms)
        {

            int numberOfVisitsToday = HospitalTreatmentVisitService.GetNumberOfVisitsForPatientToday(patientUsername, hospitalTreatmentId);
            if (CheckNumberOfVisitsToday(numberOfVisitsToday)) return;

            HospitalTreatmentVisitService.Add(bloodPressure, bodyTemperature, symptoms, hospitalTreatmentId, DateTime.Now, patientUsername);
            Notification.ShowSuccessDialog("Visit successfully recorded!");
        }

        private static bool CheckNumberOfVisitsToday(int numberOfVisitsToday)
        {
            if (numberOfVisitsToday < 2) return false;

            Notification.ShowErrorDialog("Already done two visits on this treatment today!");
            return true;

        }

        public override bool CanExecute(object? parameter)
        {
            return ValidateFields() && base.CanExecute(parameter);
        }

        private bool ValidateFields()
        {
            string regexPattern = @"^[0-9]+$";
            Regex regex = new Regex(regexPattern);
            if (!ValidateBodyTemperature(regex)) return false;
            if (!ValidateBloodPressure(regex)) return false;
            return _hospitalTreatmentVisitViewModel.SelectedHospitalTreatment != null;
        }

        private bool ValidateBodyTemperature(Regex regex)
        {
            var bodyTemperature = _hospitalTreatmentVisitViewModel.BodyTemperature;
            return bodyTemperature != null && regex.IsMatch(bodyTemperature);
        }

        private bool ValidateBloodPressure(Regex regex)
        {
            var bloodPressure = _hospitalTreatmentVisitViewModel.BloodPressure;
            return bloodPressure != null && regex.IsMatch(bloodPressure);
        }
    }
}
