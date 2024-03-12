using System;
using System.Windows;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Scheduling;

namespace ZdravoCorp.PhysicalAsset.Rooms.Presentation
{
    /// <summary>
    /// Interaction logic for ScheduleSplitRoomRenovationDialog.xaml
    /// </summary>
    public partial class ScheduleSplitRoomRenovationDialog : Window
    {
        private readonly Room _room;
        private readonly ManagerWindow _managerWindow;
        public ScheduleSplitRoomRenovationDialog(ManagerWindow managerWindow, Room room)
        {
            InitializeComponent();
            _managerWindow = managerWindow;
            _room = room;
        }

        public bool AreDatesValid()
        {
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue) return false;
            if (StartDatePicker.SelectedDate.Value >= DateTime.Now && EndDatePicker.SelectedDate.Value > DateTime.Now)
            {
                return EndDatePicker.SelectedDate.Value > StartDatePicker.SelectedDate.Value;
            }

            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomTypeComboBox();
        }

        private void LoadRoomTypeComboBox()
        {
            foreach (var type in Enum.GetValues(typeof(Room.RoomType)))
            {
                FirstRoomTypeComboBox.Items.Add(type.ToString());
                SecondRoomTypeComboBox.Items.Add(type.ToString());
            }
            RemoveStorageFromRoomTypeComboBox();
            SelectCurrentRoomType();
        }

        private void SelectCurrentRoomType()
        {
            FirstRoomTypeComboBox.SelectedItem = _room.Type.ToString();
            SecondRoomTypeComboBox.SelectedItem = _room.Type.ToString();
        }

        private void RemoveStorageFromRoomTypeComboBox()
        {
            FirstRoomTypeComboBox.Items.Remove(Room.RoomType.Storage.ToString());
            SecondRoomTypeComboBox.Items.Remove(Room.RoomType.Storage.ToString());
        }

        private bool AreRoomNamesValid()
        {
            if (FirstRoomNameTextBox.Text == string.Empty || SecondRoomNameTextBox.Text == string.Empty) return false;
            if (FirstRoomNameTextBox.Text == SecondRoomNameTextBox.Text) return false;
            var firstRoomNameValid = RoomService.IsUniqueRoomName(FirstRoomNameTextBox.Text);
            var secondRoomNameValid = RoomService.IsUniqueRoomName(SecondRoomNameTextBox.Text);
            return firstRoomNameValid && secondRoomNameValid;
        }

        private void SubmitSplitRoomRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AreDatesValid()&& AreRoomNamesValid())
                {
                        SplitRoomRenovation renovation = CreateRenovation();
                        RenovationSchedule.ScheduleSplitRoomRenovation(renovation);
                        Notification.ShowSuccessDialog("Split renovation scheduled!");
                        Close();
                }
                else
                {
                    Notification.ShowErrorDialog("Please select a valid time slot and room name!");
                }
            }
            catch (Exception ex)
            {
                Notification.ShowErrorDialog(ex.Message);
            }
        }

        private SplitRoomRenovation CreateRenovation()
        {
            Room.RoomType firstRoomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType),
                FirstRoomTypeComboBox.SelectedValue.ToString());
            Room.RoomType secondRoomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType),
                SecondRoomTypeComboBox.SelectedValue.ToString());
            TimeSlot timeSlot = new TimeSlot(StartDatePicker.SelectedDate.Value,
                EndDatePicker.SelectedDate.Value);
            return new SplitRoomRenovation(_room.Name, firstRoomType, timeSlot, FirstRoomNameTextBox.Text, SecondRoomNameTextBox.Text, secondRoomType);

        }
    }
}
