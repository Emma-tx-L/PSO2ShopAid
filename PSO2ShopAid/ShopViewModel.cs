using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DrWPF.Windows.Data;

namespace PSO2ShopAid
{
    public class ShopViewModel : BaseViewModel
    {
        public ObservableDictionary<ItemName, Item> Items { get; set; }
        public ObservableDictionary<ItemName, Item> Inventory { get; set; }

        public ShopViewModel()
        {
            Items = DataManager.LoadItems();
            RefreshInventory();
        }

        public void RefreshInventory()
        {
            Inventory = new ObservableDictionary<ItemName, Item>();

            foreach (Item item in Items)
            {
                if (item.Stock > 0)
                {
                    Inventory.Add(item.name, item);
                }
            }
        }

    }
}
