using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using static ZdravoCorp.PhysicalAsset.Rooms.Domain.Renovation;
using static ZdravoCorp.PhysicalAsset.Rooms.Domain.Room;

namespace ZdravoCorp.PhysicalAsset.Rooms.Service
{
    public static class SplitRoomRenovationService
    {
        public static SplitRoomRenovationRepository SplitRoomRenovationRepository = new();
        public static List<SplitRoomRenovation> GetSplitRoomRenovations()
        {
            return SplitRoomRenovationRepository.Renovations;
        }

        public static void Add(SplitRoomRenovation renovation)
        {
            SplitRoomRenovationRepository.Add(renovation);
        }

        public static void Save()
        {
            SplitRoomRenovationRepository.Save();
        }
        public static void CheckStartedRenovations()
        {
            foreach (var renovation in SplitRoomRenovationRepository.Renovations)
            {
                if (renovation.Status != RenovationStatus.STARTED) continue;
                if (renovation.TimeSlot.End > DateTime.Now) continue;
                renovation.Finish();
                CompleteRenovation(renovation);
            }
        }

        private static void CompleteRenovation(SplitRoomRenovation renovation)
        {
            AddNewRooms(renovation);
            TransferItemsToSplitRoom(renovation);
            RemoveOldRoom(renovation);
        }

        private static void RemoveOldRoom(SplitRoomRenovation renovation)
        {
            RoomService.RemoveRoom(renovation.RoomName);
        }

        private static void TransferItemsToSplitRoom(SplitRoomRenovation renovation)
        {
            InventoryService.TransferAllItemsInRoom(renovation.RoomName, renovation.FirstRoomName);
        }

        private static void AddNewRooms(SplitRoomRenovation renovation)
        {
            var firstRoom = new Room(renovation.FirstRoomName, renovation.RoomType);
            var secondRoom = new Room(renovation.SecondRoomName, renovation.SecondRoomType);
            RoomService.AddRoom(firstRoom);
            RoomService.AddRoom(secondRoom);
        }

        public static List<SplitRoomRenovation> GetOtherRenovations(string id)
        {
            return SplitRoomRenovationRepository.GetOtherRenovations(id);
        }
    }
}
