using FluentScheduler;

namespace ZdravoCorp.PhysicalAsset.Inventory.Service.InventoryTimer
{
    public class InventoryTimerRegistry:Registry
    {
        public InventoryTimerRegistry()
        {
            Schedule<InventoryTimerJob>().ToRunNow().AndEvery(60).Seconds();
        }
    }
}
