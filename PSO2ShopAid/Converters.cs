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

    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSinceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time = (TimeSpan)value;
            if (time.Minutes < 60)
            {
                return $"Updated {time.Minutes} minutes ago";
            }
            else if (time.Hours < 24)
            {
                return $"Updated {time.Hours} hours ago";
            }
            else
            {
                return $"Updated {time.Days} days ago";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
