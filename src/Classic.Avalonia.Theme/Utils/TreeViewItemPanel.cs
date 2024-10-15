using System;
using Avalonia;
using Avalonia.Controls;

namespace Classic.Avalonia.Theme.Utils;

internal class TreeViewItemPanel : Panel
{
    protected override Size ArrangeOverride(Size finalSize)
    {
        return base.ArrangeOverride(finalSize);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double width = 0.0;
        double height = 0.0;
        foreach (var control in Children)
        {
            if (control is DiagonalLine)
                continue;
            control.Measure(availableSize);
            width = Math.Max(width, control.DesiredSize.Width);
            height = Math.Max(height, control.DesiredSize.Height);
        }
        return new Size(width, height);
    }
}