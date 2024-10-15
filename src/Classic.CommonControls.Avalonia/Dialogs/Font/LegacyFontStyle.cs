using Avalonia.Media;

namespace Classic.CommonControls.Dialogs;

public class LegacyFontStyle
{
    public LegacyFontStyle(FontStyle fontStyle, FontWeight fontWeight)
    {
        FontStyle = fontStyle;
        FontWeight = fontWeight;
        if (FontStyle == FontStyle.Normal)
        {
            Name = FontWeight.ToString();
        }
        else
        {
            if (FontWeight == FontWeight.Normal)
            {
                Name = FontStyle.ToString();
            }
            Name = $"{FontStyle} {FontWeight}";
        }
    }

    public FontStyle FontStyle { get; }
    public FontWeight FontWeight { get; }
    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }
}