using System.Windows;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.Vacations.VacationRequests;

namespace ZdravoCorp.Vacations.VacationManagment
{
    /// <summary>
    /// Interaction logic for ApproveVacationDialog.xaml
    /// </summary>
    public partial class ApproveVacationDialog : Window
    {
        public ApproveVacationDialog(VacationRequest vacationRequest,ManagerWindow managerWindow)
        {
            InitializeComponent();
            ApproveVacationViewModel viewModel = new ApproveVacationViewModel(vacationRequest,this,managerWindow);
            DataContext = viewModel;
        }
    }
}
