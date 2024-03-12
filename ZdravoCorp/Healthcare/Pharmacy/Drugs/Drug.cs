using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.Pharmacy.Drugs
{
    public class Drug
    {
        public string Name { get; set; }

        public List<string> Ingredients { get; set; }

        public int NumberOfPills { get; set; }

        public int NumberOfPackages { get; set; }

        public Drug(string name, List<string> ingredients, int numberOfPills, int numberOfPackages = 0)
        {
            Name = name;
            Ingredients = ingredients;
            NumberOfPills = numberOfPills;
            NumberOfPackages = numberOfPackages;
        }

        public bool Contains(string ingredient)
        {
            return Ingredients.Any(i => i == ingredient);
        }
    }
}
