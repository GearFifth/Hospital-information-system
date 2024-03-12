using System;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.PhysicalAsset.Inventory.Repository;
using ZdravoCorp.PhysicalAsset.Inventory.Service;

namespace ZdravoCorp.PhysicalAsset
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentForm.xaml
    /// </summary>
    public partial class SpendDynamicEquipmentForm : Window
    {
        private readonly string _roomName;
        private InventoryItem _selectedItem;
        public SpendDynamicEquipmentForm(string roomName)
        {
            InitializeComponent();
            _roomName = roomName;
            _selectedItem = null;
        }

        private void SpendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedItem == null)
                {
                    throw new InvalidOperationException("Equipment must be selected.");
                }

                int spentQuantity = int.Parse(spentQuantityTextBox.Text);
                if (spentQuantity > _selectedItem.Quantity)
                {
                    throw new InvalidOperationException("Not enough items to spend.");
                }

                InventoryService.SpendItem(_selectedItem, spentQuantity);
                currentQuantityLabel.Content = _selectedItem.Quantity.ToString();

            }
            catch (Exception error)
            {
                Notification.ShowErrorDialog(error.Message);
            }
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FillEquipmentComboBox()
        {
            foreach (InventoryItem item in InventoryService.GetDynamicEquipmentInRoom(_roomName))
            {
                equipmentComboBox.Items.Add(item.Equipment.Name);
            }

        }

        private void EquipmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            _selectedItem = InventoryService.GetItem(equipmentComboBox.SelectedValue.ToString(), _roomName);
            currentQuantityLabel.Content = _selectedItem.Quantity.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillEquipmentComboBox();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Notification.ShowSuccessDialog("Examination has been finished.");
        }
    }
}
