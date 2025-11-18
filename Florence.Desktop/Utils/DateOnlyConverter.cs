using System;
using System.Globalization;
using System.Windows.Data;

namespace Florence.Desktop.Utils
{
    public class DateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is DateOnly d ? d.ToDateTime(TimeOnly.MinValue) : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is DateTime dt ? DateOnly.FromDateTime(dt) : DateOnly.FromDateTime(DateTime.Now);
    }
}
