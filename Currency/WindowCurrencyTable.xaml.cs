using CurrencyProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CurrencyProject
{
    /// <summary>
    /// Interaction logic for WindowCurrencyTable.xaml
    /// </summary>
    /// 

    public partial class WindowCurrencyTable : Window
    {     
        public WindowCurrencyTable()
        {
            InitializeComponent();

        }

        public WindowCurrencyTable(Dictionary<string, Currency> currencies)
            : this()
        {           
            dgCurrencies.ItemsSource = currencies.Values;
        }

        private void dgCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
