using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace PSO2ShopAid
{
    public partial class ItemWindow : Window
    {
        private static Item item;
        private static MainWindow main;
        public static ObservableCollection<PriceSuffix> PriceSuffixes = new ObservableCollection<PriceSuffix>((PriceSuffix[])Enum.GetValues(typeof(PriceSuffix)));
        public ItemWindow(Item i, MainWindow m)
        {
            InitializeComponent();
            main = m;
            item = i;
            DataContext = item;
            Item_NewPriceSuffix.DataContext = PriceSuffixes;
        }

        protected override void OnClosed(EventArgs e)
        {
            main.OpenItemWindows.Remove(item.NameEN);
            base.OnClosed(e);
        }
    }
}
