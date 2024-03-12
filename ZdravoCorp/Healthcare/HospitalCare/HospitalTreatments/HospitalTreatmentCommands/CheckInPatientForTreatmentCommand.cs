using System;
using System.ComponentModel;
using System.Windows.Navigation;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.View;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Service;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands
{
    internal class CheckInPatientForTreatmentCommand : CommandBase
    {
        private readonly HospitalTreatmentCheckInViewModel _hospitalTreatmentCheckInViewModel;
        private HospitalTreatmentCheckInView _hospitalTreatmentCheckInView;

        public CheckInPatientForTreatmentCommand(HospitalTreatmentCheckInViewModel hospitalTreatmentCheckInViewModel,
            HospitalTreatmentCheckInView hospitalTreatmentCheckInView)
        {
            _hospitalTreatmentCheckInViewModel = hospitalTreatmentCheckInViewModel;
            _hospitalTreatmentCheckInView = hospitalTreatmentCheckInView;
            _hospitalTreatmentCheckInViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HospitalTreatmentCheckInViewModel.SelectedRoom))
            {
                OnCanExecutedChanged();
            }
        }

        public override void Execute(object? parameter)
        {

            int daysOnTherapy = _hospitalTreatmentCheckInViewModel.SelectedTreatmentReferral!.NumOfDays;
            string therapy = _hospitalTreatmentCheckInViewModel.SelectedTreatmentReferral.Therapy;
            string patientsUsername = _hospitalTreatmentCheckInViewModel.SelectedPatient!.Username;
            string roomName = _hospitalTreatmentCheckInViewModel.SelectedRoom!.Name;

            HospitalTreatmentService.Add(DateTime.Now, DateTime.Now.AddDays(daysOnTherapy), therapy, patientsUsername, roomName);

            MakeReferralUsed();
            RefreshWindow(patientsUsername, roomName);
            
            
            
        }

        private void MakeReferralUsed()
        {
            _hospitalTreatmentCheckInViewModel.SelectedTreatmentReferral!.IsUsed = true;
            TreatmentReferralService.Save();
        }

        private void RefreshWindow(string patientsUsername, string roomName)
        {
            HospitalTreatmentCheckInView hospitalTreatmentCheckInView = new();
            hospitalTreatmentCheckInView.Show();
            _hospitalTreatmentCheckInView.Close();
            Notification.ShowSuccessDialog("Successfully checked patient" + patientsUsername + " in " + roomName);
        }

        public override bool CanExecute(object? parameter)
        {
            return _hospitalTreatmentCheckInViewModel.SelectedRoom != null && !IsSelectedPatientAlreadyOnTherapy() && base.CanExecute(parameter);
        }

        private bool IsSelectedPatientAlreadyOnTherapy()
        {
            TreatmentReferral selectedTreatment = _hospitalTreatmentCheckInViewModel.SelectedTreatmentReferral!;
            if (HospitalTreatmentService.IsPatientAlreadyOnTherapy(selectedTreatment)) return true;
            return false;
        }
    }
}
