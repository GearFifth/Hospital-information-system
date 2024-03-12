using System.Windows;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.DoctorView;
using ZdravoCorp.Vacations.VacationRequests.ViewModel;

namespace ZdravoCorp.Vacations.VacationRequests.View
{
    /// <summary>
    /// Interaction logic for Request.xaml
    /// </summary>
    public partial class VacationRequestDialog : Window
    {

        public VacationRequestDialog(DoctorWindow? doctorWindow)
        {
            InitializeComponent();
            VacationRequestDialogViewModel vacationRequestDialogViewModel = new(this, doctorWindow);
            this.DataContext = vacationRequestDialogViewModel;
        }
    }
}
