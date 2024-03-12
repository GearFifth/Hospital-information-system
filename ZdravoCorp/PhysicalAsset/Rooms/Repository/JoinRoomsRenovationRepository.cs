using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.MainUI.Notices;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Rooms.Repository
{
    public class JoinRoomsRenovationRepository
    {
        public const string JoinRoomsRenovationsFilePath = "..\\..\\..\\PhysicalAsset\\Rooms\\Data\\joinRoomsRenovations.csv";
        public  List<JoinRoomsRenovation> Renovations = new();
        public  Serializer<JoinRoomsRenovation> JoinRoomsRenovationsSerializer = new();

        public JoinRoomsRenovationRepository()
        {
            Renovations = JoinRoomsRenovationsSerializer.fromCSV(JoinRoomsRenovationsFilePath);
        }

        public  void Save()
        {
            JoinRoomsRenovationsSerializer.toCSV(JoinRoomsRenovationsFilePath, Renovations);
        }

        public  List<JoinRoomsRenovation> GetOtherRenovations(string id)
        {
            return Renovations.FindAll(item => item.Id != id);
        }

        public  void Add(JoinRoomsRenovation renovation)
        {
            Renovations.Add(renovation);
            Save();
        }
    }
}
