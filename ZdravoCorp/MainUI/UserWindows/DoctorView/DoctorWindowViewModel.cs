using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Healthcare.Communication;
using ZdravoCorp.MainUI.Users;
using ZdravoCorp.Vacations.VacationRequests;

namespace ZdravoCorp.MainUI.UserWindows.DoctorView
{
    public class DoctorWindowViewModel
    {
        public ObservableCollection<VacationRequest> VacationRequests { get; set; }
        public ObservableCollection<Message> SentMessages { get; set; }
        public ObservableCollection<Message> ReceivedMessages { get; set; }

        public DoctorWindow _doctorWindow;

        private ICommand _showVacationRequestDialogCommand;
        public ICommand ShowVacationRequestDialogCommand
        {
            get { return _showVacationRequestDialogCommand ??= new ShowVacationRequestDialogCommand(); }
        }

        private ICommand _sendMessageCommand;

        public ICommand SendMessageCommand
        {
            get { return _sendMessageCommand ??= new DoctorSendMessageCommand(this); }
        }

        public DoctorWindowViewModel(DoctorWindow doctorWindow)
        {
            _doctorWindow = doctorWindow;
            LoadRequests();
            LoadSentMessages();
            LoadReceivedMessages();
            LoadWorkersCombobox();
        }

        public void LoadRequests()
        {
            VacationRequests = VacationRequestService.GetAllDoctorVacationRequests(Globals.LoggedUser.Username);
        }

        public void LoadSentMessages()
        {
            SentMessages = MessageService.GetSentUserMessages(Globals.LoggedUser.Username);
        }

        public void LoadReceivedMessages()
        {
            ReceivedMessages = MessageService.GetReceivedUserMessages(Globals.LoggedUser.Username);
        }

        public void LoadWorkersCombobox()
        {
            List<string> doctors = UserService.GetUsernamesOfRole("Doctor");
            doctors.Remove(Globals.LoggedUser.Username);
            List<string> nurses = UserService.GetUsernamesOfRole("Nurse");

            foreach (string doctor in doctors)
            {
                _doctorWindow.workersComboBox.Items.Add(doctor);
            }
            foreach (string nurse in nurses)
            {
                _doctorWindow.workersComboBox.Items.Add(nurse);
            }
        }

        public void ReloadParentWindow()
        {
            DoctorWindow newDoctorWindow = new();
            newDoctorWindow.communicationTab.IsSelected = true;
            newDoctorWindow.Show();
            _doctorWindow.Close();
        }
    }
}
