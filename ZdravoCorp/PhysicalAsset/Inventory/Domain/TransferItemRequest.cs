using System;
using ZdravoCorp.PhysicalAsset.Inventory.Repository;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Inventory.Domain
{
    public class TransferItemRequest : Serializable
    {

        public enum TransferStatus
        {
            SENT,
            FINISHED,
            CANCELLED
        }
        public string Id { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public DateTime TransferTime { get; set; }
        public Room DestinationRoom { get; set; }
        public TransferStatus Status { get; set; }

        public TransferItemRequest()
        {
            Id = "";
            InventoryItem = new InventoryItem();
            TransferTime = DateTime.Now.AddDays(1);
            DestinationRoom = new Room();
            Status = TransferStatus.SENT;
        }

        public TransferItemRequest(InventoryItem inventoryItem, Room destinationRoom, DateTime dateTime)
        {
            Id = "";
            InventoryItem = new(inventoryItem);
            DestinationRoom = destinationRoom;
            Status = TransferStatus.SENT;
            TransferTime = inventoryItem.Equipment.IsDynamic ? DateTime.Now : dateTime;
        }

        public TransferItemRequest(string id, InventoryItem inventoryItem, DateTime transferTime, Room destinationRoom, TransferStatus status)
        {
            Id = id;
            InventoryItem = new(inventoryItem);
            TransferTime = transferTime;
            DestinationRoom = destinationRoom;
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id,
                InventoryItem.Equipment.Id,
                InventoryItem.Room.Name,
                InventoryItem.Quantity.ToString(),
                DestinationRoom.Name,
                TransferTime.ToString(),
                Status.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = values[0];
            InventoryItem.Equipment = InventoryService.GetEquipment(values[1]);
            InventoryItem.Room = RoomService.GetRoom(values[2]);
            InventoryItem.Quantity = int.Parse(values[3]);
            DestinationRoom = RoomService.GetRoom(values[4]);
            TransferTime = DateTime.Parse(values[5]);
            Status = (TransferStatus)Enum.Parse(typeof(TransferStatus), values[6]);
        }

        public string[] ToTable()
        {
            string[] rowValues =
            {
                Id,
                InventoryItem.Equipment.Name,
                InventoryItem.Equipment.Type.ToString(),
                InventoryItem.Equipment.IsDynamic.ToString(),
                InventoryItem.Room.Name,
                InventoryItem.Room.Type.ToString(),
                InventoryItem.Quantity.ToString(),
                DestinationRoom.Name,
                TransferTime.ToString(),
                Status.ToString()
            };
            return rowValues;
        }

        public void CancelTransfer()
        {
            Status = TransferStatus.CANCELLED;
        }

        public void FinishTransfer()
        {
            Status = TransferStatus.FINISHED;
        }
    }
}
