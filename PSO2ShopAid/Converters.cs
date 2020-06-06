using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PSO2ShopAid
{
    [ValueConversion(typeof(Price), typeof(string))]
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }

            Price price = (Price)value;
            return price.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string priceString = value as string;
            return priceString.ToPrice();
        }
    }

    [ValueConversion(typeof(Price), typeof(string))]
    public class PriceToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }
            Price price = (Price)value;

            if (price.RawPrice < 0)
            {
                return "#ff4a89";
            }
            else
            {
                return "#66ff69";
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
            if (value == null)
            {
                return "-";
            }
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

    [ValueConversion(typeof(double), typeof(string))]
    public class TooltipPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }
            float price = (float)value;

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
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }
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

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class ShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }
            DateTime date = (DateTime)value;
            return date.ToString("h:mm tt ddd \n d MMM yyyy");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class VeryShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }
            DateTime date = (DateTime)value;
            return date.ToString("MMM d yyyy");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(DateTime))]
    public class DefaultDateToNowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return default;
            }
            DateTime date = (DateTime)value;
            return date.Equals(default) ? DateTime.Now : date;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date;
        }
    }

    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSinceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }
            TimeSpan time = (TimeSpan)value;
            if (time.TotalDays < 0)
            {
                return $"{-time.Days}d, {-time.Hours}h later";
            }

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

    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }
            TimeSpan time = (TimeSpan)value;
            if (time.TotalHours < 1)
            {
                return $"{time.Minutes}min";
            }
            else if (time.TotalDays < 1)
            {
                return $"{time.Hours}h";
            }
            else
            {
                return $"{time.Days}d {time.Hours}h";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class EncounterToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "#ffffff";
            }
            Encounter encounter = (Encounter)value;

            if (encounter.DidPurchase)
            {
                return "#fcf403";
            }

            if (encounter.IsSell)
            {
                return "#fc0d39";
            }

            return "#e6e6e6";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
