using System.Windows;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.PhysicalAsset.Inventory.Service;

namespace ZdravoCorp.PhysicalAsset.Inventory.Presentation
{
    /// <summary>
    /// Interaction logic for FilterByEquipmentTypeDialog.xaml
    /// </summary>
    public partial class FilterByEquipmentTypeDialog : Window
    {
        private ManagerWindow _managerWindow;
        public FilterByEquipmentTypeDialog(ManagerWindow managerWindow)
        {
            InitializeComponent();
            _managerWindow = managerWindow;
        }

        private void SubmitButtonEquipmentType_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentTypeComboBox.SelectedValue is not null)
            {
                string equipmentType = EquipmentTypeComboBox.SelectedValue.ToString();
                _managerWindow.UpdateInventoryTable(InventoryService.GetItemsFilteredByEquipment(equipmentType), _managerWindow.InventoryDataGrid);
                Close();
            }
            else
            {
                Notification.ShowErrorDialog("Please select an equipment type!");
            }
        }
    }
}
