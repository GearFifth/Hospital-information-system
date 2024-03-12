using System;
using System.Windows.Input;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ICommands;
using ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.Views;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalizedPatients.ViewModels
{
    public class HandleTreatmentViewModel
    {
        public HospitalizedPatientsWindow HospitalizedPatientsWindow;
        public HandleTreatmentWindow HandleTreatmentWindow;
        public HospitalTreatment HospitalTreatment;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PatientUsername { get; set; }
        public string RoomName { get; set; }
        public string Treatment { get; set; }
        public string DaysToExtend { get; set; }

        public bool SelectedAppointCheckUp { get; set; }

        private ICommand _extendTreatmentCommand;
        public ICommand ExtendTreatmentCommand
        {
            get { return _extendTreatmentCommand ??= new ExtendTreatmentCommand(this); }
        }

        private ICommand _finishTreatmentCommand;
        public ICommand FinishTreatmentCommand
        {
            get { return _finishTreatmentCommand ??= new FinishTreatmentCommand(this); }
        }

        private ICommand _editTreatmentCommand;

        public ICommand EditTreatmentCommand
        {
            get { return _editTreatmentCommand ??= new EditTreatmentCommand(this); }
        }
        public HandleTreatmentViewModel(HospitalTreatment hospitalTreatment,
            HandleTreatmentWindow handleTreatmentWindow, HospitalizedPatientsWindow? hospitalizedPatientsWindow)
        {
            HospitalTreatment = hospitalTreatment;
            HandleTreatmentWindow = handleTreatmentWindow;
            HospitalizedPatientsWindow = hospitalizedPatientsWindow;

            fillWindow();
        }

        private void fillWindow()
        {
            StartDate = HospitalTreatment.TreatmentBeginning;
            EndDate = HospitalTreatment.TreatmentEnding;
            PatientUsername = HospitalTreatment.PatientUsername;
            RoomName = HospitalTreatment.RoomName;
            Treatment = HospitalTreatment.Treatment;
            DaysToExtend = "";
            SelectedAppointCheckUp = false;
        }

        public int parseDaysToExtend()
        {
            try
            {
                return int.Parse(DaysToExtend);
            }
            catch
            {
                throw new ArgumentException("The entered days for extension are incorrect");
            }
        }

        public void EditTreatment()
        {
            if (Treatment == "")
            {
                throw new InvalidOperationException("Treatment can't be empty.");
            }

            HospitalTreatment.Treatment = Treatment;
            HospitalTreatmentService.Update(HospitalTreatment);
        }

        public void ReloadHospitalizedPatientsWindow()
        {
            HospitalizedPatientsWindow newHospitalizedPatientsWindow = new();
            newHospitalizedPatientsWindow.ShowDialog();
            HospitalizedPatientsWindow.Close();
        }
    }
}
