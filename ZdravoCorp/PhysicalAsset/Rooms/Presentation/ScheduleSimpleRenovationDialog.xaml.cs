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
    /// Interaction logic for ScheduleSimpleRenovationDialog.xaml
    /// </summary>
    public partial class ScheduleSimpleRenovationDialog : Window
    {
        private readonly Room _room;
        private readonly ManagerWindow _managerWindow;
        public ScheduleSimpleRenovationDialog(ManagerWindow managerWindow,Room room)
        {
            InitializeComponent();
            _managerWindow = managerWindow;
            _room = room;
        }

        private void SubmitSimpleRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AreDatesValid())
                {
                    Room.RoomType roomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType),
                        RoomTypeComboBox.SelectedValue.ToString());
                    TimeSlot timeSlot = new TimeSlot(StartDatePicker.SelectedDate.Value,
                        EndDatePicker.SelectedDate.Value);
                    RenovationSchedule.ScheduleSimpleRenovation(new SimpleRenovation(_room.Name,roomType,timeSlot));
                    Notification.ShowSuccessDialog("Simple renovation scheduled!");
                    Close();
                }
                else
                {
                    Notification.ShowErrorDialog("Please select a valid time slot!");
                }
            }
            catch (Exception ex)
            {
                Notification.ShowErrorDialog(ex.Message);
            }


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
                RoomTypeComboBox.Items.Add(type.ToString());
            }
            RoomTypeComboBox.Items.Remove(Room.RoomType.Storage.ToString());
            RoomTypeComboBox.SelectedItem = _room.Type.ToString();
        }
    }
}
