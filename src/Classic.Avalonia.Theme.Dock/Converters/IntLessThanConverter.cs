using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Classic.Avalonia.Theme.Dock;

internal class IntLessThanConverter : IValueConverter
{
    public int TrueIfLessThan { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue < TrueIfLessThan;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
