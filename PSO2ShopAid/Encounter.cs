using Newtonsoft.Json;
using System;

namespace PSO2ShopAid
{
    public class Encounter : BaseViewModel, IComparable<Encounter>
    {
        public Encounter(Price currPrice, DateTime currDate, bool sold = false, Investment purchase = null)
        {
            price = currPrice;
            date = currDate;
            IsSell = sold;
            Purchase = purchase;
        }

        public Encounter(Price currPrice, bool sold = false, Investment purchase = null)
        {
            price = currPrice;
            date = DateTime.Now;
            IsSell = sold;
            Purchase = purchase;
        }

        [JsonConstructor]
        public Encounter(Price p, DateTime d, Investment purchase, bool didPurchase, bool sold)
        {
            price = p;
            date = d;
            Purchase = purchase;
            IsSell = sold;
        }

        private Price _price;
        private DateTime _date;
        private Investment _purchase;

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
        public bool DidPurchase { get { return Purchase != null; } }

        public int CompareTo(Encounter other)
        {
            return other.date.CompareTo(date);
        }
    }
}
