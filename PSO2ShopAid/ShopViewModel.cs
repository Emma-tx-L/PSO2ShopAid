using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PSO2ShopAid
{
    public class ShopViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }

        public ShopViewModel()
        {
            Items = DataManager.LoadItems();
            RefreshInventory();
        }

        public void RefreshInventory()
        {
            Inventory = new ObservableCollection<Item>();

            foreach (Item item in Items)
            {
                if (item.Stock > 0)
                {
                    Inventory.Add(item);
                }
            }
        }

    }
}
