using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            item.NotifyPropertyChanged(nameof(item.ReleaseDate));
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

        private void DeleteRevivalDate(object sender, RoutedEventArgs e)
        {
            
            
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
