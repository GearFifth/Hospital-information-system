using System.Windows;

namespace ZdravoCorp.MainUI.NotificationDialogs.Presentation
{
    /// <summary>
    /// Interaction logic for SuccessfulDialog.xaml
    /// </summary>
    public partial class SuccessDialog : Window
    {
        public SuccessDialog(string message)
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
