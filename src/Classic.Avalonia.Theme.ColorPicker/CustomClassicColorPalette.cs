using Avalonia.Media;

namespace Classic.Avalonia.Theme.ColorPicker;

internal class CustomClassicColorPalette : List<Color>
{
    public static CustomClassicColorPalette Instance { get; } = new CustomClassicColorPalette();

    public CustomClassicColorPalette()
    {
        for (int i = 0; i < 16; ++i)
            Add(Colors.White);
    }
}