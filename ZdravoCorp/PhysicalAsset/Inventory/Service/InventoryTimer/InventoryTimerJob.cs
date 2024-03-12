using System.Threading.Tasks;
using FluentScheduler;
using ZdravoCorp.PhysicalAsset.Orders.Service;

namespace ZdravoCorp.PhysicalAsset.Inventory.Service.InventoryTimer
{
    public class InventoryTimerJob : IJob
    {
        public void Execute()
        {
            Task.Run(OrdersService.CheckOrders);
            Task.Run(TransferItemService.CheckTransferRequests);
        }
    }
}
