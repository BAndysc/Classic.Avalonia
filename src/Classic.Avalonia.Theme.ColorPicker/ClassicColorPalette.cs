using Avalonia.Controls;
using Avalonia.Media;

namespace Classic.Avalonia.Theme.ColorPicker;

public class ClassicColorPalette : IColorPalette
{
    public static IColorPalette Instance { get; } = new ClassicColorPalette();

    private static Color[] Colors = new[]
    {
        Color.Parse("#EF8784"),
        Color.Parse("#FFFF92"),
        Color.Parse("#A1FC8F"),
        Color.Parse("#75FB8E"),
        Color.Parse("#A0FCFE"),
        Color.Parse("#367EF7"),
        Color.Parse("#EF87BE"),
        Color.Parse("#EF87F9"),
        Color.Parse("#EA3323"),
        Color.Parse("#FFFF54"),
        Color.Parse("#A1FC4E"),
        Color.Parse("#75FB61"),
        Color.Parse("#74FBFD"),
        Color.Parse("#367EBB"),
        Color.Parse("#8080BB"),
        Color.Parse("#EA34F7"),
        Color.Parse("#784342"),
        Color.Parse("#EF8750"),
        Color.Parse("#75FB4C"),
        Color.Parse("#377E7F"),
        Color.Parse("#173F7C"),
        Color.Parse("#8080F7"),
        Color.Parse("#75143F"),
        Color.Parse("#EA337F"),
        Color.Parse("#75140C"),
        Color.Parse("#EF8733"),
        Color.Parse("#377E22"),
        Color.Parse("#377E47"),
        Color.Parse("#0004F5"),
        Color.Parse("#000299"),
        Color.Parse("#75157C"),
        Color.Parse("#7417F5"),
        Color.Parse("#3A0603"),
        Color.Parse("#784315"),
        Color.Parse("#183F0C"),
        Color.Parse("#183F3F"),
        Color.Parse("#00017B"),
        Color.Parse("#00003D"),
        Color.Parse("#3A063E"),
        Color.Parse("#3A067B"),
        Color.Parse("#000000"),
        Color.Parse("#808026"),
        Color.Parse("#808049"),
        Color.Parse("#808080"),
        Color.Parse("#507F7F"),
        Color.Parse("#C0C0C0"),
        Color.Parse("#3A063E"),
        Color.Parse("#FFFFFF"),
    };

    public Color GetColor(int colorIndex, int shadeIndex)
    {
        return Colors[colorIndex];
    }

    public int ColorCount => Colors.Length;
    public int ShadeCount => 1;
}