using System;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Scheduling;

namespace ZdravoCorp.PhysicalAsset.Rooms.Domain
{
    public class JoinRoomsRenovation : Renovation
    {
        public string SecondRoomName { get; set; }
        public string NewRoomName { get; set; }


        public JoinRoomsRenovation(string roomName, Room.RoomType roomType, TimeSlot timeSlot) : base(roomName, roomType, timeSlot)
        {

        }

        public JoinRoomsRenovation(string roomName, Room.RoomType roomType, TimeSlot timeSlot, string secondRoomName, string newRoomName) : base(roomName, roomType, timeSlot)
        {
            SecondRoomName = secondRoomName;
            NewRoomName = newRoomName;
        }

        public JoinRoomsRenovation() : base()
        {

        }

        public override string[] ToCSV()
        {
            string[] csvValues =
            {
                Id,
                RoomName,
                RoomType.ToString(),
                TimeSlot.Start.ToString(),
                TimeSlot.End.ToString(),
                Status.ToString(),
                SecondRoomName,
                NewRoomName
            };
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = values[0];
            RoomName = values[1];
            RoomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType), values[2]);
            TimeSlot.Start = DateTime.Parse(values[3]);
            TimeSlot.End = DateTime.Parse(values[4]);
            Status = (RenovationStatus)Enum.Parse(typeof(RenovationStatus), values[5]);
            SecondRoomName = values[6];
            NewRoomName = values[7];
        }
    }
}