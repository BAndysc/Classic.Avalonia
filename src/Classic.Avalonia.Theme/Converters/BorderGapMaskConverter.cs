using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Classic.Avalonia.Theme.Converters;

internal class BorderGapMaskConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        //
        // Parameter Validation
        //

        Type doubleType = typeof(double);

        if (parameter == null ||
            values == null ||
            values.Count != 3 ||
            values[0] == null ||
            values[1] == null ||
            values[2] == null ||
            !doubleType.IsInstanceOfType(values[0]) ||
            !doubleType.IsInstanceOfType(values[1]) ||
            !doubleType.IsInstanceOfType(values[2]) )
        {
            return AvaloniaProperty.UnsetValue;
        }

        Type paramType = parameter.GetType();
        if (!(doubleType.IsAssignableFrom(paramType) || typeof(string).IsAssignableFrom(paramType)))
        {
            return AvaloniaProperty.UnsetValue;
        }

        //
        // Conversion
        //

        double headerWidth = (double?)values[0] ?? 0;
        double borderWidth = (double?)values[1] ?? 0;
        double borderHeight = (double?)values[2] ?? 0;

        // Doesn't make sense to have a Grid
        // with 0 as width or height
        if (borderWidth == 0
            || borderHeight == 0)
        {
            return null;
        }

        // Width of the line to the left of the header
        // to be used to set the width of the first column of the Grid
        double lineWidth;
        if (parameter is string)
        {
            lineWidth = Double.Parse(((string)parameter), NumberFormatInfo.InvariantInfo);
        }
        else
        {
            lineWidth = (double)parameter;
        }

        Grid grid = new Grid();
        grid.Width = borderWidth;
        grid.Height = borderHeight;
        ColumnDefinition colDef1 = new ColumnDefinition();
        ColumnDefinition colDef2 = new ColumnDefinition();
        ColumnDefinition colDef3 = new ColumnDefinition();
        colDef1.Width = new GridLength(lineWidth);
        colDef2.Width = new GridLength(headerWidth);
        colDef3.Width = new GridLength(1, GridUnitType.Star);
        grid.ColumnDefinitions.Add(colDef1);
        grid.ColumnDefinitions.Add(colDef2);
        grid.ColumnDefinitions.Add(colDef3);
        RowDefinition rowDef1 = new RowDefinition();
        RowDefinition rowDef2 = new RowDefinition();
        rowDef1.Height = new GridLength(borderHeight / 2);
        rowDef2.Height = new GridLength(1, GridUnitType.Star);
        grid.RowDefinitions.Add(rowDef1);
        grid.RowDefinitions.Add(rowDef2);

        Rectangle rectColumn1 = new Rectangle();
        Rectangle rectColumn2 = new Rectangle();
        Rectangle rectColumn3 = new Rectangle();
        rectColumn1.Fill = Brushes.Black;
        rectColumn2.Fill = Brushes.Black;
        rectColumn3.Fill = Brushes.Black;

        Grid.SetRowSpan(rectColumn1, 2);
        Grid.SetRow(rectColumn1, 0);
        Grid.SetColumn(rectColumn1, 0);

        Grid.SetRow(rectColumn2, 1);
        Grid.SetColumn(rectColumn2, 1);

        Grid.SetRowSpan(rectColumn3, 2);
        Grid.SetRow(rectColumn3, 0);
        Grid.SetColumn(rectColumn3, 2);

        grid.Children.Add(rectColumn1);
        grid.Children.Add(rectColumn2);
        grid.Children.Add(rectColumn3);

        return (new VisualBrush(grid));
    }
}