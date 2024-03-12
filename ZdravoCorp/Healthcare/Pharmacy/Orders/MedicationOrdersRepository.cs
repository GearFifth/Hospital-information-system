using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions;
using ZdravoCorp.Healthcare.Pharmacy.Drugs;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Healthcare.Pharmacy.Orders
{
    public class MedicationOrdersRepository
    {
        public const string OrderRepositoryItemsFilePath = "..\\..\\..\\Healthcare\\Pharmacy\\Orders\\orderMedication.csv";
        public  List<MedicationOrder> OrderedMedication = new();
        public  Serializer<MedicationOrder> OrdersSerializer = new();
        public MedicationOrdersRepository()
        {
            OrderedMedication = OrdersSerializer.fromCSV(OrderRepositoryItemsFilePath);
            UpdateDrugsInventory();
        }

        public void Add(MedicationOrder medicationOrder)
        {
            AssignId(medicationOrder);
            OrderedMedication.Add(medicationOrder);
            Save();
        }
        private void UpdateDrugsInventory()
        {
            foreach (MedicationOrder medicationOrder in OrderedMedication)
            {
                if (medicationOrder.IsDelivered || medicationOrder.OrderDate.AddDays(1) >= DateTime.Now) continue;

                Drug drug = DrugService.GetDrug(medicationOrder.DrugName)!;
                drug.NumberOfPackages += medicationOrder.Quantity;
                medicationOrder.IsDelivered = true;
            }
            DrugService.Save();
            Save();
        }

        public  void Save()
        {
            OrdersSerializer.toCSV(OrderRepositoryItemsFilePath, OrderedMedication);
        }

        public bool Contains(int id)
        {
            return OrderedMedication.Any(medicationOrder => medicationOrder.Id == id);
        }
        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        private void AssignId(MedicationOrder medicationOrder)
        {
            do
            {
                medicationOrder.Id = GenerateId();
            } while (Contains(medicationOrder.Id));
        }

    }
}
