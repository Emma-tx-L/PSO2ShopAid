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
            AllItems = DataManager.LoadItems();
            RefreshInventory();
            PriceSuffixes = new ObservableCollection<PriceSuffix>((PriceSuffix[])Enum.GetValues(typeof(PriceSuffix)));
            FilterItems(null);
        }

        private ObservableCollection<Item> _filteredItems;
        private ObservableCollection<Item> _filteredInventory;
        private ObservableCollection<Item> _allItems;
        private ObservableCollection<Item> _allInventory;
        private string _searchKeyword;

        public ObservableCollection<Item> AllItems
        {
            get => _allItems;
            set
            {
                _allItems = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AllInventory));
                NotifyPropertyChanged(nameof(FilteredItems));
                NotifyPropertyChanged(nameof(FilteredInventory));
                RefreshInventory();
                FilterItems(_searchKeyword);
            }
        }

        public ObservableCollection<Item> AllInventory
        {
            get => _allInventory;
            set
            {
                _allInventory = value;
                NotifyPropertyChanged();
                FilterItems(_searchKeyword);
            }
        }

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
                FilterItems(_searchKeyword);
            }
        }

        public void RefreshInventory()
        {
            ObservableCollection<Item> inventory = new ObservableCollection<Item>();

            foreach (Item item in AllItems)
            {
                if (item.Stock > 0)
                {
                    inventory.Add(item);
                }
            }

            AllInventory = inventory;
        }

        public void AddNewItem(string nameEN, string colour = null)
        {
            Item newItem;
            if (colour == null) { newItem = new Item(nameEN); }
            else { newItem = new Item(nameEN, colour); }

            foreach (Item item in AllItems)
            {
                if (item.IsSame(newItem))
                {
                    return;
                }
            }

            AllItems.Add(newItem);

            Console.WriteLine($"Added new item");
        }

        public void AddNewItem(string nameEN, Price price, bool isPurchase, string colour = null)
        {
            Item newItem;
            if (colour == null) { newItem = new Item(nameEN); }
            else { newItem = new Item(nameEN, colour); }

            foreach (Item item in AllItems)
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
                    NotifyPropertyChanged(nameof(AllItems));
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

            AllItems.Add(newItem);
        }

        public void RemoveItem(Item toRemove)
        {
            List<Item> removeList = new List<Item>();
            foreach (Item item in AllItems)
            {
                if (item.IsSame(toRemove))
                {
                    removeList.Add(toRemove);
                }
            }

            AllItems = new ObservableCollection<Item>(AllItems.Except(removeList));
            NotifyPropertyChanged(nameof(AllItems));
        }

        public void CompleteRefresh()
        {
            foreach (Item item in AllItems)
            {
                item.NotifyChanged();
            }

            NotifyPropertyChanged(nameof(AllItems));
            RefreshInventory();
        }

        private void FilterItems(string keyword)
        {
            IEnumerable<Item> filteredItems =
                string.IsNullOrWhiteSpace(keyword) ?
                AllItems :
                AllItems.Where(i => i.NameEN.ToLower().Contains(keyword.ToLower()));

            IEnumerable<Item> filteredInventory =
                string.IsNullOrWhiteSpace(keyword) ?
                AllInventory :
                AllInventory.Where(i => i.NameEN.ToLower().Contains(keyword.ToLower()));

            FilteredItems = new ObservableCollection<Item>(filteredItems);
            FilteredInventory = new ObservableCollection<Item>(filteredInventory);
        }
    }
}
