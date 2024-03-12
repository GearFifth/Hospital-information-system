using System.Windows;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.View;
using ZdravoCorp.MainUI;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands
{
    public class LogoutCommand : CommandBase
    {
        private readonly Window _window;
        public LogoutCommand(Window hospitalTreatmentCheckInView)
        {
            _window = hospitalTreatmentCheckInView;
        }

        public override void Execute(object? parameter)
        {
            
            MainWindow mainWindow = new();
            mainWindow.Show();

            Globals.LoggedUser = null;
            _window.Close();
            
            }
    }
}
