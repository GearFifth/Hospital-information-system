using System;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using ZdravoCorp.Scheduling;

namespace ZdravoCorp.PhysicalAsset.Rooms.Domain
{
    public class SimpleRenovation : Renovation
    {
        public SimpleRenovation(string roomName, Room.RoomType roomType, TimeSlot timeSlot) : base(roomName, roomType, timeSlot)
        {

        }

        public SimpleRenovation() : base()
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
        }
    }
}
