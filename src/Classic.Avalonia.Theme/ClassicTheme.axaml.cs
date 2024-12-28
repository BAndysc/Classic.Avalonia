using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Avalonia.Styling;
using Classic.CommonControls;

[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Classic.Avalonia.Theme")]

namespace Classic.Avalonia.Theme;

public class ClassicTheme : Styles
{
    public static readonly StyledProperty<bool> FontAliasingProperty = AvaloniaProperty.Register<ClassicTheme, bool>(nameof(FontAliasing), defaultValue: true);
    public static ThemeVariant Standard { get; } = new("Standard", ThemeVariant.Light);
    public static ThemeVariant Classic { get; } = new("Classic", ThemeVariant.Light);
    public static ThemeVariant Brick { get; } = new("Brick", ThemeVariant.Light);
    public static ThemeVariant Wheat { get; } = new("Wheat", ThemeVariant.Light);
    public static ThemeVariant Marine { get; } = new("Marine", ThemeVariant.Light);
    public static ThemeVariant Sprouce { get; } = new("Sprouce", ThemeVariant.Light);
    public static ThemeVariant Plum { get; } = new("Plum", ThemeVariant.Light);
    public static ThemeVariant Rose { get; } = new("Rose", ThemeVariant.Light);
    public static ThemeVariant Storm { get; } = new("Storm", ThemeVariant.Light);
    public static ThemeVariant Desert { get; } = new("Desert", ThemeVariant.Light);
    public static ThemeVariant Eggplant { get; } = new("Eggplant", ThemeVariant.Light);
    public static ThemeVariant StarsAndStripes { get; } = new("StarsAndStripes", ThemeVariant.Light);
    public static ThemeVariant Pumpkin { get; } = new("Pumpkin", ThemeVariant.Light);

    public static IReadOnlyList<ThemeVariant> AllVariants { get; } = [Standard, Classic, Brick, Wheat, Marine, Sprouce, Plum, Rose, Storm, Desert, Eggplant, StarsAndStripes, Pumpkin];

    static ClassicTheme()
    {
        static void UpdateRangeClasses(RangeBase bar)
        {
            bar.Classes.Set("__classic_theme_is_empty", bar.Minimum >= bar.Maximum);
        }

        RangeBase.ValueProperty.Changed.AddClassHandler<RangeBase>((bar, _) =>
        {
            UpdateRangeClasses(bar);
        });
        Control.LoadedEvent.AddClassHandler<RangeBase>((bar, _) =>
        {
            UpdateRangeClasses(bar);
        });
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Control.LoadedEvent.AddClassHandler<Window>((window, _) =>
            {
                window.Classes.Add("__classic_theme_is_mac");
            });
        }

        FontAliasingProperty.Changed.AddClassHandler<ClassicTheme>((theme, e) =>
        {
            theme.Resources[SystemParameters.FontAliasingKey] = e.GetNewValue<bool>();
        });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClassicTheme"/> class.
    /// </summary>
    /// <param name="sp">The parent's service provider.</param>
    public ClassicTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }

    public bool FontAliasing
    {
        get => GetValue(FontAliasingProperty);
        set => SetValue(FontAliasingProperty, value);
    }
}
