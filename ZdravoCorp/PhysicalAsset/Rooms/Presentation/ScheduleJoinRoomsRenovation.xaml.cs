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
    /// Interaction logic for ScheduleJoinRoomsRenovation.xaml
    /// </summary>
    public partial class ScheduleJoinRoomsRenovation : Window
    {
        private readonly Room _room;
        private readonly ManagerWindow _managerWindow;

        public ScheduleJoinRoomsRenovation(ManagerWindow managerWindow, Room room)
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

        private void SubmitJoinRoomsRenovationButton_OnClickRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidFormFields())
                {
                        JoinRoomsRenovation renovation = CreateRenovation();
                        RenovationSchedule.ScheduleJoinRoomsRenovation(renovation);
                        Notification.ShowSuccessDialog("Join rooms renovation scheduled!");
                        Close();
                }
                else
                {
                    Notification.ShowErrorDialog("Please fill out all form fields!");
                }
            }
            catch (Exception ex)
            {
                Notification.ShowErrorDialog(ex.Message);
            }
        }

        private bool ValidFormFields()
        {
            return AreDatesValid() && IsRoomNameValid() && IsSecondRoomSelected();
        }

        private bool IsSecondRoomSelected()
        {
            return SecondRoomComboBox.SelectedItem is not null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomTypeComboBox();
            LoadRoomComboBox();
        }

        private void LoadRoomTypeComboBox()
        {
            foreach (var type in Enum.GetValues(typeof(Room.RoomType)))
            {
                NewRoomTypeComboBox.Items.Add(type.ToString());
            }

            RemoveStorageFromRoomTypeComboBox();
            SelectCurrentRoomType();
        }

        private void LoadRoomComboBox()
        {
            foreach (var room in RoomService.GetAllOtherRooms(_room.Name))
            {
                SecondRoomComboBox.Items.Add(room.Name);
            }
            SecondRoomComboBox.Items.Remove(new Room());
        }


        private void SelectCurrentRoomType()
        {
            NewRoomTypeComboBox.SelectedItem = _room.Type.ToString();
        }

        private void RemoveStorageFromRoomTypeComboBox()
        {
            NewRoomTypeComboBox.Items.Remove(Room.RoomType.Storage.ToString());
        }

        private bool IsRoomNameValid()
        {
            return RoomService.IsUniqueRoomName(NewRoomNameTextBox.Text);
        }

        private JoinRoomsRenovation CreateRenovation()
        {
            Room.RoomType newRoomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType),
                NewRoomTypeComboBox.SelectedValue.ToString());
            TimeSlot timeSlot = new TimeSlot(StartDatePicker.SelectedDate.Value,
                EndDatePicker.SelectedDate.Value);
            return new JoinRoomsRenovation(_room.Name, newRoomType, timeSlot, SecondRoomComboBox.Text,NewRoomNameTextBox.Text);

        }

    }
}
