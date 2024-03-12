using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;

namespace ZdravoCorp.PhysicalAsset.Inventory.Presentation
{
    /// <summary>
    /// Interaction logic for TransferItemDialog.xaml
    /// </summary>
    public partial class TransferItemDialog : Window
    {
        public TransferItemDialog(ManagerWindow managerWindow, InventoryItem item)
        {
            _item = new(item);
            _managerWindow = managerWindow;
            InitializeComponent();
            if (item.Equipment.IsDynamic)
            {
                DisableDatePicker();
            }
            LoadRoomCheckBox();
        }

        private ManagerWindow _managerWindow;
        private InventoryItem _item;

        private void DisableDatePicker()
        { 
            TransferItemDatePicker.IsEnabled = false; 
            TransferTimeTextBox.IsEnabled = false;


        }

        private void PreviewNumberInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = FilterByQuantityDialog.IsTextAllowed(e.Text);
        }

        private void LoadRoomCheckBox()
        {
            List<Room> rooms = RoomService.GetAllOtherRooms(_item.Room.Name);
            foreach (Room room in rooms)
            {
                RoomNameComboBox.Items.Add(room.Name);
            }
        }
        

        public bool CheckQuantity()
        {
            if (!string.IsNullOrEmpty(TransferQuantityTextBox.Text) && int.TryParse(TransferQuantityTextBox.Text, out _))
            {
                int quantity = int.Parse(TransferQuantityTextBox.Text);
                if (quantity > 0 && quantity <= _item.Quantity)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckRoomNameComboBox()
        {
            return RoomNameComboBox.SelectedValue is not null;
        }

        private void SubmitMoveButton_Click(object sender, RoutedEventArgs e)
        {
            Room room = new();
            if (ValidFormFields())
            {
                ParseTransferForm();
                Close();
            }
            else
            {
                Notification.ShowErrorDialog("Please fill out all the fields!");
            }
        }

        private bool ValidFormFields()
        {
            return CheckQuantity() && CheckRoomNameComboBox()&& ValidDateTime();
        }

        private bool ValidDateTime()
        {
            if (!_item.Equipment.IsDynamic)
            {
                if (TransferItemDatePicker.SelectedDate.HasValue &&
                    TimeOnly.TryParse(TransferTimeTextBox.Text, out TimeOnly result))
                {
                    DateTime date = TransferItemDatePicker.SelectedDate.Value.Date;
                    TimeOnly time = TimeOnly.Parse(TransferTimeTextBox.Text);
                    date = date.AddHours(time.Hour).AddMinutes(time.Minute);
                    return date>=DateTime.Now;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void ParseTransferForm()
        {
            _item.Quantity = int.Parse(TransferQuantityTextBox.Text);
            Room room = RoomService.GetRoom(RoomNameComboBox.SelectedValue.ToString());
            DateTime date = DateTime.Now;
            if (!_item.Equipment.IsDynamic)
            {
                date = TransferItemDatePicker.SelectedDate.Value.Date;
                TimeOnly time = TimeOnly.Parse(TransferTimeTextBox.Text);
                date = date.AddHours(time.Hour).AddMinutes(time.Minute);
            }
            _managerWindow.TransferItem(_item, room, date);
        }

    }
}
