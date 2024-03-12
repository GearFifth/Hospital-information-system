using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Orders.Domain;

namespace ZdravoCorp.PhysicalAsset.Orders.Repository
{
    public interface IOrderRepository
    {
        public void Save();

        public List<OrderItem> GetAll();

        public void Add(OrderItem item);

        public bool IsUniqueId(string id);
    }
}
