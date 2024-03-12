using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Inventory.Repository
{
    public class EquipmentRepository
    {
        public const string EquipmentFilePath = "..\\..\\..\\PhysicalAsset\\Inventory\\Data\\equipment.csv";
        public List<Domain.Equipment> Equipment = new();
        public Serializer<Domain.Equipment> EquipmentSerializer = new();

        public EquipmentRepository()
        {
            Equipment = EquipmentSerializer.fromCSV(EquipmentFilePath);
        }

        public void Save()
        {
            EquipmentSerializer.toCSV(EquipmentFilePath, Equipment);
        }

        public Domain.Equipment GetEquipment(string id)
        {
            return Equipment.FirstOrDefault(item => item.Id == id);
        }
    }
}
