using System;
using System.Globalization;
using System.Windows.Data;

namespace CarPool.App.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;
            if (timeSpan == TimeSpan.MinValue)
            {
                return "";
            }
            return timeSpan.ToString(@"hh\:mm");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return TimeSpan.Parse((string)value);
            }
            catch (FormatException)
            {
                return default(TimeSpan);
            }
        }
    }
}

