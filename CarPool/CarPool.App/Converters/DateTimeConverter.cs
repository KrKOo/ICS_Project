using System;
using System.Globalization;
using System.Windows.Data;

namespace CarPool.App.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime)value;
            if (dateTime == DateTime.MinValue)
            {
                return "";
            }
            return dateTime.ToString("dd/MM/yyyy HH:mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return DateTime.Parse((string)value);
            }
            catch(FormatException)
            {
                return default(DateTime);
            }
        }
    }
}
