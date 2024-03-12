namespace ZdravoCorp.Healthcare.Pharmacy.Orders
{
    public static class MedicationOrdersService
    {
        public static MedicationOrdersRepository MedicationOrdersRepository = new();
        public static void Save()
        {
            MedicationOrdersRepository.Save();
        }

        public static void Add(MedicationOrder medicationOrder)
        {
            MedicationOrdersRepository.Add(medicationOrder);
        }
    }
}
