using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;

namespace ZdravoCorp.PhysicalAsset.Rooms.Service
{
    public static class SimpleRenovationService
    {
        public static SimpleRenovationRepository SimpleRenovationRepository = new();
        public static List<SimpleRenovation> GetAllSimpleRenovations()
        {
            return SimpleRenovationRepository.Renovations;
        }

        public static void Add(SimpleRenovation renovation)
        {
            SimpleRenovationRepository.Add(renovation);
        }

        public static void Save()
        {
            SimpleRenovationRepository.Save();
        }

        public static void CheckStartedRenovations()
        {
            foreach (var renovation in SimpleRenovationRepository.Renovations)
            {
                if (renovation.Status != Renovation.RenovationStatus.STARTED) continue;
                if (renovation.TimeSlot.End > DateTime.Now) continue;
                renovation.Finish();
                RoomService.ChangeRoomType(renovation.RoomName,renovation.RoomType);
            }
        }
    }
}
