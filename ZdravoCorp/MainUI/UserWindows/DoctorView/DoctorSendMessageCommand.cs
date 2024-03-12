using System;
using ZdravoCorp.Healthcare.Communication;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.MainUI.UserWindows.DoctorView
{
    public class DoctorSendMessageCommand : CommandBase
    {
        private DoctorWindowViewModel _doctorWindowViewModel;

        public DoctorSendMessageCommand(DoctorWindowViewModel doctorWindowViewModel)
        {
            _doctorWindowViewModel = doctorWindowViewModel;
        }
        public override void Execute(object? parameter)
        {
            if (_doctorWindowViewModel._doctorWindow.workersComboBox.Text != "" &&
                _doctorWindowViewModel._doctorWindow.contentTextBox.Text != "")
            {
                string content = _doctorWindowViewModel._doctorWindow.contentTextBox.Text;
                string receiver = _doctorWindowViewModel._doctorWindow.workersComboBox.Text;

                Message message = new Message(DateTime.Now, content, Globals.LoggedUser.Username, receiver);
                MessageService.Add(message);
                _doctorWindowViewModel.ReloadParentWindow();
            }
            else
            {
                Notification.ShowErrorDialog("You can't send empty messages!");
            }
        }
    }
}
