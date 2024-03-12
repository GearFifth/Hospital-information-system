using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Domain;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.Services;
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;
using ZdravoCorp.Utils.Commands;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.HospitalTreatmentCommands
{
    internal class FilterHospitalTreatmentCommand : CommandBase
    {
        private HospitalTreatmentVisitViewModel _hospitalTreatmentVisitViewModel;
        public FilterHospitalTreatmentCommand(HospitalTreatmentVisitViewModel hospitalTreatmentVisitViewModel)
        {
            _hospitalTreatmentVisitViewModel = hospitalTreatmentVisitViewModel;
        }

        public override void Execute(object? parameter)
        {
            _hospitalTreatmentVisitViewModel.HospitalTreatments = new ObservableCollection<HospitalTreatment>();
            string query = (parameter as TextBox)!.Text;
            ExecuteQuery(query);
        }

        private void ExecuteQuery(string query)
        {
            foreach (HospitalTreatment hospitalTreatment in HospitalTreatmentService.GetAllHospitalTreatments())
            {
                if(!hospitalTreatment.IsActive())continue;

                if (hospitalTreatment.PatientUsername.Contains(query) || hospitalTreatment.RoomName.Contains(query))
                {
                    _hospitalTreatmentVisitViewModel.HospitalTreatments.Add(hospitalTreatment);
                }
            }
        }
    }
}
