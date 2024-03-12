using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Scheduling.Appointments;

namespace ZdravoCorp.PhysicalAsset.Rooms.Service
{
    public static class RenovationSchedule
    {

        public static void ScheduleSimpleRenovation(SimpleRenovation renovation)
        {
            if (!IsAvailable(renovation.RoomName, renovation.TimeSlot))
                throw new Exception("The selected room is already occupied for the given period!");
            if (RoomNameOverlapsWithRenovatedRoom(renovation))
                throw new Exception("The selected room name overlaps with future renovations!");
            RenovationService.Add(renovation);
        }

        public static void ScheduleSplitRoomRenovation(SplitRoomRenovation renovation)
        {
            if (!IsAvailable(renovation.RoomName, renovation.TimeSlot))
                throw new Exception("The selected room is already occupied for the given period!");
            if (RoomNameOverlapsWithRenovatedRoom(renovation))
                throw new Exception("The selected room name overlaps with future renovations!");
            RenovationService.Add(renovation);
        }

        public static void ScheduleJoinRoomsRenovation(JoinRoomsRenovation renovation)
        {
            if (!IsAvailable(renovation.RoomName, renovation.TimeSlot) ||
                !IsAvailable(renovation.SecondRoomName, renovation.TimeSlot))
                throw new Exception("The selected room is already occupied for the given period!");
            if (RoomNameOverlapsWithRenovatedRoom(renovation)) throw new Exception("The selected room name overlaps with future renovations!");
            RenovationService.Add(renovation);
        }

        public static void CheckRenovations()
        {
            RenovationService.CheckScheduledRenovations();
            RenovationService.CheckStartedRenovations();
            RenovationService.Save();
            RoomService.Save();
        }

        public static bool IsAvailable(string roomName, TimeSlot timeSlot)
        {

            return !OverlapsWithAnyAppointment(roomName, timeSlot) &&
                   !OverlapsWithAnySimpleRoomRenovation(roomName, timeSlot) &&
                   !OverlapsWithAnyJoinRoomsRenovation(roomName) && !OverlapsWithAnySplitRoomRenovation(roomName);
        }

        public static bool OverlapsWithAnyAppointment(string roomName, TimeSlot timeSlot)
        {
            var appointments = AppointmentService.GetAppointmentsInRange(timeSlot);
            return appointments.FindAll(appointment =>
                appointment.RoomName == roomName && appointment.TimeSlot.OverlapsWith(timeSlot)).Any();
        }

        public static bool OverlapsWithAnySimpleRoomRenovation(string roomName, TimeSlot timeSlot)
        {
            return RenovationService.GetAllRenovations().FindAll(renovation =>
                renovation.RoomName == roomName && renovation.TimeSlot.OverlapsWith(timeSlot)).Any();
        }

        public static bool OverlapsWithAnySplitRoomRenovation(string roomName)
        {
            return SplitRoomRenovationService.GetSplitRoomRenovations().FindAll(renovation =>
                renovation.RoomName == roomName).Any();
        }

        public static bool OverlapsWithAnyJoinRoomsRenovation(string roomName)
        {
            return JoinRoomsRenovationService.GetJoinRoomsRenovations().FindAll(renovation =>
                renovation.RoomName == roomName || renovation.SecondRoomName == roomName).Any();
        }

        public static bool RoomNameOverlapsWithRenovatedRoom(SimpleRenovation simpleRenovation)
        {
            return NameOverlapsSplitRenovation(simpleRenovation.RoomName) || NameOverlapsJoinRenovation(simpleRenovation.RoomName);
        }

        private static bool NameOverlapsJoinRenovation(string roomName)
        {
            return JoinRoomsRenovationService.GetJoinRoomsRenovations().Exists(renovation =>
                renovation.NewRoomName == roomName);
        }

        private static bool NameOverlapsSplitRenovation(string roomName)
        {
            return SplitRoomRenovationService.GetSplitRoomRenovations().Exists(renovation =>
                renovation.FirstRoomName == roomName ||
                renovation.SecondRoomName == roomName);
        }

        public static bool RoomNameOverlapsWithRenovatedRoom(SplitRoomRenovation splitRoomRenovation)
        {
            return NameOverlapsWithFirstRoom(splitRoomRenovation) || NameOverlapsWithSecondRoom(splitRoomRenovation) || NameOverlapsJoinRenovation(splitRoomRenovation);
        }

        private static bool NameOverlapsJoinRenovation(SplitRoomRenovation splitRoomRenovation)
        {
            return JoinRoomsRenovationService.GetJoinRoomsRenovations().Exists(renovation =>
                renovation.NewRoomName == splitRoomRenovation.FirstRoomName ||
                renovation.NewRoomName == splitRoomRenovation.SecondRoomName);
        }

        private static bool NameOverlapsWithSecondRoom(SplitRoomRenovation splitRoomRenovation)
        {
            var otherSplitRenovations = SplitRoomRenovationService.GetOtherRenovations(splitRoomRenovation.Id);
            return otherSplitRenovations.Exists(renovation =>
                renovation.SecondRoomName == splitRoomRenovation.FirstRoomName ||
                renovation.SecondRoomName == splitRoomRenovation.SecondRoomName);
        }

        private static bool NameOverlapsWithFirstRoom(SplitRoomRenovation splitRoomRenovation)
        {
            var otherSplitRenovations = SplitRoomRenovationService.GetOtherRenovations(splitRoomRenovation.Id);
            return otherSplitRenovations.Exists(renovation =>
                renovation.FirstRoomName == splitRoomRenovation.FirstRoomName ||
                renovation.FirstRoomName == splitRoomRenovation.SecondRoomName);
        }

        public static bool RoomNameOverlapsWithRenovatedRoom(JoinRoomsRenovation joinRoomsRenovation)
        {
            return NameOverlapsSplitRenovation(joinRoomsRenovation) || NameOverlapsJoinRenovations(joinRoomsRenovation);
        }

        private static bool NameOverlapsJoinRenovations(JoinRoomsRenovation joinRoomsRenovation)
        {
            var otherJoinRenovations = JoinRoomsRenovationService.GetOtherRenovations(joinRoomsRenovation.Id);
            return otherJoinRenovations.Exists(renovation =>
                renovation.NewRoomName == joinRoomsRenovation.NewRoomName);
        }

        private static bool NameOverlapsSplitRenovation(JoinRoomsRenovation joinRoomsRenovation)
        {
            return SplitRoomRenovationService.GetSplitRoomRenovations().Exists(renovation =>
                renovation.FirstRoomName == joinRoomsRenovation.NewRoomName ||
                renovation.SecondRoomName == joinRoomsRenovation.NewRoomName);
        }
    }
}
