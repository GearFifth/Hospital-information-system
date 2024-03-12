using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Orders.Domain;
using ZdravoCorp.PhysicalAsset.Orders.Repository;
using ZdravoCorp.Utils;

namespace ZdravoCorp.PhysicalAsset.Orders.Service
{
    public static class OrdersService
    {
        public static IOrderRepository OrdersRepository = new OrdersRepository();
        public static void GenerateUniqueId(OrderItem orderItem)
        {
            bool isUnique = false;
            string id = "";
            while (!isUnique)
            {
                id = RandomStringGenerator.GenerateRandomString(5);
                isUnique = OrdersRepository.IsUniqueId(id);
            }

            orderItem.Id = id;
        }

        public static List<OrderItem> GetOrders()
        {
            return OrdersRepository.GetAll();
        }

        public static void MakeOrder(InventoryItem inventoryItem)
        {
            OrderItem orderItem = new OrderItem();
            orderItem.InventoryItem = inventoryItem;
            GenerateUniqueId(orderItem);
            OrdersRepository.Add(orderItem);
        }

        public static void CheckOrders()
        {
            foreach (var item in OrdersRepository.GetAll())
            {
                if (item.Status == OrderItem.OrderStatus.SENT)
                {
                    if (item.Time < DateTime.Now)
                    {
                        item.Status = OrderItem.OrderStatus.FINISHED;
                        InventoryService.ReceiveOrder(item.InventoryItem);
                    }
                }
            }
            OrdersRepository.Save();
        }
    }
}
