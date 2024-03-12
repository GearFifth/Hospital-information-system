using System;
using ZdravoCorp.PhysicalAsset.Inventory.Service;
using ZdravoCorp.PhysicalAsset.Rooms.Service;
using ZdravoCorp.Scheduling;

namespace ZdravoCorp.PhysicalAsset.Rooms.Domain
{
    public class SplitRoomRenovation : Renovation
    {
        public string FirstRoomName { get; set; }
        public string SecondRoomName { get; set; }
        public Room.RoomType SecondRoomType { get; set; }

        public SplitRoomRenovation(string roomName, Room.RoomType roomType, TimeSlot timeSlot) : base(roomName, roomType, timeSlot)
        {

        }

        public SplitRoomRenovation(string roomName, Room.RoomType roomType, TimeSlot timeSlot, string firstRoomName, string secondRoomName, Room.RoomType secondRoomType) : base(roomName, roomType, timeSlot)
        {
            FirstRoomName = firstRoomName;
            SecondRoomName = secondRoomName;
            SecondRoomType = secondRoomType;
        }

        public SplitRoomRenovation() : base()
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
                FirstRoomName,
                SecondRoomName,
                SecondRoomType.ToString()
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
            FirstRoomName = values[6];
            SecondRoomName = values[7];
            SecondRoomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType), values[8]);
        }
    }
}
