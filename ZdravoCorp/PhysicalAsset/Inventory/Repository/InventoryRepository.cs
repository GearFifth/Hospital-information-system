using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils.Serializer;



namespace ZdravoCorp.PhysicalAsset.Inventory.Repository
{
    public class InventoryRepository
    {
        public const string InventoryItemsFilePath = "..\\..\\..\\PhysicalAsset\\Inventory\\Data\\inventoryItems.csv";

        public  List<InventoryItem> InventoryItems = new();
        public  Serializer<InventoryItem> InventorySerializer = new();
        private  readonly object SaveLock = new object();

        public InventoryRepository()
        {
            InventoryItems = InventorySerializer.fromCSV(InventoryItemsFilePath);
        }

        public  void Save()
        {
            lock (SaveLock)
            {
                InventorySerializer.toCSV(InventoryItemsFilePath, InventoryItems);
            }
        }

        public  List<InventoryItem> FilterByEquipment(string type)
        {
            var equipmentType = (Equipment.EquipmentType)Enum.Parse(typeof(Equipment.EquipmentType), type);
            return InventoryItems.FindAll(item => item.Equipment.Type == equipmentType);
        }

        public  List<InventoryItem> FilterByRoomType(string type)
        {

            var roomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType), type);
            return InventoryItems.FindAll(item => item.Room.Type == roomType);
        }

        public  List<InventoryItem> FilterByRoom(string roomName)
        {
            return InventoryItems.FindAll(item => item.Room.Name == roomName);
        }

        public  List<InventoryItem> FilterByQuantity(int quantity)
        {
            return InventoryItems.FindAll(item => item.Quantity == quantity);
        }

        public  List<InventoryItem> FilterByQuantityRange(int lowerRange, int upperRange)
        {
            return InventoryItems.FindAll(item => item.Quantity >= lowerRange && item.Quantity <= upperRange);
        }

        public  List<InventoryItem> FilterNotInStorage()
        {
            return InventoryItems.FindAll(item => item.Room.Type != Room.RoomType.Storage);
        }

        public  List<InventoryItem> FilterContainingSearch(string search)
        {
            return InventoryItems.FindAll(item => item.Equipment.Contains(search)
                                                  || item.Room.Contains(search) || item.Quantity.ToString().Contains(search));
        }

        public  InventoryItem? GetItem(string equipmentName, string roomName)
        {
            return InventoryItems.FirstOrDefault(item =>
                Equals(item.Equipment.Name, equipmentName) &&
                Equals(item.Room.Name, roomName));
        }

        public  List<InventoryItem> GetItemsInRoom(string roomName)
        {
            return InventoryItems.FindAll(item => item.Room.Name == roomName);
        }
    }
}
