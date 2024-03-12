using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.Pharmacy.Drugs
{
    public class DrugService
    {
        public static DrugRepository DrugRepository = new();
        public static List<Drug> GetAllDrugs()
        {
            return DrugRepository.Drugs;
        }

        public static Drug? GetDrug(string drugName)
        {
            return DrugRepository.GetDrug(drugName);
        }
        public static List<Drug> GetDrugsWithLessThanFivePackages()
        {
            return DrugRepository.GetDrugsWithLessThanFivePackages();
        }
        public static void Save()
        {
            DrugRepository.Save();
        }
    }
}
