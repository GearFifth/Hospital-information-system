using System.Windows;
using System.Windows.Input;
using ZdravoCorp.MainUI.NotificationDialogs;
using ZdravoCorp.MainUI.UserWindows;
using ZdravoCorp.MainUI.UserWindows.ManagerView;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.PhysicalAsset.Inventory.Presentation;

namespace ZdravoCorp.PhysicalAsset.Orders.Presentation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OrderItemDialog : Window
    {
        private ManagerWindow _managerWindow;
        private InventoryItem _item;

        public OrderItemDialog(ManagerWindow managerWindow,InventoryItem item)
        {
            _item = item;
            _managerWindow = managerWindow;
            InitializeComponent();
        }

        private void PreviewNumberInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = FilterByQuantityDialog.IsTextAllowed(e.Text);
        }


        private void SubmitOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var quantity = 1;
            if (!string.IsNullOrEmpty(OrderQuantityTextBox.Text) && int.TryParse(OrderQuantityTextBox.Text, out _))
            {
                quantity = int.Parse(OrderQuantityTextBox.Text);
                if (quantity > 0)
                {
                    _item.Quantity = quantity;
                    _managerWindow.OrderItem(_item);
                    Close();
                }
                else
                {
                    Notification.ShowErrorDialog("Please select a valid quantity!");
                }
            }
        }
    }
}
