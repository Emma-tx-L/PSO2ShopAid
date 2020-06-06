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
        private int sameActionTimeout = 2000;

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
            if (!isInitialized)
            {
                return;
            }

            DatePicker datePicker = (DatePicker)sender;
            item.AddRevivalDate((DateTime)datePicker.SelectedDate);
        }

        private void ChangeEncounterDate(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized)
            {
                return;
            }

            DatePicker datePicker = sender as DatePicker;
            Encounter encounter = datePicker.DataContext as Encounter;
            encounter.ChangeDate((DateTime)datePicker.SelectedDate);

            MessageBox.Show($"Updated Date to {datePicker.SelectedDate}");
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

        private void UpdateEncounterPrice(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Encounter encounter = button.DataContext as Encounter;
            TextBox textBox = VisualTreeHelper.GetChild(button.Parent, 1) as TextBox;
            Price price = textBox.Text.ToPrice();
            encounter.ChangePrice(price);
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
            float rawPrice;

            try
            {
                rawPrice = float.Parse(priceString);
            }
            catch
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            PriceSuffix suffix = (PriceSuffix)Item_NewPriceSuffix.SelectedItem;
            Price price = new Price(rawPrice, suffix);

            item.Log(price);

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
            float rawPrice;

            try
            {
                rawPrice = float.Parse(priceString);
            }
            catch
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            PriceSuffix suffix = (PriceSuffix)Item_NewPriceSuffix.SelectedItem;
            Price price = new Price(rawPrice, suffix);

            item.Purchase(price);

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
            float rawPrice;

            try
            {
                rawPrice = float.Parse(priceString);
            }
            catch
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            PriceSuffix suffix = (PriceSuffix)Item_NewPriceSuffix.SelectedItem;
            Price price = new Price(rawPrice, suffix);

            item.Sell(price);

            await Task.Delay(sameActionTimeout);
            isSelling = false;
        }
    }
}
