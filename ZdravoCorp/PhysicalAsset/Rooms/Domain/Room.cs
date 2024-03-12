using System;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Rooms.Domain
{
    public class Room : Serializable
    {
        public enum RoomType
        {
            OperationRoom,
            ExaminationRoom,
            PatientRoom,
            WaitingRoom,
            Storage

        }

        public string Name { get; set; }
        public RoomType Type { get; set; }

        public Room()
        {
            Name = "Storage";
            Type = RoomType.Storage;
        }

        public Room(string name, RoomType type)
        {
            Name = name;
            Type = type;
        }

        public Room(string name, string type)
        {
            Name = name;
            Type = (RoomType)Enum.Parse(typeof(RoomType), type);
        }

        public Room(Room room)
        {
            Name = room.Name;
            Type = room.Type;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Name,
                Type.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Name = values[0];
            Type = (RoomType)Enum.Parse(typeof(RoomType), values[1]);
        }

        public bool Contains(string search)
        {
            var isInName = Name.Contains(search);
            var isInType = Type.ToString().Contains(search);
            return isInName || isInType;
        }

        public void ChangeType(RoomType type)
        {
            Type = type;
        }
    }
}
