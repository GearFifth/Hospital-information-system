using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Repository;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.View;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Domain;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel
{
    public class HospitalTreatmentCheckInViewModel : ViewModelBase
    {
        

        private Patient? _selectedPatient;
        public Patient? SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        private TreatmentReferral? _selectedTreatmentReferral;
        public TreatmentReferral? SelectedTreatmentReferral
        {
            get => _selectedTreatmentReferral;
            set
            {
                _selectedTreatmentReferral = value;
                OnPropertyChanged(nameof(SelectedTreatmentReferral));
            }
        }

        private Room? _selectedRoom;

        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
            }
        }

        private string? _loggedUser;
        public string LoggedUser
        {
            get => _loggedUser;
            set
            {
                _loggedUser = value;
                OnPropertyChanged(nameof(LoggedUser));
            }
        }

        private ICommand? _checkInCommand { get; set; }

        public ICommand CheckInCommand
        {
            get
            {
                return _checkInCommand ??= new CheckInPatientForTreatmentCommand(this, _hospitalTreatmentCheckInView);
            }
        }

        private ICommand? _logoutCommand { get; set; }

        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??= new LogoutCommand(_hospitalTreatmentCheckInView);
            }
        }

        private readonly HospitalTreatmentCheckInView _hospitalTreatmentCheckInView;

        private ICommand? _patientSelectionChangedCommand { get; set; }

        public ICommand PatientSelectionChangedCommand
        {
            get
            {
                return _patientSelectionChangedCommand ??= new PatientSelectionChangedCommand(this);
            }
        }

        private ICommand? _treatmentReferralsSelectionChangedCommand { get; set; }

        public ICommand TreatmentReferralsSelectionChangedCommand
        {
            get
            {
                return _treatmentReferralsSelectionChangedCommand ??= new TreatmentReferralsSelectionChangedCommand(this);
            }
        }

        private ICommand? _treatmentReferralsChangedCommand { get; set; }

        public ICommand TreatmentReferralsChangedCommand
        {
            get
            {
                return _treatmentReferralsChangedCommand ??= new TreatmentReferralsSelectionChangedCommand(this);
            }
        }
        private ICommand? _backToNurseWindow { get; set; }

        public ICommand BackToNurseWindow
        {
            get
            {
                return _backToNurseWindow ??= new BackToNurseWindowCommand(_hospitalTreatmentCheckInView);
            }
        }

        private ObservableCollection<Room>? _rooms;

        public ObservableCollection<Room>? Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        private ObservableCollection<TreatmentReferral>? _treatmentReferrals;

        public ObservableCollection<TreatmentReferral>? TreatmentReferrals
        {
            get => _treatmentReferrals;
            set
            {
                _treatmentReferrals = value;
                OnPropertyChanged(nameof(TreatmentReferrals));
            }
        }

        private ObservableCollection<Patient>? _patients;

        public ObservableCollection<Patient>? Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }

        public HospitalTreatmentCheckInViewModel(HospitalTreatmentCheckInView hospitalTreatmentCheckInView)
        {
            //LoadRepositories();
            _hospitalTreatmentCheckInView = hospitalTreatmentCheckInView;
            LoggedUser = Globals.LoggedUser!.Username;
            LoadPatientsDataGrid();
        }

        //private static void LoadRepositories()
        //{
        //    _ = new HospitalTreatmentService(new HospitalTreatmentRepository());
        //}

        private void LoadPatientsDataGrid()
        {
            Patients = new ObservableCollection<Patient>();
            foreach (Patient patient in PatientService.GetAllPatients())
            {
                Patients.Add(patient);
            }
        }



    }
}
