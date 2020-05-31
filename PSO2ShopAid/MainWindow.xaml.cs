using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PSO2ShopAid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ShopViewModel Shop;

        public MainWindow()
        {
            InitializeComponent();
            Shop = new ShopViewModel();
            DataContext = Shop;
        }

        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            string name = NewItem_Name.Text;
            string priceString = NewItem_Price.Text;
            PriceSuffix suffix = (PriceSuffix)NewItem_PriceSuffix.SelectedItem;
            Color? colour = NewItem_Colour.SelectedColor;
            string hex = colour?.ToString();
            bool isPurchase = (bool)NewItem_IsPurchase.IsChecked;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Item name can't be empty.");
                return;
            }

            if (string.IsNullOrEmpty(priceString))
            {
                Shop.AddNewItem(name, hex);
            }

            else
            {
                try
                {
                    Price price = new Price(float.Parse(priceString), suffix);
                    Shop.AddNewItem(name, price, isPurchase, hex);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Please enter a valid price.");
                    Console.WriteLine(err);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataManager.SaveItems();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            DataManager.SaveItems();
        }
    }
}
