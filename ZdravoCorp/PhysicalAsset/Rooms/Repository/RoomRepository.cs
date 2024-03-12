using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PhysicalAsset.Rooms.Repository
{
    public class RoomRepository
    {
        public const string RoomsFilePath = "..\\..\\..\\PhysicalAsset\\Rooms\\Data\\rooms.csv";
        public  List<Room> Rooms = new();
        public  Serializer<Room> RoomsSerializer = new();

        public RoomRepository()
        {
            Rooms = RoomsSerializer.fromCSV(RoomsFilePath);
        }

        public  void Save()
        {
            RoomsSerializer.toCSV(RoomsFilePath, Rooms);
        }

        public  Room GetRoom(string name)
        {
            return Rooms.FirstOrDefault(room => room.Name == name);
        }

        public  List<Room> GetRooms(Room.RoomType roomType)
        {
            return Rooms.FindAll(room => room.Type == roomType);
        }

        public  void Add(Room room)
        {
            Rooms.Add(room);
            Save();
        }

        public  void Remove(string roomName)
        {
            Rooms.RemoveAll(room => room.Name == roomName);
            Save();
        }


        public ObservableCollection<Room> GetAllAvailablePatientRooms(int numberOfDays)
        {
            ObservableCollection<Room> availableRooms = new();
            foreach (var room in Rooms.Where(room => CheckIfRoomIsAvailable(numberOfDays, room)))
            {
                availableRooms.Add(room);
            }
            return availableRooms;
        }

        private static int GetNumberOfPatientsInPatientRoomOnTimeSlot(int numberOfDays, Room room)
        {
            int numberOfPatients = 0;
            foreach (HospitalTreatment hospitalTreatment in HospitalTreatmentService.GetAllHospitalTreatments())
            {
                if (!CheckIfTreatmentIsAppropriate(room, hospitalTreatment)) continue;

                if (OverlapsWith(hospitalTreatment.TreatmentBeginning, hospitalTreatment.TreatmentEnding, numberOfDays)) 
                    numberOfPatients++;
            }

            return numberOfPatients;
        }
        private static bool CheckIfRoomIsAvailable(int numberOfDays, Room room)
        {
            if (room.Type != Room.RoomType.PatientRoom) return false;
            var numberOfPatients = GetNumberOfPatientsInPatientRoomOnTimeSlot(numberOfDays, room);
            return numberOfPatients < 3;
        }

        private static bool CheckIfTreatmentIsAppropriate(Room room, HospitalTreatment hospitalTreatment)
        {
            if (hospitalTreatment.Status == HospitalTreatment.TreatmentStatus.Finished) return false;
            return hospitalTreatment.RoomName == room.Name;
        }

        private static bool OverlapsWith(DateTime treatmentBeginning, DateTime treatmentEnding, int numberOfDays)
        {
            return (treatmentBeginning >= DateTime.Now && treatmentBeginning <= DateTime.Now.AddDays(numberOfDays)) ||
                   (treatmentEnding >= DateTime.Now && treatmentBeginning <= DateTime.Now);
        }
    }
}
