using Newtonsoft.Json;
using System;

namespace PSO2ShopAid
{
    public class Encounter : BaseViewModel, IComparable<Encounter>
    {
        [JsonConstructor]
        public Encounter(Price currPrice, DateTime currDate, bool isPurchase = false, bool sold = false)
        {
            price = currPrice;
            date = currDate;
            IsSell = sold;
            DidPurchase = isPurchase;
            if (isPurchase)
            {
                Purchase = new Investment(currPrice, currDate, this);
            }
        }

        public Encounter(Price currPrice, bool sold = false, bool isPurchase = false)
        {
            price = currPrice;
            date = DateTime.Now;
            IsSell = sold;
            DidPurchase = isPurchase;
            if (isPurchase)
            {
                Purchase = new Investment(currPrice, this);
            }
        }

        private Price _price;
        private DateTime _date;
        private Investment _purchase;
        private bool _didPurchase;

        public Price price
        {
            get => _price;
            set
            {
                _price = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime date
        {
            get => _date;
            set
            {
                _date = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsSell { get; set; }
        public Investment Purchase
        {
            get => _purchase;
            set
            {
                _purchase = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DidPurchase));
            }
        }

        public bool DidPurchase
        {
            get => _didPurchase;
            set
            {
                _didPurchase = value;
                NotifyPropertyChanged();
            }
        }

        public void ChangePrice(Price newPrice)
        {
            price = newPrice;

            if (DidPurchase)
            {
                Purchase.PurchasePrice = newPrice;
            }
        }

        public void ChangeDate(DateTime newDate)
        {
            date = newDate;

            if (DidPurchase)
            {
                Purchase.PurchaseDate = newDate;
            }
        }

        public int CompareTo(Encounter other)
        {
            return other.date.CompareTo(date);
        }
    }
}
