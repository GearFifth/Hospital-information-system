using System.ComponentModel;
using System.Windows.Controls;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands
{
    internal class TreatmentReferralsSelectionChangedCommand : CommandBase
    {
        private readonly HospitalTreatmentCheckInViewModel _hospitalTreatmentCheckInViewModel;
        public TreatmentReferralsSelectionChangedCommand(HospitalTreatmentCheckInViewModel hospitalTreatmentCheckInViewModel)
        {
            _hospitalTreatmentCheckInViewModel = hospitalTreatmentCheckInViewModel;
            _hospitalTreatmentCheckInViewModel.PropertyChanged += OnViewModelPropertyChanged;
            
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HospitalTreatmentCheckInViewModel.SelectedTreatmentReferral))
            {
                OnCanExecutedChanged();
            }
        }

        public override void Execute(object? parameter)
        {
            TreatmentReferral treatmentReferral = ((parameter as DataGrid)!.SelectedItem as TreatmentReferral)!;
            _hospitalTreatmentCheckInViewModel.SelectedTreatmentReferral = treatmentReferral;
            _hospitalTreatmentCheckInViewModel.Rooms = RoomService.GetAllAvailablePatientRooms(treatmentReferral.NumOfDays);
            _hospitalTreatmentCheckInViewModel.SelectedRoom = null;
        }

        public override bool CanExecute(object? parameter)
        {
            return _hospitalTreatmentCheckInViewModel.SelectedTreatmentReferral != null && base.CanExecute(parameter);
        }
    }
}
