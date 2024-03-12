using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ZdravoCorp.Healthcare.PatientHealthcare.ReportHistories;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.Scheduling.Appointments;

namespace ZdravoCorp.Healthcare.PatientHealthcare.MedicalRecords
{
    /// <summary>
    /// Interaction logic for MedicalRecordWindow.xaml
    /// </summary>
    public partial class MedicalRecordWindow : Window
    {
        private MedicalRecord _medicalRecord;
        private Dictionary<int, string> _reportHistory = new();
        DateTime _currentReportTimestamp;
        public MedicalRecordWindow(MedicalRecord medicalRecord)
        {
            InitializeComponent();
            this._medicalRecord = medicalRecord;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitDiseasesDataGrid();
            InitAllergyDataGrid();
            FillMedicalRecordWithValues();
            FillReportHistory();
        }

        private void FillMedicalRecordWithValues()
        {
            Patient patient = PatientService.GetPatient(_medicalRecord.PatientUsername);
            firstNameLabel.Content = patient.FirstName;
            lastNameLabel.Content = patient.LastName;
            usernameLabel.Content = patient.Username;
            dateOfBirthLabel.Content = _medicalRecord.DateOfBirth;
            heightLabel.Content = _medicalRecord.Height;
            weightLabel.Content = _medicalRecord.Weight;
            FillDataGridWithDiseases();
            FillDataGridWithAllergy();
        }

        private void InitDiseasesDataGrid()
        {
            var column = new DataGridTextColumn
            {
                Header = "Disease Name",
                Binding = new Binding()
            };

            diseasesDataGrid.Columns.Add(column);
        }

        private void InitAllergyDataGrid()
        {
            var column = new DataGridTextColumn
            {
                Header = "Allergy",
                Binding = new Binding()
            };

            allergyDataGrid.Columns.Add(column);
        }


        private void FillDataGridWithDiseases()
        {
            diseasesDataGrid.Items.Clear();

            if (_medicalRecord.DiseaseHistory[0].Trim().Equals(""))
            {
                return;
            }

            foreach (var disease in _medicalRecord.DiseaseHistory)
            {
                diseasesDataGrid.Items.Add(disease);
            }
            diseasesDataGrid.Items.Refresh();
        }

        private void FillDataGridWithAllergy()
        {
            allergyDataGrid.Items.Clear();

            if (AreAllergiesEmpty()) return;

            foreach (var disease in _medicalRecord.Allergies)
            {
                allergyDataGrid.Items.Add(disease);
            }

            allergyDataGrid.Items.Refresh();
        }

        private bool AreAllergiesEmpty()
        {
            if (_medicalRecord.Allergies == null || _medicalRecord.Allergies.Count == 0) return true;

            return _medicalRecord.Allergies[0].Trim().Equals("");
        }

        private void UpdateReportFields(DateTime previousReportTime, string previousReport)
        {
            dateOfReportLabel.Content = "Report from: " + previousReportTime.ToString("M/d/yyyy h:mm:ss tt");
            reportTextBox.Text = previousReport;
            _currentReportTimestamp = previousReportTime;
        }

        private void FillReportHistory()
        {
            _reportHistory = ReportHistoryService.GetReportHistory(_medicalRecord.PatientUsername);

            if (!_reportHistory.Any())
            {
                reportTextBox.IsReadOnly = true;
                return;
            }

            KeyValuePair<int, string> mostReacentReport = _reportHistory.MaxBy(x => x.Key);
            UpdateReportFields(AppointmentService.GetAppointment(mostReacentReport.Key).TimeSlot.Start, mostReacentReport.Value);

        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (KeyValuePair<int, string> report in _reportHistory.OrderBy(x => x.Key))
            {
                if (AppointmentService.GetAppointment(report.Key).TimeSlot.Start <= _currentReportTimestamp) continue;

                UpdateReportFields(AppointmentService.GetAppointment(report.Key).TimeSlot.Start, report.Value);
                break;
            }
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime previousReportTime = default;
            string previousReport = "";
            foreach (KeyValuePair<int, string> report in _reportHistory.OrderBy(x => x.Key))
            {
                if (AppointmentService.GetAppointment(report.Key).TimeSlot.Start < _currentReportTimestamp)
                {
                    previousReport = report.Value;
                    previousReportTime = AppointmentService.GetAppointment(report.Key).TimeSlot.Start;
                    continue;
                }

                if (previousReportTime == default) break;

                UpdateReportFields(previousReportTime, previousReport);
                break;
            }
        }
    }
}
