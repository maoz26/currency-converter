using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyProject
{
    /// <summary>
    /// This interface include a method for getting all currency exchange rates as a dictionary.
    /// And a method capable of calculating the conversion from one currency to another.
    /// </summary>
    interface IXMLCurrencyController
    {
        double CalculateRelation(Currency source, Currency destiny);     
         Dictionary<string, Currency> GetCurrencies();
         DateTime GetDate();
         void Update();        
    }
}
