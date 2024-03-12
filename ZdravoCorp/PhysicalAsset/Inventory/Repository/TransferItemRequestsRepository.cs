using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Inventory.Repository
{
    public class TransferItemRequestsRepository : ITransferItemRequestsRepository
    {
        public const string TransferRequestsFilePath = "..\\..\\..\\PhysicalAsset\\Inventory\\Data\\transferItemRequests.csv";

        public List<TransferItemRequest> TransferRequests = new();
        public Serializer<TransferItemRequest> TransferRequestsSerializer = new();
        private readonly object SaveLock = new object();

        public TransferItemRequestsRepository()
        {
            TransferRequests = TransferRequestsSerializer.fromCSV(TransferRequestsFilePath);
        }

        public void Save()
        {
            lock (SaveLock)
            {
                TransferRequestsSerializer.toCSV(TransferRequestsFilePath, TransferRequests);
            }
        }

        public void Add(TransferItemRequest moveRequest)
        {
            TransferRequests.Add(moveRequest);
            Save();
        }

        public List<TransferItemRequest> GetAll()
        {
            return TransferRequests;
        }

        public bool IsUniqueId(string id)
        {
            return !TransferRequests.FindAll(item => item.Id == id).Any();
        }
    }
}
