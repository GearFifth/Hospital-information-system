using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ZdravoCorp.PhysicalAsset.Rooms.Domain;
using ZdravoCorp.PhysicalAsset.Rooms.Repository;
using ZdravoCorp.Scheduling;
using ZdravoCorp.Scheduling.Appointments;
using static ZdravoCorp.PhysicalAsset.Rooms.Domain.Room;

namespace ZdravoCorp.PhysicalAsset.Rooms.Service
{
    public static class RoomService
    {
        public static RoomRepository RoomRepository = new();

        public static void Save()
        {
            RoomRepository.Save();
        }
        public static Room GetRoom(string roomName)
        {
            return RoomRepository.GetRoom(roomName);
        }

        public static ObservableCollection<Room> GetAllAvailablePatientRooms(int numberOfDays)
        {
            return RoomRepository.GetAllAvailablePatientRooms(numberOfDays);
        }

        public static List<Room> GetAllRooms()
        {
            return RoomRepository.Rooms;
        }
        public static Room GetFreeRoom(RoomType roomType, TimeSlot timeslot)
        {
            var room = RoomRepository.GetRooms(roomType).FirstOrDefault(room => IsFree(room.Name, timeslot));
            if (room == null)
            {
                throw new InvalidOperationException("There is no free room for given time.");
            }

            return room;
        }

        private static bool IsFree(string roomName, TimeSlot timeslot)
        {
            List<Appointment> overlappingAppointments = AppointmentService.GetActiveOverlappingAppointments(timeslot);
            return overlappingAppointments.All(appointment => appointment.RoomName != roomName) && RenovationSchedule.IsAvailable(roomName, timeslot);
        }

        public static List<Room> GetAllOtherRooms(string roomName)
        {
            return RoomRepository.Rooms.FindAll(item => item.Name != roomName);
        }

        public static Room GetFreeRoom(RoomType roomType, TimeSlot timeslot, int ignoreAppointmentId)
        {
            List<Room> rooms = RoomRepository.GetRooms(roomType);
            List<Appointment> overlappingAppointments = AppointmentService.GetActiveOverlappingAppointments(timeslot);
            foreach (Room room in rooms)
            {
                bool isFree = true;
                foreach (var appointment in overlappingAppointments)
                {
                    if (appointment.RoomName == room.Name && appointment.Id != ignoreAppointmentId)
                    {
                        isFree = false;
                        break;
                    }
                }
                if (isFree)
                {
                    return room;
                }
            }
            throw new InvalidOperationException("There is no free room for given time.");
        }

        public static void RemoveRoom(string roomName)
        {
            RoomRepository.Remove(roomName);
        }

        public static void AddRoom(Room room)
        {
            RoomRepository.Add(room);
        }

        public static bool IsUniqueRoomName(string roomName)
        {
            return !RoomRepository.Rooms.FindAll(room => room.Name == roomName).Any();
        }

        public static void ChangeRoomType(string roomName, RoomType type)
        {
            var room = GetRoom(roomName);
            room.ChangeType(type);
        }
    }
}
