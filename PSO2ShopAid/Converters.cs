using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PSO2ShopAid
{
    [ValueConversion(typeof(Price), typeof(string))]
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Price price = (Price)value;

            if (price.RawPrice < 1000000)
            {
                return $"{price.priceK}k";
            }
            else
            {
                return $"{price.priceM}m";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(string))]
    public class RawPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double price = (double)value;

            if (price < 1000000)
            {
                return $"{Math.Round(price / 1000, 1)}k";
            }
            else
            {
                return $"{Math.Round(price / 1000000, 1)}m";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateAxisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            TimeSpan timeSince = DateTime.Now.Subtract(date);

            if (timeSince.TotalHours < 1)
            {
                return $"{timeSince.Minutes}m";
            }

            if (timeSince.TotalDays < 1)
            {
                return $"{timeSince.Hours}h";
            }

            return $"{Math.Round(timeSince.TotalDays)}d";

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSinceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time = (TimeSpan)value;
            if (time.TotalHours < 1)
            {
                return $"Updated {time.Minutes} minutes ago";
            }
            else if (time.TotalDays < 1)
            {
                return $"Updated {time.Hours} hours ago";
            }
            else
            {
                return $"Updated {time.Days}d {time.Hours}h ago";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class DidPurchaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool didPurchase = (bool)value;
            if (didPurchase)
            {
                return "#fcf403";
            }

            return "#e6e6e6";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
