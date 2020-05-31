using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO2ShopAid
{
    public class Investment
    {
        public DateTime PurchaseDate { get; set; }
        public Price PurchasePrice { get; set; }
        public DateTime SellDate { get; set; }
        public Price SellPrice { get; set; }
        public bool IsSold { get { return SellPrice != null && SellDate != null; } }


        public Investment(Price price, DateTime date)
        {
            PurchaseDate = date;
            PurchasePrice = price;
        }

        public Investment(Price price)
        {
            PurchaseDate = DateTime.Now;
            PurchasePrice = price;
        }

        [JsonConstructor]
        public Investment(DateTime purchaseDate, Price purchasePrice, DateTime sellDate, Price sellPrice, bool isSold)
        {
            PurchaseDate = purchaseDate;
            PurchasePrice = purchasePrice;
            SellDate = sellDate;
            SellPrice = sellPrice;
        }

        public void Sell(Price price, DateTime date)
        {
            SellDate = date;
            SellPrice = price;
        }

        public void Sell(Price price)
        {
            SellDate = DateTime.Now;
            SellPrice = price;
        }

        public Price NetGain()
        {
            if (!IsSold)
            {
                return new Price(0);
            }

            return SellPrice.Subtract(PurchasePrice);
        }

        public float PercentGain()
        {
            if (!IsSold)
            {
                return 0;
            }

            return SellPrice.PercentChange(PurchasePrice);
        } 

        public TimeSpan FlipTime()
        {
            if (!IsSold)
            {
                return new TimeSpan(0);
            }

            return SellDate.Subtract(PurchaseDate);
        }
    }
}
