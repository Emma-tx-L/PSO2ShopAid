using System;

namespace PSO2ShopAid
{
    public class Encounter
    {
        public Price price { get; set; }
        public DateTime date { get; set; }
        public Investment Purchase { get; }
        public bool DidPurchase { get { return Purchase != null; } }

        public Encounter(Price currPrice, DateTime currDate, Investment purchase = null)
        {
            price = currPrice;
            date = currDate;
            Purchase = purchase;
        }

        public Encounter(Price currPrice, Investment purchase = null)
        {
            price = currPrice;
            date = DateTime.Now;
            Purchase = purchase;
        }
    }
}
