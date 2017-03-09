using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyProject
{
    delegate void XMLdelegate();

    /// <summary>
    /// This class manages the connection to the remote server and gets updated data of Israel-bank.
    /// In addition, includes fields of currentCurrency and units to play with (change money). 
    /// </summary>
    public class XMLCurrencyController : IXMLCurrencyController
    {
        XDocument doc;
        Dictionary<string, Currency> currencies;

        DateTime lastModified;
        Currency currentCurrency; 
        double units;
        public XMLCurrencyController()
        {
        }
        public XMLCurrencyController(string url)
        {
            try
            {
                doc = XDocument.Load(url);
            }
            catch (Exception exc) {
                exc = new Exception("There is a problem to connect the remote server." + exc.InnerException);
            }
            XMLdelegate dele = Update;
            IAsyncResult iasync = dele.BeginInvoke(null, null);
            dele.EndInvoke(iasync);
            this.currentCurrency = currencies["NIS"];           
        }

        public double Units
        {
            get { 
                return units; 
            }
            set {
                units = value; 
            }
        }
        public Currency CurrentCurrency
        {
            get { 
                return currentCurrency; 
            }
            set {
                currentCurrency = value; 
            }
        }

        /// <summary>
        /// Gets the currencies on remote file
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Currency> GetCurrencies() {
            try
            {
                var data = from cu in doc.Descendants("CURRENCY")
                           select new
                           {
                               name = cu.Descendants("NAME").First().Value,
                               unit = cu.Descendants("UNIT").First().Value,
                               code = cu.Descendants("CURRENCYCODE").First().Value,
                               country = cu.Descendants("COUNTRY").First().Value,
                               rate = cu.Descendants("RATE").First().Value,
                               change = cu.Descendants("CHANGE").First().Value
                           };
                currencies = new Dictionary<string, Currency>();
                Currency nis = new Currency("Shekel", 1, "NIS", "Israel", 1, 1);
                currencies.Add(nis.Code, nis);
                foreach (var temp in data)
                {
                    Currency currency = new Currency(temp.name, Int32.Parse(temp.unit), temp.code, temp.country, Double.Parse(temp.rate), Double.Parse(temp.change));
                    currencies.Add(temp.code, currency);
                }
                return currencies;
            }
            catch (Exception exc)
            {
                exc = new Exception("There is a problem to encode the xml file. " + exc.InnerException);
            }
            return null;                            
        }

        /// <summary>
        /// Gets the last modified date on remote file
        /// </summary>
        /// <returns></returns>
        public DateTime GetDate()
        {
            try
            {
                var data = from d in doc.Descendants("CURRENCIES")
                           select new
                           {
                               date = d.Descendants("LAST_UPDATE").First().Value,
                           };
                foreach (var temp in data)
                {
                    string tempLastModified = temp.date;
                    string[] strArray = tempLastModified.Split('-');
                    return new DateTime(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2]));
                }
            }
            catch (Exception exc)
            {
                exc = new Exception("There is a problem to encode the xml file. " + exc.InnerException);
            }
            return default(DateTime);
        }


        /// <summary>
        /// Updates the controller data
        /// </summary>
        public void Update()
        {
            DateTime lastModified = GetDate();
            if (lastModified > this.lastModified)
            {
                this.lastModified = lastModified;
                this.currencies = GetCurrencies();
            }
        }


        /// <summary>
        /// This method gets an objects of currencies and returns the converted relation.
        /// </summary>
        /// <param name="currencySource"></param>
        /// <param name="currencyDestny"></param>
        /// <returns></returns>
        public double CalculateRelation(Currency currencySource, Currency currencyDestiny)
        {
            try
            {
                int unit1 = currencySource.Unit;
                int unit2 = currencyDestiny.Unit;
                double rate1 = currencySource.Rate;
                double rate2 = currencyDestiny.Rate;
                double relation1 = rate1 / unit1;
                double relation2 = rate2 / unit2;
                return (relation1 / relation2);
            }
            catch (Exception exc)
            {
                exc = new Exception("There is a problem to converts the currentCurrency" + exc.InnerException);
            }
            return 1;
        }

        /// <summary>
        /// This function gets a currency to convert and makes a change to the 'units' and 'currentCurrency' fields.
        /// </summary>
        /// <param name="currencyDestny"></param>
        /// <returns></returns>
        public double Convert(Currency currencyDestiny)
        {
            double relation = CalculateRelation(currentCurrency, currencyDestiny);
            units = relation * units;
            currentCurrency = (Currency)currencyDestiny;
            return relation;
        }
       
        public Dictionary<string, Currency> Currencies {
            get { 
                return currencies; 
            }
        }
        public DateTime LasrModified
        {
            get {
                return lastModified; 
            }
        }


    }
}
