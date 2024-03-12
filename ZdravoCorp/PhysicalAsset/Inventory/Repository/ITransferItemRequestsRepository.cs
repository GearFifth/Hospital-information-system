using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;

namespace ZdravoCorp.PhysicalAsset.Inventory.Repository
{
    public interface ITransferItemRequestsRepository
    {
        public void Save();

        public void Add(TransferItemRequest request);

        public List<TransferItemRequest> GetAll();

        public bool IsUniqueId(string id);
    }
}
