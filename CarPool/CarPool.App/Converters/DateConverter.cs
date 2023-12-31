﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CarPool.App.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((DateOnly)value == DateOnly.MinValue)
            {
                return "";
            }
            return value.ToString() ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return DateOnly.Parse((string)value);
            }
            catch (FormatException)
            {
                return default(DateOnly);
            }
        }
    }
}

