using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentScheduler;
using PatientWindow = ZdravoCorp.MainUI.UserWindows.PatientView.PatientWindow;
using ZdravoCorp.Healthcare.HospitalCare.Referrals.Repository;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;
using ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords;
using ZdravoCorp.Healthcare.PatientHealthcare.ReportHistories;
using ZdravoCorp.Healthcare.Pharmacy.Drugs;
using ZdravoCorp.Healthcare.Pharmacy.Orders;
using ZdravoCorp.Healthcare.Roles.Doctor;
using ZdravoCorp.PhysicalAsset.Inventory.Repository;
using ZdravoCorp.PhysicalAsset.Inventory.Service.InventoryTimer;
using ZdravoCorp.PhysicalAsset.Orders.Repository;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.MainUI.Users;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.Notices;
using ZdravoCorp.MainUI.UserWindows.DoctorView;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.MainUI.UserWindows.NurseVIew;
using ZdravoCorp.PhysicalAsset.Inventory.Service;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            User user = UserService.GetLoginUser(this.usernameTextbox.Text, this.passwordTextbox.Password);
            if (user == null)
            {
                errorLabel.Visibility = Visibility.Visible;
                return;
            }

            Globals.LoggedUser = user;
            try
            {
                openRoleWindow(user);
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
                
                return;
            }
            this.Close();
        }

        private void openRoleWindow(User user)
        {
            switch (user.Role)
            {
                case User.UserRole.Doctor:
                    DoctorWindow dw = new DoctorWindow();
                    dw.Show();
                    break;
                case User.UserRole.Nurse:
                    NurseWindow nw = new NurseWindow();
                    nw.Show();
                    break;
                case User.UserRole.Patient:
                    if (PatientService.IsPatientBlocked(user.Username))
                    {
                        throw new InvalidOperationException("This account has been permanently disabled.");
                    }
                    else
                    {
                        PatientWindow pd = new PatientWindow();
                        pd.Show();
                    }
                    break;
                case User.UserRole.Manager:
                    ManagerWindow wm = new ManagerWindow();
                    wm.Show();
                    break;
            }
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            JobManager.Initialize(new InventoryTimerRegistry());
            RenovationSchedule.CheckRenovations();
        }


        private void loginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                loginBtn_Click(sender,e);
        }
    }

}
