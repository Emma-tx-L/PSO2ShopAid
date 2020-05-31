using System;
using System.Collections.Generic;

namespace PSO2ShopAid
{
    public class Item
    {
        public ItemName name { get; set; }
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

        public Tuple<Price, DateTime> LatestEncounter
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

        public Price LatestPrice
        {
            get
            {
                return LatestEncounter.Item1;
            }
        }

        public TimeSpan TimeSinceUpdate
        {
            get
            {
                if (LatestEncounter.Item2 == default)
                {
                    return new TimeSpan(0);
                }

                return DateTime.Now.Subtract(LatestEncounter.Item2);
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
            Investment newInvestment = time.Equals(default) ? new Investment(price) : new Investment(price, time);
            Encounter newEncounter = time.Equals(default) ? new Encounter(price, newInvestment) : new Encounter(price, time, newInvestment);

            Investments.Add(newInvestment);
            Encounters.Add(newEncounter);
        }

        public void Log(Price price, DateTime time = default)
        {
            Encounter newEncounter = time.Equals(default) ? new Encounter(price) : new Encounter(price, time);
            Encounters.Add(newEncounter);
        }
    }

    public class ItemName
    {
        public string NameEN { get; set; }
        public string NameJP { get; set; }

        public ItemName(string name, Language language)
        {
            if (language.Equals(Language.EN))
            {
                NameEN = name;
            }
            else
            {
                NameJP = name;
            }
        }
    }

    public enum Language
    {
        EN,
        JP
    }
}
