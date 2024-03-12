using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Orders.Domain;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Orders.Repository
{
    public class OrdersRepository : IOrderRepository
    {
        public const string OrderRepositoryItemsFilePath = "..\\..\\..\\PhysicalAsset\\Orders\\Data\\orderItems.csv";
        public List<OrderItem> OrderItems = new();
        public Serializer<OrderItem> OrdersSerializer = new();
        private readonly object SaveLock = new object();
        public OrdersRepository()
        {
            OrderItems = OrdersSerializer.fromCSV(OrderRepositoryItemsFilePath);
        }

        public void Save()
        {
            lock (SaveLock)
            {
                OrdersSerializer.toCSV(OrderRepositoryItemsFilePath, OrderItems);
            }
        }

        public List<OrderItem> GetAll()
        {
            return OrderItems;
        }

        public void Add(OrderItem item)
        {
            OrderItems.Add(item);
            Save();
        }

        public bool IsUniqueId(string id)
        {
            return !OrderItems.FindAll(item => item.Id == id).Any();
        }
    }
}
