using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using ZdravoCorp.Utils;

namespace ZdravoCorp.PhysicalAsset.Rooms.Service
{
    public class RenovationService
    {
        public static RenovationRepository RenovationRepository = new(SimpleRenovationService.GetAllSimpleRenovations(),SplitRoomRenovationService.GetSplitRoomRenovations(),JoinRoomsRenovationService.GetJoinRoomsRenovations());
        public static List<Renovation> GetAllRenovations()
        {
            return RenovationRepository.Renovations;
        }

        public static void Save()
        {
            SimpleRenovationService.Save();
            SplitRoomRenovationService.Save();
            JoinRoomsRenovationService.Save();
        }

        public static void Add(SimpleRenovation renovation)
        {
            GenerateUniqueId(renovation);
            RenovationRepository.Add(renovation);
            SimpleRenovationService.Add(renovation);
        }

        public static void Add(SplitRoomRenovation renovation)
        {
            GenerateUniqueId(renovation);
            RenovationRepository.Add(renovation);
            SplitRoomRenovationService.Add(renovation);
        }

        public static void Add(JoinRoomsRenovation renovation)
        {
            GenerateUniqueId(renovation);
            RenovationRepository.Add(renovation);
            JoinRoomsRenovationService.Add(renovation);
        }

        public static void GenerateUniqueId(Renovation renovation)
        {
            bool isUnique = false;
            string id = "";
            while (!isUnique)
            {
                id = RandomStringGenerator.GenerateRandomString(5);
                isUnique = RenovationRepository.Renovations.FindAll(item => item.Id == id).Count == 0;
            }

            renovation.Id = id;
        }

        public static void CheckStartedRenovations()
        {
            SimpleRenovationService.CheckStartedRenovations();
            SplitRoomRenovationService.CheckStartedRenovations();
            JoinRoomsRenovationService.CheckStartedRenovations();
        }

        public static void CheckScheduledRenovations()
        {
            foreach (var renovation in RenovationRepository.Renovations)
            {
                if (renovation.Status != Renovation.RenovationStatus.SCHEDULED) continue;
                if (renovation.TimeSlot.Start > DateTime.Now) continue;
                renovation.Start();
            }
        }
    }
}
