using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;
using ZdravoCorp.Healthcare.Pharmacy.Drugs;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.NurseVIew;
using ZdravoCorp.Scheduling.Appointments.Add;

namespace ZdravoCorp.Healthcare.Pharmacy.Selling
{
    public partial class SupplyingPrescribedMedicationWindow : Window
    {
        public SupplyingPrescribedMedicationWindow()
        {
            InitializeComponent();
            loggedUsernameLabel.Content = Globals.LoggedUser!.Username;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NurseWindow nurseWindow = new();
            nurseWindow.Show();
            this.Close();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window == mainWindow) continue;
                
                window.Close();
            }
            Globals.LoggedUser = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeDataGrids();
        }

        private void InitializeDataGrids()
        {
            patientDataGrid.ItemsSource = PatientService.GetAllActivePatients();
            InitDrugPrescriptionDataGrid();
        }

        private void InitDrugPrescriptionDataGrid()
        {
            DataTable dataTable = InitPrescriptionTableColumns();
            drugPrescriptionDataGrid.ItemsSource = new DataView(dataTable);
        }

        private DataTable InitPrescriptionTableColumns()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("Patient username", typeof(string));
            dataTable.Columns.Add("Drug name", typeof(string));
            dataTable.Columns.Add("Therapy start", typeof(string));
            dataTable.Columns.Add("Therapy end", typeof(string));
            dataTable.Columns.Add("Daily dose", typeof(string));
            dataTable.Columns.Add("Consume time", typeof(string));
            return dataTable;
        }
        private static string[] DrugPrescriptionToTable(DrugPrescription drugPrescription)
        {
            string[] tableValues =
            {
                drugPrescription.Id.ToString(),
                drugPrescription.PatientUsername,
                drugPrescription.DrugName,
                drugPrescription.Period.Start.ToString(),
                drugPrescription.Period.End.ToString(),
                drugPrescription.DailyDose.ToString(),
                drugPrescription.consumeTime.ToString(),
            };
            return tableValues;
        }

        private bool IsDrugPrescriptionSelected()
        {
            if (drugPrescriptionDataGrid.SelectedItem != null) return true;
            
            Notification.ShowErrorDialog("Please first select the drug prescription! ");
            return false;
        }

        private void PatientDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (patientDataGrid.SelectedItem == null) return;

            var selectedPatient = (patientDataGrid.SelectedItem as Patient)!;
            var patientsPrescriptions = DrugPrescriptionService.GetPatientsUnusedPrescriptions(selectedPatient.Username);
            FillDrugPrescriptionDataGrid(patientsPrescriptions);
            
        }

        private void FillDrugPrescriptionDataGrid(List<DrugPrescription> patientsPrescriptions)
        {
            DataTable dataTable = InitPrescriptionTableColumns();

            foreach (DrugPrescription drugPrescription in patientsPrescriptions)
            {
                string[] row = DrugPrescriptionToTable(drugPrescription);
                dataTable.Rows.Add(row);
            }

            drugPrescriptionDataGrid.ItemsSource = new DataView(dataTable);
        }

        private void RefreshThisWindow()
        {
            SupplyingPrescribedMedicationWindow supplyingPrescribedMedicationWindow = new();
            supplyingPrescribedMedicationWindow.Show();
            this.Close();
        }

        private void SellMedicationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDrugPrescriptionSelected()) return;

            ProcessTransaction();

            DrugService.Save();
            DrugPrescriptionService.SaveRepository();
        }

        private void ProcessTransaction()
        {
            DrugPrescription selectedDrugPrescription = GetSelectedDrugPrescription();
            Drug drug = DrugService.GetDrug(selectedDrugPrescription.DrugName)!;

            try
            {
                if (!IsDrugInStock(drug)) return;
                selectedDrugPrescription.CalculateNextDoseDate();
                SuccessfulDrugTransaction(drug, selectedDrugPrescription);
            }
            catch (Exception ex)
            {
                Notification.ShowErrorDialog("Cannot sell next dose of medicine. " + ex.Message);
            }
        }

        private static void SuccessfulDrugTransaction(Drug drug, DrugPrescription selectedDrugPrescription)
        {
            drug.NumberOfPackages--;
            Notification.ShowSuccessDialog("Successfully sold medicine, next purchase can be done on: " + selectedDrugPrescription.NextDoseDate);
        }

        private static bool IsDrugInStock(Drug drug)
        {
            if (drug.NumberOfPackages != 0) return true;

            Notification.ShowErrorDialog("This drug is out of stock! ");
            return false;

        }

        private DrugPrescription GetSelectedDrugPrescription()
        {
            DataRowView selectedRow = (DataRowView)drugPrescriptionDataGrid.SelectedItem;

            int id = Convert.ToInt32(selectedRow["ID"].ToString()!);

            DrugPrescription drugPrescription = DrugPrescriptionService.GetDrugPrescription(id)!;
            return drugPrescription;
        }

        private void SendOnExaminationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDrugPrescriptionSelected()) return;
            
            DrugPrescription selectedDrugPrescription = GetSelectedDrugPrescription();

            if (DateTime.Now >= selectedDrugPrescription.Period.End)
            {
                ScheduleAppointment(selectedDrugPrescription.PatientUsername);
                return;
            }
            Notification.ShowErrorDialog("Patients therapy isn't finished yet! ");
        }

        private void ScheduleAppointment(string patientsUsername)
        {
            ChooseDoctorSpecializationDialog chooseDoctorSpecialization = new(PatientService.GetPatient(patientsUsername)!, GetSelectedDrugPrescription());
            chooseDoctorSpecialization.ShowDialog();
        }

        private void CheckMedicationInventoryBtn_Click(object sender, RoutedEventArgs e)
        {
            MedicationInventoryWindow medicationInventoryWindow = new();
            medicationInventoryWindow.Show();
            this.Close();
        }
    }
}
