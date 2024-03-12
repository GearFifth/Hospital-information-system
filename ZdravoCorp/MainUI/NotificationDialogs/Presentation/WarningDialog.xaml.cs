using System.Windows;

namespace ZdravoCorp.MainUI.NotificationDialogs.Presentation
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class WarningDialog : Window
    {
        public WarningDialog(string message)
        {
            InitializeComponent();
            messageTextBlock.Text = message;
        }

        private void okayBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
