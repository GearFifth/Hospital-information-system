using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.Communication;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.NurseVIew
{
    public class NurseSendMessageCommand : CommandBase
    {
        private NurseWindowViewModel _nurseWindowViewModel;

        public NurseSendMessageCommand(NurseWindowViewModel nurseWindowViewModel)
        {
            _nurseWindowViewModel = nurseWindowViewModel;
        }

        public override void Execute(object? Parameter)
        {
            if (_nurseWindowViewModel._nurseWindow.workersComboBox.Text != "" &&
                _nurseWindowViewModel._nurseWindow.contentTextBox.Text != "")
            {
                string content = _nurseWindowViewModel._nurseWindow.contentTextBox.Text;
                string receiver = _nurseWindowViewModel._nurseWindow.workersComboBox.Text;

                Message message = new Message(DateTime.Now, content, Globals.LoggedUser.Username, receiver);
                MessageService.Add(message);
                _nurseWindowViewModel.ReloadParentWindow();
            }
            else
            {
                Notification.ShowErrorDialog("You can't send empty messages!");
            }
        }
    }
}
