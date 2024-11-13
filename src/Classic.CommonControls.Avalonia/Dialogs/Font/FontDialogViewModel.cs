using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media;

namespace Classic.CommonControls.Dialogs;

public class FontDialogViewModel : INotifyPropertyChanged
{
    public List<FontFamily> Fonts { get; }
    public ObservableCollection<LegacyFontStyle> FontStyles { get; } = new();
    public List<double> FontSizes { get; } = new() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

    private FontFamily? selectedFont;
    public FontFamily? SelectedFont
    {
        get => selectedFont;
        set
        {
            SetField(ref selectedFont, value);
            SetField(ref fontNameText, value?.Name, nameof(FontNameText));
            UpdateFontStyles();
        }
    }

    private LegacyFontStyle? selectedFontStyle;
    public LegacyFontStyle? SelectedFontStyle
    {
        get => selectedFontStyle;
        set => SetField(ref selectedFontStyle, value);
    }

    private double selectedFontSize;
    public double SelectedFontSize
    {
        get => selectedFontSize;
        set => SetField(ref selectedFontSize, value);
    }

    private string? fontNameText;
    public string? FontNameText
    {
        get => fontNameText;
        set
        {
            SetField(ref fontNameText, value);
            if (Fonts.FirstOrDefault(font => font.Name.Equals(value, StringComparison.OrdinalIgnoreCase)) is { } foundFont)
            {
                SelectedFont = foundFont;
            }
        }
    }

    public event Action<FontDialogResult>? AcceptRequested;
    public event Action? CancelRequested;

    public FontDialogViewModel()
    {
        Fonts = FontManager.Current.SystemFonts.ToList();
        selectedFont = Fonts[0];
        selectedFontSize = 11;
        UpdateFontStyles();
        fontNameText = selectedFont.Name;
    }

    public FontDialogViewModel(FontDialogResult? initial) : this()
    {
        if (initial == null)
            return;

        selectedFont = Fonts.FirstOrDefault(x => x == initial.Family) ?? Fonts.FirstOrDefault();
        UpdateFontStyles();
        selectedFontSize = initial.Size;
        selectedFontStyle = FontStyles.FirstOrDefault(x => x.FontWeight == initial.Weight && x.FontStyle == initial.Style);
    }

    public void Accept()
    {
        if (SelectedFont == null)
            return;

        AcceptRequested?.Invoke(
            new FontDialogResult(SelectedFont,
                SelectedFontStyle?.FontStyle ?? FontStyle.Normal,
                SelectedFontStyle?.FontWeight ?? FontWeight.Normal,
                SelectedFontSize));
    }

    public void Cancel()
    {
        CancelRequested?.Invoke();
    }

    private static FontWeight[] weights =
    [
        FontWeight.Thin,
        FontWeight.ExtraLight,
        FontWeight.Light,
        FontWeight.SemiLight,
        FontWeight.Regular,
        FontWeight.Medium,
        FontWeight.SemiBold,
        FontWeight.Bold,
        FontWeight.ExtraBold,
        FontWeight.Black,
        FontWeight.ExtraBlack
    ];

    private static FontStyle[] styles = [FontStyle.Normal, FontStyle.Italic, FontStyle.Oblique];

    private void UpdateFontStyles()
    {
        FontStyles.Clear();
        if (selectedFont == null)
            return;

        foreach (var style in styles)
        {
            foreach (var weight in weights)
            {
                var typeface = new Typeface(selectedFont, style, weight);
                var glyph = typeface.GlyphTypeface;
                if (glyph.Style == style && glyph.Weight == weight)
                {
                    FontStyles.Add(new LegacyFontStyle(style, weight));
                }
            }
        }

        SelectedFontStyle = FontStyles.FirstOrDefault();
    }

    #region INPC

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}