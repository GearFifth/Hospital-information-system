using System;
using ZdravoCorp.Scheduling.Appointments;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Healthcare.Pharmacy.Orders
{
    public class MedicationOrder : Serializable
    {
        public string DrugName { get; set; }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }

        public bool IsDelivered { get; set; }

        public MedicationOrder(string drugName, DateTime orderDate, int quantity, bool isDelivered = false)
        {
            Id = -1;
            DrugName = drugName;
            OrderDate = orderDate;
            Quantity = quantity;
            IsDelivered = isDelivered;
        }

        public MedicationOrder()
        {
            DrugName = "";
            Id = -1;
            OrderDate = DateTime.MinValue;
            Quantity = 0;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                DrugName,
                OrderDate.ToString(),
                Quantity.ToString(),
                IsDelivered.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            DrugName = values[1];
            OrderDate = DateTime.Parse(values[2]);
            Quantity = int.Parse(values[3]);
            IsDelivered = bool.Parse(values[4]);
        }
    }
}
