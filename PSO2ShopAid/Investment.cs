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
        public bool IsSold { get { return SellPrice != null; } }


        public Investment(DateTime date, Price price)
        {
            PurchaseDate = date;
            PurchasePrice = price;
        }

        public Investment(Price price)
        {
            PurchaseDate = DateTime.Now;
            PurchasePrice = price;
        }

        public void Sell(DateTime date, Price price)
        {
            SellDate = date;
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
            if (SellDate == null)
            {
                return new TimeSpan(0);
            }

            return SellDate.Subtract(PurchaseDate);
        }
    }
}
