using Newtonsoft.Json;
using System;

namespace PSO2ShopAid
{
    public class Investment : BaseViewModel, IComparable<Investment>
    {
        public Investment(Price price, DateTime date, Encounter link)
        {
            PurchaseDate = date;
            PurchasePrice = price;
            LinkedLog = link;
        }

        public Investment(Price price, Encounter link)
        {
            PurchaseDate = DateTime.Now;
            PurchasePrice = price;
            LinkedLog = link;
        }

        [JsonConstructor]
        public Investment(DateTime purchaseDate, Price purchasePrice, DateTime sellDate, Encounter link, Price sellPrice, bool isSold)
        {
            PurchaseDate = purchaseDate;
            PurchasePrice = purchasePrice;
            SellDate = sellDate;
            SellPrice = sellPrice;
            LinkedLog = link;
        }

        private DateTime _purchaseDate;
        private Price _purchasePrice;
        private DateTime _sellDate;
        private Price _sellPrice;
        private Encounter LinkedLog;

        public DateTime PurchaseDate
        {
            get => _purchaseDate;
            set
            {
                _purchaseDate = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DaysToFlip));
            }
        }
        public Price PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Profit));
            }
        }
        public DateTime SellDate
        {
            get => _sellDate;
            set
            {
                _sellDate = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsSold));
                NotifyPropertyChanged(nameof(DaysToFlip));
            }
        }
        public Price SellPrice
        {
            get => _sellPrice;
            set
            {
                _sellPrice = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsSold));
                NotifyPropertyChanged(nameof(Profit));
            }
        }

        public bool IsSold { get { return SellPrice != null && SellDate != null; } }
        public TimeSpan DaysToFlip
        {
            get
            {
                return IsSold ? SellDate.Subtract(PurchaseDate) : DateTime.Now.Subtract(PurchaseDate);
            }

        }

        public Price Profit
        {
            get
            {
                return IsSold ? SellPrice.Subtract(PurchasePrice) : new Price(0);
            }
        }

        public void Sell(Price price, DateTime date)
        {
            SellDate = date;
            SellPrice = price;
            this.NotifyChanged();
        }

        public void Sell(Price price)
        {
            SellDate = DateTime.Now;
            SellPrice = price;
            this.NotifyChanged();
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

        public Encounter GetLink()
        {
            return LinkedLog;
        }

        public int CompareTo(Investment other)
        {
            return other.PurchaseDate.CompareTo(PurchaseDate);
        }
    }
}
