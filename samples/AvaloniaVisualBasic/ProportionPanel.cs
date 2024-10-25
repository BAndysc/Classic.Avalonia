using System;
using Avalonia;
using Avalonia.Controls;

namespace AvaloniaVisualBasic;

public class ProportionPanel : Panel
{
    public static readonly StyledProperty<double> ProportionProperty = AvaloniaProperty.Register<ProportionPanel, double>("Proportion");

    protected override Size ArrangeOverride(Size finalSize)
    {
        var width = finalSize.Width;
        var height = finalSize.Height;

        var proportionalWidth = Proportion * height;
        var actualWidth = Math.Min(proportionalWidth, width);
        var actualHeight = actualWidth / Proportion;

        foreach (var child in Children)
        {
            child.Arrange(new Rect(0, 0, actualWidth, actualHeight));
        }

        return new Size(actualWidth, actualHeight);
    }

    public double Proportion
    {
        get { return (double)GetValue(ProportionProperty); }
        set { SetValue(ProportionProperty, value); }
    }
}