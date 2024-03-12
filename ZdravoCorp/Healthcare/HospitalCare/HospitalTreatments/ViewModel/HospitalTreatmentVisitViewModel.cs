using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.View;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.MainUI;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel
{
    internal class HospitalTreatmentVisitViewModel : ViewModelBase
    {
        private string? _loggedUser;
        public string? LoggedUser
        {
            get => _loggedUser;
            set
            {
                _loggedUser = value;
                OnPropertyChanged(nameof(LoggedUser));
            }
        }

        private HospitalTreatment? _selectedHospitalTreatment;

        public HospitalTreatment? SelectedHospitalTreatment
        {
            get => _selectedHospitalTreatment;
            set
            {
                _selectedHospitalTreatment = value;
                OnPropertyChanged(nameof(SelectedHospitalTreatment));
            }
        }
        private string? _bodyTemperature;
        public string? BodyTemperature
        {
            get => _bodyTemperature;
            set
            {
                _bodyTemperature = value;
                OnPropertyChanged(nameof(BodyTemperature));
            }
        }

        private string? _bloodPressure;
        public string? BloodPressure
        {
            get => _bloodPressure;
            set
            {
                _bloodPressure = value;
                OnPropertyChanged(nameof(BloodPressure));
            }
        }

        private string? _symptoms;
        public string? Symptoms
        {
            get => _symptoms;
            set
            {
                _symptoms = value;
                OnPropertyChanged(nameof(Symptoms));
            }
        }

        private ICommand? _logoutCommand { get; set; }

        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??= new LogoutCommand(_hospitalTreatmentVisitView);
            }
        }

        private ICommand? _filterHospitalTreatmentsCommand { get; set; }

        public ICommand FilterHospitalTreatmentsCommand
        {
            get
            {
                return _filterHospitalTreatmentsCommand ??= new FilterHospitalTreatmentCommand(this);
            }
        }

        private ICommand? _reportVisitCommand { get; set; }

        public ICommand ReportVisitCommand
        {
            get
            {
                return _reportVisitCommand ??= new ReportVisitCommand(this);
            }
        }

        private ICommand? _backToNurseWindow { get; set; }

        public ICommand BackToNurseWindow
        {
            get
            {
                return _backToNurseWindow ??= new BackToNurseWindowCommand(_hospitalTreatmentVisitView);
            }
        }

        private ObservableCollection<HospitalTreatment>? _hospitalTreatments;

        public ObservableCollection<HospitalTreatment>? HospitalTreatments
        {
            get => _hospitalTreatments;
            set
            {
                _hospitalTreatments = value;
                OnPropertyChanged(nameof(HospitalTreatments));
            }
        }
        private readonly HospitalTreatmentVisitView _hospitalTreatmentVisitView;
        public HospitalTreatmentVisitViewModel(HospitalTreatmentVisitView hospitalTreatmentVisitView)
        {
            _hospitalTreatmentVisitView = hospitalTreatmentVisitView;
            LoggedUser = Globals.LoggedUser!.Username;
            LoadHospitalTreatmentsDataGrid();
        }

        private void LoadHospitalTreatmentsDataGrid()
        {
            HospitalTreatments = new ObservableCollection<HospitalTreatment>();
            foreach (HospitalTreatment hospitalTreatment in HospitalTreatmentService.GetAllHospitalTreatments())
            {
                if (!hospitalTreatment.IsActive()) continue;
                HospitalTreatments.Add(hospitalTreatment);
            }
        }
    }
}
