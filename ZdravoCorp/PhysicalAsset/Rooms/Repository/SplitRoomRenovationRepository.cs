using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Rooms.Repository
{
    public class SplitRoomRenovationRepository
    {
        public const string SplitRoomRenovationsFilePath = "..\\..\\..\\PhysicalAsset\\Rooms\\Data\\splitRoomRenovations.csv";
        public  List<SplitRoomRenovation> Renovations = new();
        public  Serializer<SplitRoomRenovation> SplitRoomRenovationsSerializer = new();

        public SplitRoomRenovationRepository()
        {
            Renovations = SplitRoomRenovationsSerializer.fromCSV(SplitRoomRenovationsFilePath);
        }

        public  void Save()
        {
            SplitRoomRenovationsSerializer.toCSV(SplitRoomRenovationsFilePath, Renovations);
        }

        public  List<SplitRoomRenovation> GetOtherRenovations(string id)
        {
            return Renovations.FindAll(item => item.Id != id);
        }

        public  void Add(SplitRoomRenovation renovation)
        {
            Renovations.Add(renovation);
            Save();
        }
    }
}