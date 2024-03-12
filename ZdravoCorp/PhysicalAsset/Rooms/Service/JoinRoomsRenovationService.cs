using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.MainUI.Notices;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using static ZdravoCorp.PhysicalAsset.Rooms.Domain.Renovation;
using static ZdravoCorp.PhysicalAsset.Rooms.Domain.Room;

namespace ZdravoCorp.PhysicalAsset.Rooms.Service
{
    public static class JoinRoomsRenovationService
    {
        public static JoinRoomsRenovationRepository JoinRoomsRenovationRepository = new();
        public static List<JoinRoomsRenovation> GetJoinRoomsRenovations()
        {
            return JoinRoomsRenovationRepository.Renovations;
        }

        public static void Add(JoinRoomsRenovation renovation)
        {
            JoinRoomsRenovationRepository.Add(renovation);
        }

        public static void Save()
        {
            JoinRoomsRenovationRepository.Save();
        }

        public static void CheckStartedRenovations()
        {
            foreach (var renovation in JoinRoomsRenovationRepository.Renovations)
            {
                if (renovation.Status != RenovationStatus.STARTED) continue;
                if (renovation.TimeSlot.End > DateTime.Now) continue;
                renovation.Finish();
                CompleteRenovation(renovation);
            }
        }

        private static void CompleteRenovation(JoinRoomsRenovation renovation)
        {
            AddNewRoom(renovation);
            TransferItemsToJoinedRoom(renovation);
            RemoveJoinedRooms(renovation);
        }

        private static void TransferItemsToJoinedRoom(JoinRoomsRenovation renovation)
        {
            InventoryService.TransferAllItemsInRoom(renovation.RoomName, renovation.NewRoomName);
            InventoryService.TransferAllItemsInRoom(renovation.SecondRoomName, renovation.NewRoomName);
        }

        private static void RemoveJoinedRooms(JoinRoomsRenovation renovation)
        {
            RoomService.RemoveRoom(renovation.RoomName);
            RoomService.RemoveRoom(renovation.SecondRoomName);
        }

        private static void AddNewRoom(JoinRoomsRenovation renovation) 
        {
            var newRoom = new Room(renovation.NewRoomName, renovation.RoomType);
            RoomService.AddRoom(newRoom);
        }

        public static List<JoinRoomsRenovation> GetOtherRenovations(string id)
        {
            return JoinRoomsRenovationRepository.GetOtherRenovations(id);
        }
    }
}
