using System.Collections.ObjectModel;
using System.Windows.Controls;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Service;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands
{
    public class PatientSelectionChangedCommand : CommandBase
    {
        
        private readonly HospitalTreatmentCheckInViewModel _hospitalTreatmentCheckInViewModel;

        public PatientSelectionChangedCommand(HospitalTreatmentCheckInViewModel hospitalTreatmentCheckInViewModel)
        {
            _hospitalTreatmentCheckInViewModel = hospitalTreatmentCheckInViewModel;
        }
        public override void Execute(object? parameter)
        {

            Patient patient = ((parameter as DataGrid)!.SelectedItem as Patient)!;
            _hospitalTreatmentCheckInViewModel.SelectedPatient = patient;
            _hospitalTreatmentCheckInViewModel.TreatmentReferrals = TreatmentReferralService.GetPatientsTreatmentReferrals(patient.Username);
            _hospitalTreatmentCheckInViewModel.Rooms = new ObservableCollection<Room>();


        }
    }
}
