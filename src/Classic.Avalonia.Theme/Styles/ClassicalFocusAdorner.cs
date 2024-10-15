using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;

namespace Classic.Avalonia.Theme.Controls;

public class ClassicalFocusAdorner : MarkupExtension, ITemplate<Control?>
{
    public double MarginLeft { get; set; }
    public double MarginTop { get; set; }
    public double MarginRight { get; set; }
    public double MarginBottom { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public Control? Build()
    {
        var rect = new Rectangle()
        {
            Margin = new Thickness(MarginLeft, MarginTop, MarginRight, MarginBottom),
            StrokeDashArray = new AvaloniaList<double>() { 1, 1 },
            StrokeThickness = 1,
            [!Shape.StrokeProperty] = new DynamicResourceExtension(SystemColors.ControlTextBrushKey)
        };
        RenderOptions.SetEdgeMode(rect, EdgeMode.Aliased);
        return rect;
    }

    object? ITemplate.Build()
    {
        return Build();
    }
}