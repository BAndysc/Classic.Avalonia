using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Classic.Avalonia.Theme.Converters;

internal class DateTimeToStringConverter : IValueConverter
{
    public string? Prefix { get; set; } = "Today: ";
    public string? Suffix { get; set; }
    public string? Format { get; set; } = "yyyy-MM-dd";

    public static DateTimeToStringConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            var formatted = dateTime.ToString(Format, culture);
            return $"{Prefix}{formatted}{Suffix}";
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static string TodaysDateForCalendar => $"Today: {DateTime.Now:yyyy-MM-dd}";
}