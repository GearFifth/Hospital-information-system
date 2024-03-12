using System;
using System.Windows.Input;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.DoctorView;
using ZdravoCorp.Vacations.VacationRequests.ICommands;
using ZdravoCorp.Vacations.VacationRequests.View;

namespace ZdravoCorp.Vacations.VacationRequests.ViewModel
{
    public class VacationRequestDialogViewModel
    {
        public VacationRequestDialog VacationRequestDialog { get; set; }
        public DoctorWindow DoctorWindow { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        private ICommand _createVacationRequestCommand;
        public ICommand CreateVacationRequestCommand
        {
            get { return _createVacationRequestCommand ??= new CreateVacationRequestCommand(this); }
        }

        public VacationRequestDialogViewModel(VacationRequestDialog vacationRequestDialog, DoctorWindow doctorWindow)
        {
            VacationRequestDialog = vacationRequestDialog;
            DoctorWindow = doctorWindow;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public void ReloadParentWindow()
        {
            DoctorWindow newDoctorWindow = new();
            newDoctorWindow.vacationTab.IsSelected = true;
            newDoctorWindow.Show();
            DoctorWindow.Close();
        }
    }
}
