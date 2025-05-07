using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Classic.Avalonia.Theme.Dock.Utils;

internal class FontUtils
{
    public static readonly AttachedProperty<bool?> FontAliasingProperty = AvaloniaProperty.RegisterAttached<FontUtils, Control, bool?>("FontAliasing");

    public static bool? GetFontAliasing(AvaloniaObject element) => element.GetValue(FontAliasingProperty);

    public static void SetFontAliasing(AvaloniaObject element, bool? value) => element.SetValue(FontAliasingProperty, value);

    static FontUtils()
    {
        FontAliasingProperty.Changed.AddClassHandler<Control>((c, e) =>
        {
            RenderOptions.SetTextRenderingMode(c, e.GetNewValue<bool?>() is true ? TextRenderingMode.Alias : TextRenderingMode.SubpixelAntialias);
        });
    }
}