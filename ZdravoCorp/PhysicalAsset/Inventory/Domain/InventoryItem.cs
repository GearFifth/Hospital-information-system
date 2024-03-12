using ZdravoCorp.PhysicalAsset.Inventory.Repository;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Utils.Serializer;
using static System.Int32;

namespace ZdravoCorp.PhysicalAsset.Inventory.Domain
{
    public class InventoryItem : Serializable
    {
        public Equipment Equipment { get; set; }
        public Room Room { get; set; }
        public int Quantity { get; set; }

        public InventoryItem()
        {
            Equipment = new Equipment();
            Room = new Room();
            Quantity = 0;
        }

        public InventoryItem(Equipment equipment, Room room, int quantity)
        {
            Equipment = equipment;
            Room = room;
            Quantity = quantity;
        }

        public InventoryItem(InventoryItem item)
        {
            Equipment = item.Equipment;
            Room = item.Room;
            Quantity = item.Quantity;

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Equipment.Id,
                Room.Name,
                Quantity.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Equipment = EquipmentService.GetEquipment(values[0]);
            Room = RoomService.GetRoom(values[1]);
            Quantity = Parse(values[2]);
        }

        public string[] ToTable()
        {
            string[] rowValues =
            {
                Equipment.Name,
                Equipment.Type.ToString(),
                Equipment.IsDynamic.ToString(),
                Room.Name,
                Room.Type.ToString(),
                Quantity.ToString()
            };
            return rowValues;
        }
    }
}
