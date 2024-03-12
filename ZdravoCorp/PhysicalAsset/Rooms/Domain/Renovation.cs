using System;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Rooms.Domain
{
    public class Renovation : Serializable
    {
        public string Id { get; set; }
        public string RoomName { get; set; }
        public Room.RoomType RoomType { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public RenovationStatus Status { get; set; }

        public enum RenovationStatus
        {
            SCHEDULED,
            STARTED,
            FINISHED
        }


        public Renovation()
        {
            TimeSlot = new();
        }

        public Renovation(string roomName, Room.RoomType roomType, TimeSlot timeSlot)
        {
            Id = "";
            RoomName = roomName;
            RoomType = roomType;
            TimeSlot = timeSlot;
            Status = RenovationStatus.SCHEDULED;
        }

        public void Start()
        {
            Status = RenovationStatus.STARTED;
        }

        public void Finish()
        {
            Status = RenovationStatus.FINISHED;
        }

        public virtual string[] ToCSV()
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

        public virtual void FromCSV(string[] values)
        {
            Id = values[0];
            RoomName = values[1];
            RoomType = (Room.RoomType)Enum.Parse(typeof(Room.RoomType), values[2]);
            TimeSlot.Start = Convert.ToDateTime(values[3]);
            TimeSlot.End = Convert.ToDateTime(values[4]);
            Status = (RenovationStatus)Enum.Parse(typeof(RenovationStatus), values[5]);
        }
    }
}
