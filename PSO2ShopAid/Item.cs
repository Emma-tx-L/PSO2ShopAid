using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PSO2ShopAid
{
    public class Item : BaseViewModel
    {
        public string NameEN { get; set; }
        public string NameJP { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<DateTime> RevivalDates { get; set; }
        public List<Investment> Investments { get; set; }
        public List<Encounter> Encounters { get; set; }
        public string Colour { get; set; }

        public Item(string nameEN)
        {
            NameEN = nameEN;
            RevivalDates = new List<DateTime>();
            Investments = new List<Investment>();
            Encounters = new List<Encounter>();
            Colour = ColourPicker.GetRandomColour();
        }

        public Item(string nameEN, string colour)
        {
            NameEN = nameEN;
            RevivalDates = new List<DateTime>();
            Investments = new List<Investment>();
            Encounters = new List<Encounter>();
            Colour = colour;
            ColourPicker.AddColour(colour);
        }

        [JsonConstructor]
        public Item(string nameEN, string nameJP, DateTime release, List<DateTime> rev, List<Investment> investments, List<Encounter> encounters, string col)
        {
            NameEN = nameEN;
            NameJP = nameJP;
            ReleaseDate = release;
            RevivalDates = rev;
            Investments = investments;
            Encounters = encounters;
            Colour = col;
        }

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
                return GetAllPriceRecords().Average();
            }
        }

        public Price AveragePurchasePrice
        {
            get
            {
                List<Price> unsoldPrices = GetUnsoldPurchasePriceRecords();
                return unsoldPrices.Count == 0 ? new Price(0) : unsoldPrices.Average();
            }
        }

        public Tuple<Price, DateTime> MinPrice
        {
            get
            {
                if (Encounters.Count == 0)
                {
                    return new Tuple<Price, DateTime>(new Price(0), default);
                }

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

        public Price TotalProfit
        {
            get
            {
                float total = 0;
                foreach (Investment investment in Investments)
                {
                    if (investment.IsSold)
                    {
                        total += investment.SellPrice.Subtract(investment.PurchasePrice).RawPrice;
                    }
                }

                return new Price(0);
            }
        }

        public TimeSpan TimeSinceLastPurchase
        {
            get
            {
                if (Investments.Count == 0)
                {
                    return new TimeSpan(0);
                }

                DateTime date = default;
                foreach (Investment investment in Investments)
                {
                    if (investment.PurchaseDate > date)
                    {
                        date = investment.PurchaseDate;
                    }
                }

                return DateTime.Now.Subtract(date);
            }
        }

        public Price ApproximateCurrentProfit
        {
            get
            {
                return AveragePurchasePrice.RawPrice == 0 ? AveragePurchasePrice : LatestPrice.Subtract(AveragePurchasePrice);
            }
        }

        public float ApproximateCurrentProfitPercent
        {
            get
            {
                return (float)Math.Round(LatestPrice.PercentOf(AveragePurchasePrice));
            }
        }

        public TimeSpan TimeSinceLastAvailable
        {
            get
            {
                if (RevivalDates.Count > 0)
                {
                    DateTime recent = default;
                    foreach (DateTime date in RevivalDates)
                    {
                        if (date > recent)
                            recent = date;
                    }

                    return DateTime.Now.Subtract(recent);
                }
                else if (ReleaseDate != null && !ReleaseDate.Equals(default))
                {
                    return DateTime.Now.Subtract(ReleaseDate);
                }
                else
                {
                    return new TimeSpan(0);
                }
            }
        }
            

        public List<Price> GetAllPriceRecords()
        {
            List<Price> records = new List<Price>();

            foreach (Encounter log in Encounters)
            {
                records.Add(log.price);
            }

            return records;
        }

        public List<Price> GetUnsoldPurchasePriceRecords()
        {
            List<Price> records = new List<Price>();

            foreach (Investment investment in Investments)
            {
                if (!investment.IsSold)
                    records.Add(investment.PurchasePrice);
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

        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this);
            }
            catch { return base.ToString(); }

        }
    }

    public static class ItemOp
    {
        public static bool IsSame(this Item i1, Item i2)
        {
            return i1.NameEN == i2.NameEN;
        }

        public static void NotifyChanged(this Item item)
        {
            PropertyInfo[] properties = typeof(Item).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                item.NotifyPropertyChanged(property.Name);
            }
        }
    }
}
