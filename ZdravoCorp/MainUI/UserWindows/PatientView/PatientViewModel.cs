using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Surveys.HospitalSurveys;

namespace ZdravoCorp.MainUI.UserWindows.PatientView
{
    public class PatientViewModel
    {
        public ObservableCollection<Appointment> Appointments { get; set; }

        private ICommand _showHospitalSurveyViewCommand;
        public ICommand ShowHospitalSurveyViewCommand
        {
            get { return _showHospitalSurveyViewCommand ??= new ShowHospitalSurveyViewCommand(); }
        }

        public ICommand _showDoctorSurveyViewCommand;
        public ICommand ShowDoctorSurveyViewCommand
        {
            get { return _showDoctorSurveyViewCommand ??= new ShowDoctorSurveyViewCommand(); }
        }

        public PatientViewModel()
        {
            /*ShowHospitalSurveyViewCommand = new RelayCommand(ShowHospitalSurveyView, CanShowHospitalSurveyView);*/
            /*ShowDoctorSurveyViewCommand = new RelayCommand(ShowDoctorSurveyView, CanShowDoctorSurveyView);*/
            LoadAppointments();
        }

        public void LoadAppointments()
        {
            Appointments = new ObservableCollection<Appointment>(AppointmentService.GetAllAppointmentsForPatient(Globals.LoggedUser.Username));
        }
    }
}
