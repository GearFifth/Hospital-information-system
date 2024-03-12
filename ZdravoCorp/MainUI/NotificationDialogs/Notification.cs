using ZdravoCorp.MainUI.NotificationDialogs.Presentation;

namespace ZdravoCorp.MainUI.NotificationDialogs
{
    public static class Notification
    {
        public static void ShowErrorDialog(string message)
        {
            ErrorDialog errorDialog = new(message);
            errorDialog.ShowDialog();
        }

        public static void ShowSuccessDialog(string message)
        {
            SuccessDialog successDialog = new(message);
            successDialog.ShowDialog();
        }

        public static void ShowWarningDialog(string message)
        {
            WarningDialog warningDialog = new(message);
            warningDialog.ShowDialog();
        }
    }
}
