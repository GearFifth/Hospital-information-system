using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.NurseVIew;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands
{
    internal class BackToNurseWindowCommand : CommandBase
    {
        private readonly Window _windowToBackFrom;
        public BackToNurseWindowCommand(Window hospitalTreatmentVisitViewModel)
        {
            _windowToBackFrom = hospitalTreatmentVisitViewModel;
        }

        public override void Execute(object? parameter)
        {
            NurseWindow nurseWindow = new();
            nurseWindow.Show();
            _windowToBackFrom.Close();
        }
    }
}
