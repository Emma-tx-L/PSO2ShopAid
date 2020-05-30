using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO2ShopAid
{
    public class Price
    {
        private long rawValue;
        private Tuple<long, PriceSuffix> priceK;
        private Tuple<long, PriceSuffix> priceM;

        public Price(long rawNum)
        {
            rawValue = rawNum;
            priceK = GetShortPrice(rawNum, PriceSuffix.k);
            priceM = GetShortPrice(rawNum, PriceSuffix.m);
        }

        public Price(long num, PriceSuffix suffix)
        {
            rawValue = GetRawPrice(num, suffix);
            priceK = GetShortPrice(rawValue, PriceSuffix.k);
            priceM = GetShortPrice(rawValue, PriceSuffix.m);
        }

        public Tuple<long, PriceSuffix> GetShortPrice(long rawNum, PriceSuffix suffix)
        {
            long value = rawNum / 1000;
            return new Tuple<long, PriceSuffix>(value, suffix);
        }

        public long GetRawPrice(long num, PriceSuffix suffix)
        {
            return suffix.Equals(PriceSuffix.k) ? num * 1000 : num * 1000000;
        }
    }

    public enum PriceSuffix
    {
        [Description("thousand")]
        k,
        [Description("million")]
        m
    }
}
