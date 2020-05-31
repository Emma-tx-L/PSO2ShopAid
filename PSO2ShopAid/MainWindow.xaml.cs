using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


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
    }
}
