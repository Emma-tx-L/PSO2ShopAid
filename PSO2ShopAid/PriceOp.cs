using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO2ShopAid
{
    public static class PriceOp
    {
        public static Price Subtract(this Price a, Price b)
        {
            return new Price(a.RawPrice - b.RawPrice);
        }

        public static Price Add(this Price a, Price b)
        {
            return new Price(a.RawPrice + b.RawPrice);
        }

        public static float PercentChange(this Price curr, Price orig)
        {
            return orig.RawPrice == 0 ? 0 : (curr.RawPrice - orig.RawPrice) / orig.RawPrice * 100;
        }

        public static float PercentOf(this Price curr, Price orig)
        {
            return orig.RawPrice == 0 ? 0 : curr.RawPrice / orig.RawPrice * 100;
        }

        public static Price Average(this IEnumerable<Price> prices)
        {
            float count = prices.Count();
            if (count == 0)
            {
                return new Price(0);
            }

            float sum = 0;
            foreach (Price price in prices)
            {
                sum += price.RawPrice;
            }

            return new Price(sum / count);
        }
    }
}
