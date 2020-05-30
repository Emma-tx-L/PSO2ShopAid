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
        public float RawPrice { get; }
        private Tuple<float, PriceSuffix> priceK;
        private Tuple<float, PriceSuffix> priceM;

        public Price(float rawNum)
        {
            RawPrice = rawNum;
            priceK = GetShortPrice(rawNum, PriceSuffix.k);
            priceM = GetShortPrice(rawNum, PriceSuffix.m);
        }

        public Price(float num, PriceSuffix suffix)
        {
            RawPrice = GetRawPrice(num, suffix);
            priceK = GetShortPrice(RawPrice, PriceSuffix.k);
            priceM = GetShortPrice(RawPrice, PriceSuffix.m);
        }

        private Tuple<float, PriceSuffix> GetShortPrice(float rawNum, PriceSuffix suffix)
        {
            float value = rawNum / 1000;
            return new Tuple<float, PriceSuffix>(value, suffix);
        }

        private float GetRawPrice(float num, PriceSuffix suffix)
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
