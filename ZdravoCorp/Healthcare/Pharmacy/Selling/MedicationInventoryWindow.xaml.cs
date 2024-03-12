using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using ZdravoCorp.Healthcare.Pharmacy.Drugs;
using ZdravoCorp.Healthcare.Pharmacy.Orders;
using ZdravoCorp.Healthcare.Roles.Patient;
using ZdravoCorp.MainUI;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.NurseVIew;

namespace ZdravoCorp.Healthcare.Pharmacy.Selling
{
    public partial class MedicationInventoryWindow : Window
    {
        private DataTable _ordersDataTable;
        private List<(string, int)> _orders;
        public MedicationInventoryWindow()
        {
            InitializeComponent();
            loggedUsernameLabel.Content = Globals.LoggedUser!.Username;
            _ordersDataTable = InitOrdersTableColumns();
            _orders = new List<(string, int)>();
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

        private object?[] PairToObjects((string,int) pair)
        {
            return new object?[] {pair.Item1, pair.Item2};
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            orderDataGrid.ItemsSource = new DataView(_ordersDataTable);
        }
        public void UpdatePatientsTable()
        {
            DataTable dataTable = InitOrdersTableColumns();

            foreach (Patient patient in PatientService.GetAllPatients())
            {
                dataTable.Rows.Add(patient.ToTable());
            }

            orderDataGrid.ItemsSource = new DataView(dataTable);
        }
        private static DataTable InitOrdersTableColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Drug Name", typeof(string));
            dt.Columns.Add("Quantity", typeof(string));
            return dt;
        }

        private void SyncOrdersWithOrderDataGrid()
        {
            _ordersDataTable = InitOrdersTableColumns();
            foreach ((string, int) order in _orders)
            {
                _ordersDataTable.Rows.Add(PairToObjects(order));
            }

            orderDataGrid.ItemsSource = new DataView(_ordersDataTable);
        }

        private void MedicineDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            medicineDataGrid.ItemsSource = DrugService.GetDrugsWithLessThanFivePackages();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string selectedDrugName = GetSelectedDrugName();
            if (selectedDrugName == null) return;

            int quantity = GetQuantityFromTextBox();
            if (quantity == -1) return;

            _orders.Add((selectedDrugName, quantity));
            SyncOrdersWithOrderDataGrid();
        }

        private int GetQuantityFromTextBox()
        {
            string inputText = quantityTextBox.Text;

            if (int.TryParse(inputText, out int number))
            {
                if (number > 0) return number;
            }
            Notification.ShowErrorDialog("Please first enter the quantity greater than 0 for drug you want to order!");
            return -1;
        }

        private string? GetSelectedDrugName()
        {
            if (medicineDataGrid.SelectedItem != null)
                return (medicineDataGrid.SelectedItem as Drug)?.Name!;
            
            Notification.ShowErrorDialog("Please first select the drug which you want to order!");
            return null;
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_orders.Count == 0)
            {
                Notification.ShowErrorDialog("Please first make orders for drugs! ");
                return;
            }
            MakeOrdersFromPairs();
            Notification.ShowSuccessDialog("Order made successfully and will arrive on: " + DateTime.Now.AddDays(1));
        }

        private void MakeOrdersFromPairs()
        {
            foreach ((string, int) pairOrder in _orders)
            {
                MedicationOrder order = new MedicationOrder(pairOrder.Item1, DateTime.Now, pairOrder.Item2);
                MedicationOrdersService.Add(order);
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (orderDataGrid.SelectedItem == null)
            {
                Notification.ShowErrorDialog("Please first select order which you want to remove! ");
                return;
            }

            var selectedOrder = orderDataGrid.SelectedItem as DataRowView;

            RemoveSelectedOrderFromOrders(selectedOrder);
            SyncOrdersWithOrderDataGrid();
        }

        private void RemoveSelectedOrderFromOrders(DataRowView? selectedOrder)
        {
            foreach ((string, int) order in _orders)
            {
                if (order.Item1 != selectedOrder!.Row.ItemArray[0]!.ToString() ||
                    order.Item2 != int.Parse(selectedOrder.Row.ItemArray[1]!.ToString()!)) continue;

                _orders.Remove(order);
                break;
            }
        }
    }
}
