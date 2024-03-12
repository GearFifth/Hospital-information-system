using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Rooms.Repository
{
    public class SimpleRenovationRepository
    {
        public const string SimpleRenovationsFilePath = "..\\..\\..\\PhysicalAsset\\Rooms\\Data\\simpleRenovations.csv";
        public  List<SimpleRenovation> Renovations = new();
        public  Serializer<SimpleRenovation> SimpleRenovationsSerializer = new();

        public SimpleRenovationRepository()
        {
            Renovations = SimpleRenovationsSerializer.fromCSV(SimpleRenovationsFilePath);
        }

        public  void Save()
        {
            SimpleRenovationsSerializer.toCSV(SimpleRenovationsFilePath, Renovations);
        }

        public  void Add(SimpleRenovation renovation)
        {
            Renovations.Add(renovation);
            Save();
        }
    }
}
