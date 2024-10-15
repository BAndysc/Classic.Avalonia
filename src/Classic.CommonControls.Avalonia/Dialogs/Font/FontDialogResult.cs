using Avalonia.Media;

namespace Classic.CommonControls.Dialogs;

public class FontDialogResult
{
    public FontDialogResult(FontFamily family, FontStyle style, FontWeight weight, double size)
    {
        Family = family;
        Style = style;
        Weight = weight;
        Size = size;
    }

    public FontFamily Family { get; }
    public FontStyle Style { get; }
    public FontWeight Weight { get; }
    public double Size { get; }
}