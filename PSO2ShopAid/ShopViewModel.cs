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
        public ObservableCollection<PriceSuffix> PriceSuffixes { get; set; }

        public ShopViewModel()
        {
            Items = DataManager.LoadItems();
            RefreshInventory();
            PriceSuffixes = new ObservableCollection<PriceSuffix>((PriceSuffix[])Enum.GetValues(typeof(PriceSuffix)));
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

            NotifyPropertyChanged(nameof(Inventory));
        }

        public void AddNewItem(string nameEN, string colour = null)
        {
            Item newItem;
            if (colour == null) { newItem = new Item(nameEN); }
            else { newItem = new Item(nameEN, colour); }

            foreach (Item item in Items)
            {
                if (item.IsSame(newItem))
                {
                    return;
                }
            }

            Items.Add(newItem);

            Console.WriteLine($"Added new item");
        }

        public void AddNewItem(string nameEN, Price price, bool isPurchase, string colour = null)
        {
            Item newItem;
            if (colour == null) { newItem = new Item(nameEN); }
            else { newItem = new Item(nameEN, colour); }

            foreach (Item item in Items)
            {
                if (item.IsSame(newItem))
                {
                    item.Log(price);
                    if (isPurchase)
                    {
                        item.Purchase(price);
                        RefreshInventory();
                    }
                    else
                    {
                        item.Log(price);
                    }

                    item.NotifyChanged();
                    NotifyPropertyChanged(nameof(Items));
                    return;
                }
            }

            newItem.Log(price);
            if (isPurchase)
            { 
                newItem.Purchase(price);
                RefreshInventory();
            } 
            else
            {
                newItem.Log(price);
            }

            Items.Add(newItem);
        }

        public void RemoveItem(Item toRemove)
        {
            List<Item> removeList = new List<Item>();
            foreach (Item item in Items)
            {
                if (item.IsSame(toRemove))
                {
                    removeList.Add(toRemove);
                }
            }

            Items = new ObservableCollection<Item>(Items.Except(removeList));
            NotifyPropertyChanged(nameof(Items));
        }

        public void CompleteRefresh()
        {
            foreach (Item item in Items)
            {
                item.NotifyChanged();
            }

            NotifyPropertyChanged(nameof(Items));
            RefreshInventory();
        }
    }
}
