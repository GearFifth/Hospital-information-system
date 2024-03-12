using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Inventory.Domain;
using ZdravoCorp.PhysicalAsset.Inventory.Repository;

namespace ZdravoCorp.PhysicalAsset.Inventory.Service
{
    public class EquipmentService
    {
        public static EquipmentRepository EquipmentRepository= new();
        public static Equipment GetEquipment(string id)
        {
            return EquipmentRepository.GetEquipment(id);
        }
    }
}
