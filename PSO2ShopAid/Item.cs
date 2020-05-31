using System;
using System.Collections.Generic;

namespace PSO2ShopAid
{
    public class Item
    {
        public string NameEN { get; set; }
        public string NameJP { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<DateTime> RevivalDates { get; set; }
        public List<Investment> Investments { get; }
        public List<Encounter> Encounters { get; }

        public int Stock
        {
            get
            {
                int stock = 0;
                foreach (Investment investment in Investments)
                {
                    if (!investment.IsSold)
                    {
                        stock++;
                    }
                }

                return stock;

            }
        }

        public Price AveragePrice
        {
            get
            {
                return GetAllPriceRecords().Values.Average();
            }
        }

        public Tuple<Price, DateTime> MinPrice
        {
            get
            {
                Price min = new Price(float.MaxValue);
                DateTime date = default;

                foreach (Encounter log in Encounters)
                {
                    if (log.price.RawPrice < min.RawPrice)
                    {
                        min = log.price;
                        date = log.date;
                    }
                }

                return new Tuple<Price, DateTime>(min, date);
            }
        }

        public Tuple<Price, DateTime> MaxPrice
        {
            get
            {
                Price max = new Price(0);
                DateTime date = default;

                foreach (Encounter log in Encounters)
                {
                    if (log.price.RawPrice > max.RawPrice)
                    {
                        max = log.price;
                        date = log.date;
                    }
                }

                return new Tuple<Price, DateTime>(max, date);
            }
        }

        public Tuple<Price, DateTime> LatestPrice
        {
            get
            {
                Price price = new Price(0);
                DateTime date = default;

                foreach (Encounter log in Encounters)
                {
                    if (log.date > date)
                    {
                        price = log.price;
                        date = log.date;
                    }
                }

                return new Tuple<Price, DateTime>(price, date);
            }
        }


        public SortedDictionary<DateTime, Price> GetAllPriceRecords()
        {
            SortedDictionary<DateTime, Price> records = new SortedDictionary<DateTime, Price>();

            foreach (Encounter log in Encounters)
            {
                records.Add(log.date, log.price);
            }

            return records;
        }

        public void Purchase(Price price, DateTime time = default)
        {
            Investment newInvestment = time == default ? new Investment(price) : new Investment(price, time);
            Encounter newEncounter = time == default ? new Encounter(price, newInvestment) : new Encounter(price, time, newInvestment);

            Investments.Add(newInvestment);
            Encounters.Add(newEncounter);
        }

        public void Log(Price price, DateTime time = default)
        {
            Encounter newEncounter = time == default ? new Encounter(price) : new Encounter(price, time);
            Encounters.Add(newEncounter);
        }
    }

    public static class ItemOp
    {
        public static bool IsSame(this Item i1, Item i2)
        {
            return i1.NameEN == i2.NameEN || i1.NameJP == i2.NameJP;
        }
    }
}
