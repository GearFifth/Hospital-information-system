using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.PatientHealthcare.DrugPrescriptions
{
    public interface IDrugPrescriptionRepository
    {
        public void Add(DrugPrescription drugPrescription);

        public List<DrugPrescription> GetPatientsPrescriptions(string patientsUsername);

        public DrugPrescription? GetDrugPrescription(int id);

        public void Save();
    }
}
