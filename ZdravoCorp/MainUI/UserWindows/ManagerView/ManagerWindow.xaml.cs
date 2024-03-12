using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using FluentScheduler;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.PhysicalAsset.Inventory.Presentation;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Orders.Domain;
using ZdravoCorp.PhysicalAsset.Orders.Presentation;
using ZdravoCorp.PhysicalAsset.Orders.Service;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Presentation;
using ZdravoCorp.PhysicalAsset.Rooms.Service;

namespace ZdravoCorp.MainUI.UserWindows.ManagerView
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private DispatcherTimer _datagGridTimer;
        private static readonly object itemSourceLock = new object();

        public ManagerWindow()
        {
            InitializeComponent();
            ManagerViewModel managerViewModel = new ManagerViewModel();
            this.DataContext = managerViewModel;

            LoggedInLabel.Content = "Logged in: " + Globals.LoggedUser.Username;

            _datagGridTimer = new DispatcherTimer();
            _datagGridTimer.Interval = TimeSpan.FromSeconds(41);
            _datagGridTimer.Tick += DataGridTick;
            _datagGridTimer.Start();
        }

        private void DataGridTick(object sender, EventArgs e)
        {
            try
            {
                UpdateInventoryTable(InventoryService.GetInventoryItems(), InventoryDataGrid);
                UpdateInventoryTable(InventoryService.GetMissingItems(), MissingInventoryDataGrid);
                UpdateOrdersTable(OrdersService.GetOrders(), OrdersDataGrid);
            }
            catch (Exception ex)
            {
                return;
            }
        }


        public void UpdateInventoryTable(List<InventoryItem> inventoryItems, DataGrid dataGrid)
        {
            lock (itemSourceLock)
            {
                dataGrid.ItemsSource = inventoryItems;
            }
        }

        public void UpdateOrdersTable(List<OrderItem> orderItems, DataGrid dataGrid)
        {
            lock (itemSourceLock)
            {
                dataGrid.ItemsSource = orderItems;
            }
        }

        private void InventoryDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateInventoryTable(InventoryService.GetInventoryItems(), InventoryDataGrid);
        }

        private void FilterByRoomTypeButton_Click(object sender, RoutedEventArgs e)
        {
            FilterByRoomTypeDialog filterByRoomTypeDialog = new(this);
            filterByRoomTypeDialog.ShowDialog();
        }

        private void FilterByEquipmentTypeButton_Click(object sender, RoutedEventArgs e)
        {
            FilterByEquipmentTypeDialog filterByEquipmentTypeDialog = new(this);
            filterByEquipmentTypeDialog.ShowDialog();
        }

        private void FilterByQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            FilterByQuantityDialog filterByQuantityDialog = new(this);
            filterByQuantityDialog.ShowDialog();
        }

        private void FilterNotInStorageButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateInventoryTable(InventoryService.GetItemsNotInStorage(), InventoryDataGrid);
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateInventoryTable(InventoryService.GetInventoryItems(), InventoryDataGrid);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateInventoryTable(InventoryService.GetSearchItems(SearchBox.Text), InventoryDataGrid);
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Globals.LoggedUser = null;
            this.Close();
        }


        private void MissingInventoryDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateInventoryTable(InventoryService.GetMissingItems(), MissingInventoryDataGrid);
        }

        private void OrdersDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateOrdersTable(OrdersService.GetOrders(), OrdersDataGrid);
        }


        private InventoryItem? GetSelectedItem(DataGrid dataGrid)
        {
            return (InventoryItem)dataGrid.SelectedItem;
        }

        private void OrderMissingItemsButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryItem item = GetSelectedItem(MissingInventoryDataGrid);
            if (item != null)
            {
                OrderItemDialog orderItemDialog = new OrderItemDialog(this, item);
                orderItemDialog.ShowDialog();
            }
            else
            {
                Notification.ShowErrorDialog("Please select an item to order!");
            }
        }

        public void OrderItem(InventoryItem item)
        {
            OrdersService.MakeOrder(item);
        }

        public void TransferItem(InventoryItem item, Room destinationRoom, DateTime moveTime)
        {
            TransferItemService.TransferItemRequest(item, destinationRoom, moveTime);
        }

        private void RefreshOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrdersTable(OrdersService.GetOrders(), OrdersDataGrid);
        }

        private void TransferItemButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryItem item = GetSelectedItem(InventoryDataGrid);
            if (item != null)
            {
                TransferItemDialog orderItemDialog = new TransferItemDialog(this, item);
                orderItemDialog.ShowDialog();
            }
            else
            {
                Notification.ShowErrorDialog("Please select an item to transfer!");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            JobManager.Stop();
        }

        private void UpdateRoomsTable(List<Room> rooms)
        {
            RoomsDataGrid.ItemsSource = rooms;
        }

        private void RoomsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRoomsTable(RoomService.GetAllOtherRooms("Storage"));
        }

        private Room? GetSelectedRoom(DataGrid dataGrid)
        {
            return (Room)dataGrid.SelectedItem;
        }

        private void SimpleRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            Room room = GetSelectedRoom(RoomsDataGrid);
            if (room != null)
            {
                ScheduleSimpleRenovationDialog scheduleSimpleRenovationDialog =
                    new ScheduleSimpleRenovationDialog(this, room);
                scheduleSimpleRenovationDialog.ShowDialog();
            }
            else
            {
                Notification.ShowErrorDialog("Please select a room!");
            }
        }

        private void UpdateSimpleRenovationsTable(List<SimpleRenovation> simpleRenovations)
        {
            SimpleRenovationsDataGrid.ItemsSource = simpleRenovations;
        }

        private void UpdateSplitRoomRenovationTable(List<SplitRoomRenovation> splitRoomRenovations)
        {
            SplitRoomRenovationsDataGrid.ItemsSource = splitRoomRenovations;
        }

        private void UpdateJoinRoomsRenovationTable(List<JoinRoomsRenovation> joinRoomsRenovations)
        {
            JoinRoomsRenovationsDataGrid.ItemsSource = joinRoomsRenovations;
        }

        private void SimpleRenovationsDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateSimpleRenovationsTable(SimpleRenovationService.GetAllSimpleRenovations());
        }

        private void SplitRoomRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            Room room = GetSelectedRoom(RoomsDataGrid);
            if (room != null)
            {
                ScheduleSplitRoomRenovationDialog scheduleSplitRoomRenovationDialog =
                    new ScheduleSplitRoomRenovationDialog(this, room);
                scheduleSplitRoomRenovationDialog.ShowDialog();
            }
            else
            {
                Notification.ShowErrorDialog("Please select a room!");
            }
        }

        private void JoinRoomsRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            Room room = GetSelectedRoom(RoomsDataGrid);
            if (room != null)
            {
                ScheduleJoinRoomsRenovation scheduleSplitRoomRenovationDialog =
                    new ScheduleJoinRoomsRenovation(this, room);
                scheduleSplitRoomRenovationDialog.ShowDialog();
            }
            else
            {
                Notification.ShowErrorDialog("Please select a room!");
            }
        }

        private void SplitRoomRenovationsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSplitRoomRenovationTable(SplitRoomRenovationService.GetSplitRoomRenovations());
        }

        private void JoinRoomsRenovationsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateJoinRoomsRenovationTable(JoinRoomsRenovationService.GetJoinRoomsRenovations());
        }
    }
}