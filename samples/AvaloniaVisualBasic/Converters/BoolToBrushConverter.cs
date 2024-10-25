using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AvaloniaVisualBasic.Converters;

public class BoolToBrushConverter : AvaloniaObject, IValueConverter
{
    public static readonly StyledProperty<IBrush?> InactiveBrushProperty = AvaloniaProperty.Register<BoolToBrushConverter, IBrush?>("InactiveBrush");
    public static readonly StyledProperty<IBrush?> ActiveBrushProperty = AvaloniaProperty.Register<BoolToBrushConverter, IBrush?>("ActiveBrush");

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
            return ActiveBrush;
        return InactiveBrush;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }

    public IBrush? ActiveBrush
    {
        get { return (IBrush?)GetValue(ActiveBrushProperty); }
        set { SetValue(ActiveBrushProperty, value); }
    }

    public IBrush? InactiveBrush
    {
        get { return (IBrush?)GetValue(InactiveBrushProperty); }
        set { SetValue(InactiveBrushProperty, value); }
    }
}