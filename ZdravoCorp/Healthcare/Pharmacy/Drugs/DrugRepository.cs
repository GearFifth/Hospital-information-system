using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.Pharmacy.Drugs
{
    public class DrugRepository
    {
        public const string DrugsFilePath = "..\\..\\..\\Healthcare\\Pharmacy\\Drugs\\drugs.json";
        public  List<Drug> Drugs = new();

        public DrugRepository()
        {
            if (!File.Exists(DrugsFilePath)) return;

            string json = File.ReadAllText(DrugsFilePath);
            Drugs = JsonConvert.DeserializeObject<List<Drug>>(json);
        }

        public  void Save()
        {
            string json = JsonConvert.SerializeObject(Drugs, Formatting.Indented);
            File.WriteAllText(DrugsFilePath, json);
        }

        public  List<Drug> GetDrugsWithLessThanFivePackages()
        {
            return Drugs.Where(drug => drug.NumberOfPackages <= 5).ToList();
        }

        public  bool Contains(string name)
        {
            return Drugs.Any(drug => drug.Name == name);
        }

        public  Drug? GetDrug(string drugName)
        {
            return Drugs.FirstOrDefault(drug => drug.Name == drugName);
        }
    }
}
