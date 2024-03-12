using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.MainUI.UserWindows.ManagerView.ICommands;
using ZdravoCorp.Surveys.Analytics;
using ZdravoCorp.Surveys.Analytics.DoctorRankings;
using ZdravoCorp.Surveys.DoctorSurveys;
using ZdravoCorp.Surveys.HospitalSurveys;
using ZdravoCorp.Utils;
using ZdravoCorp.Utils.Commands;
using ZdravoCorp.Vacations.VacationManagment;
using ZdravoCorp.Vacations.VacationRequests;

namespace ZdravoCorp.MainUI.UserWindows.ManagerView
{
    public class ManagerViewModel : ViewModelBase
    {
        public ObservableCollection<VacationRequest> VacationRequests { get; set; }

        public VacationRequest SelectedVacation { get; set; }

        public bool HasSelectedVacation
        {
            get => _hasSelectedVacation;
            set
            {
                _hasSelectedVacation = value;
                OnPropertyChanged(nameof(HasSelectedVacation));
            }
        }

        private bool _hasSelectedVacation;

        public ObservableCollection<HospitalSurvey> HospitalSurveys { get; set; }

        public ObservableCollection<DoctorSurvey> DoctorsSurveys { get; set; }

        public ObservableCollection<Doctor> Doctors
        {
            get; set;
        }

        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }

        private Doctor _selectedDoctor;

        public ManagerWindow ManagerWindow { get; set; }

        public ICommand ShowApproveVacationDialogCommand
        {
            get
            {
                return _showApproveVacationDialogCommand ??= new ShowApproveVacationDialogCommand(this);
            }
        }

        private ICommand _showApproveVacationDialogCommand;

        public ICommand EnableApproveVacationButtonCommand
        {
            get
            {
                return _enableApproveVacationButtonCommand ??= new EnableApproveVacationButtonCommand(this);
            }
        }

        private ICommand _enableApproveVacationButtonCommand;

        public ICommand SelectedDoctorChangedCommand
        {
            get
            {
                return _selectedDoctorChangedCommand ??= new SelectedDoctorChangedCommand(this);
            }
        }

        private ICommand _selectedDoctorChangedCommand;

        public ICommand ShowHospitalStaticsCommand
        {
            get
            {
                return _showHospitalStatisticsCommand ??= new ShowHospitalStaticsCommand();
            }
        }

        private ICommand _showHospitalStatisticsCommand;

        public ICommand ShowDoctorStatisticsCommand
        {
            get
            {
                return _showDoctorStatisticsCommand ??= new ShowDoctorStatisticsCommand(this);
            }
        }

        private ICommand _showDoctorStatisticsCommand;

        public ICommand ShowBestDoctorsCommand
        {
            get
            {
                return _showBestDoctorsCommand ??= new ShowBestDoctorsCommand();
            }
        }

        private ICommand _showBestDoctorsCommand;

        public ICommand ShowWorstDoctorsCommand
        {
            get
            {
                return _showWorstDoctorsCommand ??= new ShowWorstDoctorsCommand();
            }
        }

        private ICommand _showWorstDoctorsCommand;

        public ManagerViewModel()
        {
            VacationRequests = VacationRequestService.GetAll();
            HospitalSurveys = HospitalSurveyService.GetAll();
            Doctors = DoctorService.GetAllDoctorsObservableCollection();
            SelectedDoctor = Doctors[0];
            DoctorsSurveys =
                new ObservableCollection<DoctorSurvey>(DoctorSurveyService.GetAllSurveysForDoctor(SelectedDoctor.Username));
        }

    }
}
