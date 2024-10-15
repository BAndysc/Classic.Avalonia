using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Classic.Demo;

public class ColorBox : TemplatedControl
{
    public static readonly StyledProperty<string> ColorNameProperty = AvaloniaProperty.Register<ColorBox, string>("ColorName");
    public static readonly StyledProperty<IBrush?> ColorProperty = AvaloniaProperty.Register<ColorBox, IBrush?>("Color");

    public string ColorName
    {
        get { return (string)GetValue(ColorNameProperty); }
        set { SetValue(ColorNameProperty, value); }
    }

    public IBrush? Color
    {
        get { return (IBrush?)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }
}