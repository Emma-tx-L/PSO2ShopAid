using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;

namespace PSO2ShopAid
{
    public partial class ItemWindow : Window
    {
        private static Item item;
        private static MainWindow main;
        public static ObservableCollection<PriceSuffix> PriceSuffixes = new ObservableCollection<PriceSuffix>((PriceSuffix[])Enum.GetValues(typeof(PriceSuffix)));
        private int sameActionTimeout = 1000;

        private bool isInitialized = false;
        private bool isLogging = false;
        private bool isBuying = false;
        public bool isSelling = false;

        public ItemWindow(Item i, MainWindow m)
        {
            main = m;
            item = i;
            DataContext = item;
            
            InitializeComponent();

            Item_NewPriceSuffix.DataContext = PriceSuffixes;
            isInitialized = true; // prevent some update events from firing until window is fully initialized
        }

        protected override void OnClosed(EventArgs e)
        {
            main.OpenItemWindows.Remove(item.NameEN);
            base.OnClosed(e);
        }

        private void UpdateReleaseDate(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            item.ReleaseDate = (DateTime)datePicker.SelectedDate;
        }

        private void AddRevivalDate(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized || item == null)
            {
                return;
            }

            DatePicker datePicker = (DatePicker)sender;
            item.AddRevivalDate((DateTime)datePicker.SelectedDate);
        }

        private void ChangeEncounterDate(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized || item == null)
            {
                return;
            }

            DatePicker datePicker = sender as DatePicker;
            Encounter encounter = datePicker.DataContext as Encounter;
            encounter.ChangeDate((DateTime)datePicker.SelectedDate);
        }

        private void ChangeInvestmentBuyDate(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized || item == null)
            {
                return;
            }

            DatePicker datePicker = sender as DatePicker;
            Investment investment = datePicker.DataContext as Investment;
            Encounter encounter = investment.GetLink();
            encounter.ChangeDate((DateTime)datePicker.SelectedDate);
        }

        private void ChangeInvestmentSellDate(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized || item == null)
            {
                return;
            }

            DatePicker datePicker = sender as DatePicker;
            Investment investment = datePicker.DataContext as Investment;

            if (!investment.IsSold) {
                MessageBox.Show("This investment has not been sold yet.");
                return;
            }

            investment.SellDate = (DateTime)datePicker.SelectedDate;
            item.NotifyChanged();
        }

        private void DeleteRevivalDate(object sender, RoutedEventArgs e)
        {
            List<DateTime> toDelete = RevivalsListView.SelectedItems.Cast<DateTime>().ToList();
            item.RemoveRevivalDate(toDelete);
        }


        private void EditTextBox(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.IsReadOnly = false;
            textBox.CaretIndex = textBox.Text.Length;
        }
        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.IsReadOnly = true;
        }

        private void ClearFocus(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }

        private void DeleteEncounter(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Encounter encounter = button.DataContext as Encounter;
            item.RemoveEncounter(encounter);
        }

        private void DeleteInvestment(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Investment investment = button.DataContext as Investment;
            item.RemoveInvestment(investment);
        }

        private void UpdateEncounterPrice(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Encounter encounter = button.DataContext as Encounter;
            TextBox textBox = VisualTreeHelper.GetChild(button.Parent, 1) as TextBox;
            Price price = textBox.Text.ToPrice();
            encounter.ChangePrice(price);
            item.NotifyChanged();
        }

        private void UpdateInvestmentPurchasePrice(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Investment investment = button.DataContext as Investment;
            Encounter encounter = investment.GetLink();
            TextBox textBox = VisualTreeHelper.GetChild(button.Parent, 1) as TextBox;
            Price price = textBox.Text.ToPrice();
            encounter.ChangePrice(price);
            item.NotifyChanged();
        }

        private void UpdateInvestmentSellPrice(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Investment investment = button.DataContext as Investment;
            if (!investment.IsSold)
            {
                MessageBox.Show("This investment has not been sold yet.");
                return;
            }

            TextBox textBox = VisualTreeHelper.GetChild(button.Parent, 1) as TextBox;
            Price price = textBox.Text.ToPrice();
            investment.SellPrice = price;
            item.NotifyChanged();
        }

        private async void LogEncounter(object sender, RoutedEventArgs e)
        {
            if (isLogging)
            {
                return;
            }

            isLogging = true;

            string priceString = Item_NewPrice.Text;
            PriceSuffix suffix = (PriceSuffix)Item_NewPriceSuffix.SelectedItem;

            try
            {
                Price price = priceString.ToPrice(suffix);
                item.Log(price);
            }
            catch
            {
                MessageBox.Show("Please enter a valid price.");
            }

            await Task.Delay(sameActionTimeout);
            isLogging = false;
        }

        private async void Purchase(object sender, RoutedEventArgs e)
        {
            if (isBuying)
            {
                return;
            }

            isBuying = true;

            string priceString = Item_NewPrice.Text;
            PriceSuffix suffix = (PriceSuffix)Item_NewPriceSuffix.SelectedItem;

            try
            {
                Price price = priceString.ToPrice(suffix);
                item.Purchase(price);
            }
            catch
            {
                MessageBox.Show("Please enter a valid price.");
            }

             await Task.Delay(sameActionTimeout);

            isBuying = false;
        }

        private async void Sell(object sender, RoutedEventArgs e)
        {
            if (isSelling)
            {
                return;
            }

            if (item.Stock == 0)
            {
                MessageBox.Show("This item is not in stock.");
                return;
            }

            isSelling = true;

            string priceString = Item_NewPrice.Text;
            PriceSuffix suffix = (PriceSuffix)Item_NewPriceSuffix.SelectedItem;

            try
            {
                Price price = priceString.ToPrice(suffix);
                item.Sell(price);
            }
            catch
            {
                MessageBox.Show("Please enter a valid price.");
            }

            await Task.Delay(sameActionTimeout);
            isSelling = false;
        }
    }
}
