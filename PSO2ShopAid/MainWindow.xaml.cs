using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PSO2ShopAid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ShopViewModel Shop;

        private bool isAddingItem = false;
        private int addItemTimeout = 2000;

        public List<string> OpenItemWindows = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Shop = new ShopViewModel();
            DataContext = Shop;
        }

        private async void AddNewItem(object sender, RoutedEventArgs e)
        {
            if (isAddingItem)
            {
                return;
            }

            isAddingItem = true;
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

            Inventory.Items.Refresh(); // hackery to force refresh items

            await Task.Delay(addItemTimeout);
            isAddingItem = false;
        }

        private void OpenTrackedItem(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                Item item = (Item)TrackedItems.SelectedItem;

                if (OpenItemWindows.Contains(item.NameEN))
                {
                    return;
                }

                ItemWindow itemWindow = new ItemWindow(item, this);
                OpenItemWindows.Add(item.NameEN);
                itemWindow.Show();
            }
        }

        private void OpenInventoryItem(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                Item item = (Item)Inventory.SelectedItem;

                if (OpenItemWindows.Contains(item.NameEN))
                {
                    return;
                }

                ItemWindow itemWindow = new ItemWindow(item, this);
                OpenItemWindows.Add(item.NameEN);
                itemWindow.Show();
            }
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            Item item = (Item)TrackedItems.SelectedItem;
            Shop.RemoveItem(item);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataManager.SaveItems();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            DataManager.Save();
            base.OnDeactivated(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            Shop.CompleteRefresh();
            base.OnActivated(e);

        }
    }
}
