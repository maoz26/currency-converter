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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        XMLCurrencyController controller;     
        public MainWindow()
        {
            InitializeComponent();
            InitializeController();
            LoadComponents();
        }

        public void InitializeController()
        {           
            this.controller = new XMLCurrencyController("http://www.bankisrael.gov.il/currency.xml");           
        }

        public void LoadComponents()
        {
            this.Title = "Currency Converter!";
            // load info on components
            Updated.Content = "Updated to: " + controller.LasrModified;
            List<ComboBoxItem> list = new List<ComboBoxItem>();
            comboboxTo.SelectedItem = null;
            comboboxFrom.SelectedItem = null;
            foreach (var item in controller.Currencies)
            {
                comboboxTo.Items.Add(new Item(item.Value.Name + "-" + item.Key, item.Key));
                comboboxFrom.Items.Add(new Item(item.Value.Name + "-" + item.Key, item.Key));
            }
            comboboxTo.SelectedIndex = 0;
            comboboxFrom.SelectedIndex = 0;
            LabelResult.Content = controller.Units;
            LabelCurrencyName.Content = ((Item)comboboxTo.SelectedValue).Name;
        }
                       
        private void Button_Click_Convert(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initiate wallet
                double input = Int32.Parse(textboxInput.Text);
                controller.Units = input; // initiates the units of wallet
                controller.CurrentCurrency = controller.Currencies[((Item)comboboxFrom.SelectedValue).Value]; // sets the currency in the wallet by key
                // Change the money in wallet
                controller.Convert(controller.Currencies[((Item)comboboxTo.SelectedValue).Value]); // change the money in the wallet by givven currency 
                // Updates componnents content
                LabelResult.Content = controller.Units;
                LabelCurrencyName.Content =  ((Item)comboboxTo.SelectedValue).Name;
            }
            catch (Exception exc)
            {

            }
        }
        private void Button_ClickOpenCurrencyTable(object sender, RoutedEventArgs e)
        {
            WindowCurrencyTable windowCurrencyTable = new WindowCurrencyTable(controller.Currencies);
            windowCurrencyTable.Show();
        }

        private void Button_ClickUpdate(object sender, RoutedEventArgs e)
        {
            controller.Update();
            LoadComponents();
        }
       
    }

    /// <summary>
    /// This class represents items on components
    /// </summary>
    public class Item
    {
        public string Name;
        public string Value;
        public Item(string name, string value)
        {
            Name = name; Value = value;
        }
        public override string ToString()
        {
            // Generates the text shown in the combo box
            return Name;
        }
    }
}