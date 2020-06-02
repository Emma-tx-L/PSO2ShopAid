using Newtonsoft.Json;
using System;

namespace PSO2ShopAid
{
    public class Encounter : IComparable<Encounter>
    {
        public Price price { get; set; }
        public DateTime date { get; set; }
        public bool IsSell { get; set; }
        public Investment Purchase { get; set; }
        public bool DidPurchase { get { return Purchase != null; } }

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

        public int CompareTo(Encounter other)
        {
            return other.date.CompareTo(date);
        }
    }
}
