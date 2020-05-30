using System;

namespace PSO2ShopAid
{
    public class Encounter
    {
        public Price price { get; set; }
        public DateTime date { get; set; }
        public bool DidPurchase { get; set; }

        public Encounter(Price currPrice, DateTime currDate, bool purchased = false)
        {
            price = currPrice;
            date = currDate;
            DidPurchase = purchased;
        }

        public Encounter(Price currPrice, bool purchased = false)
        {
            price = currPrice;
            date = DateTime.Now;
            DidPurchase = purchased;
        }
    }
}
