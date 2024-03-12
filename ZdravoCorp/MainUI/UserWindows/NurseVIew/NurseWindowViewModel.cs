using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Healthcare.Communication;
using ZdravoCorp.MainUI.Users;

namespace ZdravoCorp.MainUI.UserWindows.NurseVIew
{
    public class NurseWindowViewModel
    {
        public ObservableCollection<Message> SentMessages { get; set; }
        public ObservableCollection<Message> ReceivedMessages { get; set; }

        public NurseWindow _nurseWindow;

        private ICommand _sendMessageCommand;

        public ICommand SendMessageCommand
        {
            get { return _sendMessageCommand ??= new NurseSendMessageCommand(this); }
        }

        public NurseWindowViewModel(NurseWindow nurseWindow)
        {
            _nurseWindow = nurseWindow;
            LoadSentMessages();
            LoadReceivedMessages();
            LoadWorkersCombobox();
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
            List<string> nurses = UserService.GetUsernamesOfRole("Nurse");
            nurses.Remove(Globals.LoggedUser.Username);

            foreach (string doctor in doctors)
            {
                _nurseWindow.workersComboBox.Items.Add(doctor);
            }
            foreach (string nurse in nurses)
            {
                _nurseWindow.workersComboBox.Items.Add(nurse);
            }
        }

        public void ReloadParentWindow()
        {
            NurseWindow newNurseWindow = new();
            newNurseWindow.communicationTab.IsSelected = true;
            newNurseWindow.Show();
            _nurseWindow.Close();
        }
    }
}
