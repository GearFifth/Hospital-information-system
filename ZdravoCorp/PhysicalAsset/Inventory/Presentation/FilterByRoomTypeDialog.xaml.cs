using System.Windows;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.PhysicalAsset.Inventory.Service;

namespace ZdravoCorp.PhysicalAsset.Inventory.Presentation
{
    /// <summary>
    /// Interaction logic for FilterByRoomTypeDialog.xaml
    /// </summary>
    public partial class FilterByRoomTypeDialog : Window
    {
        private ManagerWindow _managerWindow;
        public FilterByRoomTypeDialog(ManagerWindow managerWindow)
        {
            InitializeComponent();
            _managerWindow = managerWindow;
        }



        private void SubmitButtonRoomType_Click(object sender, RoutedEventArgs e)
        {
            if (RoomTypeComboBox.SelectedValue is not null)
            {
                string roomType = RoomTypeComboBox.SelectedValue.ToString();
                _managerWindow.UpdateInventoryTable(InventoryService.GetItemsFilteredByRooms(roomType), _managerWindow.InventoryDataGrid);
                Close();
            }
            else
            {
                Notification.ShowErrorDialog("Please select a room type!");
            }

        }
    }
}
