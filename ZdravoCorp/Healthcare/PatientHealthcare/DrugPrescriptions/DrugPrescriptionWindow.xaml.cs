using System;
using System.Windows;
using ZdravoCorp.Healthcare.Pharmacy.Drugs;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.PhysicalAsset;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Scheduling.Appointments;
using static ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions.DrugPrescription;

namespace ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions
{
    /// <summary>
    /// Interaction logic for PrescribeDrugWindow.xaml
    /// </summary>
    public partial class DrugPrescriptionWindow : Window
    {
        private readonly Appointment _appointment;
        public DrugPrescriptionWindow(Appointment appointment)
        {
            InitializeComponent();
            _appointment = appointment;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DrugPrescriptionService.AddPrescription(ParseDrugPrescriptionFromDialog());
                Notification.ShowSuccessDialog("Successfully added prescription.");
            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SpendDynamicEquipmentForm spendDynamicEquipmentForm = new(_appointment.RoomName);
            spendDynamicEquipmentForm.ShowDialog();
        }

        private DrugPrescription ParseDrugPrescriptionFromDialog()
        {
            if (startDatePicker.SelectedDate < DateTime.Today)
            {
                throw new ArgumentException("Date can't be in the past.");
            }

            string drugName = drugComboBox.Text;
            TimeSlot period = new(startDatePicker.SelectedDate.Value.Date, endDatePicker.SelectedDate.Value.Date);
            int dailyDosage = int.Parse(dailyDosageTextBox.Text);
            ConsumeTime consumeTime = (ConsumeTime)Enum.Parse(typeof(ConsumeTime), consumeTimeComboBox.Text);
            return new DrugPrescription(_appointment.PatientUsername, drugName, period, dailyDosage, consumeTime);
        }

        private void FillDrugComboBox()
        {
            foreach (var drug in DrugService.GetAllDrugs())
            {
                drugComboBox.Items.Add(drug.Name);
            }
        }

        private void FillConsumeTimeComboBox()
        {
            foreach (var consumeTime in Enum.GetValues(typeof(ConsumeTime)))
            {
                consumeTimeComboBox.Items.Add(consumeTime);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDrugComboBox();
            FillConsumeTimeComboBox();
            startDatePicker.SelectedDate = DateTime.Now;
            endDatePicker.SelectedDate = DateTime.Now;
        }
    }
}
