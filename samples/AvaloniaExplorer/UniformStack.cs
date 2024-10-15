using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace AvaloniaExplorer;

public class UniformStack : Panel
{
    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<UniformStack, double>(nameof(Spacing));

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    static UniformStack()
    {
        AffectsArrange<UniformStack>(SpacingProperty);
        AffectsMeasure<UniformStack>(SpacingProperty);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var visibleChildren = Children.Count(x => x.IsVisible);

        if (visibleChildren == 0)
            return default;

        var x = 0.0;
        var y = 0.0;

        var width = (finalSize.Width - Math.Max(0, Spacing * (visibleChildren - 1))) / visibleChildren;
        var height = finalSize.Height;

        foreach (var child in Children)
        {
            if (!child.IsVisible)
            {
                continue;
            }

            child.Arrange(new Rect(x, y, width, height));
            x += width;
            x += Spacing;
        }

        return finalSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var visibleChildren = Children.Count(x => x.IsVisible);

        if (visibleChildren == 0)
            return default;

        var maxWidth = 0d;
        var maxHeight = 0d;

        var childAvailableSize = new Size(availableSize.Width / visibleChildren, availableSize.Height);

        foreach (var child in Children)
        {
            child.Measure(childAvailableSize);

            if (child.DesiredSize.Width > maxWidth)
            {
                maxWidth = child.DesiredSize.Width;
            }

            if (child.DesiredSize.Height > maxHeight)
            {
                maxHeight = child.DesiredSize.Height;
            }
        }
        return new Size(maxWidth * visibleChildren + Math.Max(0, Spacing * (visibleChildren - 1)), maxHeight);
    }
}