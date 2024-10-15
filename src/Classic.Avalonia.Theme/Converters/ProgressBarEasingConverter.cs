using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Classic.Avalonia.Theme.Utils;

namespace Classic.Avalonia.Theme.Converters;

public class ProgressBarEasingConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new StepEasing();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static ProgressBarEasingConverter Instance { get; } = new ProgressBarEasingConverter();
}