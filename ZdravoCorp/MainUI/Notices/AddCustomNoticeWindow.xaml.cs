using System;
using System.Windows;
using ZdravoCorp.MainUI.NotificationDialogs;

namespace ZdravoCorp.MainUI.Notices
{
    /// <summary>
    /// Interaction logic for AddCustomNoticeWindow.xaml
    /// </summary>
    public partial class AddCustomNoticeWindow : Window
    {
        public AddCustomNoticeWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        { 
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Notice notice = ParseNoticeFromDialog();
                NoticeService.Add(notice);
                Notification.ShowSuccessDialog("Succesfully added a notice!");
                Close();
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private DateTime ParseDateTimeFromDialog()
        {
            DateTime dateTime = datePicker.SelectedDate.Value.Date;
            TimeOnly timeOnly = TimeOnly.Parse(timePickerTextBox.Text);
            dateTime = dateTime.AddHours(timeOnly.Hour).AddMinutes(timeOnly.Minute);
            if (dateTime < DateTime.Now)
            {
                throw new ArgumentException("Date must be in the future.");
            }

            return dateTime;
        }

        private Notice ParseNoticeFromDialog()
        {
            DateTime dateTime = ParseDateTimeFromDialog();
            return new Notice(dateTime, contentTextBox.Text, Globals.LoggedUser.Username);
        }
    }
}
