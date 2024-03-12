using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Microsoft.Win32;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Service;

namespace ZdravoCorp.PhysicalAsset.Rooms.Repository
{
    public class RenovationRepository
    {
        public  List<Renovation> Renovations = new List<Renovation>();

        public RenovationRepository(List<SimpleRenovation> simpleRenovations,List<SplitRoomRenovation> splitRoomRenovations,List<JoinRoomsRenovation> joinRoomsRenovations)
        {
            Renovations.AddRange(simpleRenovations);
            Renovations.AddRange(splitRoomRenovations);
            Renovations.AddRange(joinRoomsRenovations);
        }

        public  void Add(Renovation renovation)
        {
            Renovations.Add(renovation);
        }
        
    }
}
