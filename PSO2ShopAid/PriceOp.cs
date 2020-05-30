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
            return (curr.RawPrice - orig.RawPrice) / orig.RawPrice * 100;
        }
    }
}
