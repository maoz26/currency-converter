using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyProject
{
    /// <summary>
    /// This class represents a currency object.
    /// </summary>
    public class Currency
    {
        private string name, code, country;
        private int unit;
        private double rate, change;

        public Currency() { }
        public Currency(string name, int unit, string code, string country, double rate, double change)
        {
            this.name = name;
            this.code = code;
            this.country = country;
            this.unit = unit;
            this.rate = rate;
            this.change = change;
        }

        public string Name { get { return name; } }
        public string Code { get { return code; } }
        public string Country { get { return country; } }
        public int Unit { get { return unit; } }
        public double Rate { get { return rate; } }
        public double Change { get { return change; } }


        // Using in object as double rate by implicit convertions.
        public static implicit operator double(Currency currency){
            return currency.rate;
        }

    }
}
