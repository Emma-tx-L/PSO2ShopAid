using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PSO2ShopAid
{
    public class ShopViewModel : BaseViewModel
    {
        public ShopViewModel()
        {
            Items = DataManager.LoadItems();
            RefreshInventory();
            PriceSuffixes = new ObservableCollection<PriceSuffix>((PriceSuffix[])Enum.GetValues(typeof(PriceSuffix)));
            FilterItems(null);
        }

        private ObservableCollection<Item> _filteredItems;
        private ObservableCollection<Item> _filteredInventory;
        private string _searchKeyword;

        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }
        public ObservableCollection<PriceSuffix> PriceSuffixes { get; set; }

        public ObservableCollection<Item> FilteredItems
        {
            get => _filteredItems;
            set
            {
                _filteredItems = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Item> FilteredInventory
        {
            get => _filteredInventory;
            set
            {
                _filteredInventory = value;
                NotifyPropertyChanged();
            }
        }

        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                NotifyPropertyChanged(nameof(FilteredItems));
                NotifyPropertyChanged(nameof(FilteredInventory));
                FilterItems(_searchKeyword);;
            }
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

        private void FilterItems(string keyword)
        {
            IEnumerable<Item> filteredItems =
                string.IsNullOrWhiteSpace(keyword) ?
                Items :
                Items.Where(i => i.NameEN.ToLower().Contains(keyword.ToLower()));

            IEnumerable<Item> filteredInventory =
                string.IsNullOrWhiteSpace(keyword) ?
                Inventory :
                Inventory.Where(i => i.NameEN.ToLower().Contains(keyword.ToLower()));

            FilteredItems = new ObservableCollection<Item>(filteredItems);
            FilteredInventory = new ObservableCollection<Item>(filteredInventory);
        }
    }
}
