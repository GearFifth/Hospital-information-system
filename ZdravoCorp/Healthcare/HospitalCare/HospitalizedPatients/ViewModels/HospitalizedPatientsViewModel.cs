using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ICommands;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels
{
    public class HospitalizedPatientsViewModel
    {
        public ObservableCollection<HospitalTreatment> HospitalTreatments { get; set; }

        public HospitalTreatment? SelectedTreatment { get; set; }
        private ICommand _showHandleTreatmentWindowCommand;
        public ICommand ShowHandleTreatmentWindowCommand
        {
            get { return _showHandleTreatmentWindowCommand ??= new ShowHandleTreatmentWindowCommand(this); }
        }

        public HospitalizedPatientsViewModel()
        {
            LoadTreatments();
        }

        public void LoadTreatments()
        {
            HospitalTreatments = new ObservableCollection<HospitalTreatment>(HospitalTreatmentService.GetAllHospitalTreatments());
        }
    }
}
