using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Data;
using Avalonia.Media.Imaging;

namespace Classic.CommonControls;

[PseudoClasses(":checked", ":unchecked")]
public class ToolBarButton : Button
{
    public static readonly StyledProperty<Bitmap?> SmallIconProperty = AvaloniaProperty.Register<ToolBarButton, Bitmap?>(nameof(SmallIcon));
    public static readonly StyledProperty<Bitmap?> LargeIconProperty = AvaloniaProperty.Register<ToolBarButton, Bitmap?>(nameof(LargeIcon));
    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<ToolBarButton, string?>(nameof(Text));
    public static readonly StyledProperty<ToolbarSize> SizeProperty = AvaloniaProperty.Register<ToolBarButton, ToolbarSize>(nameof(Size));
    public static readonly StyledProperty<ToolbarTextPlacement> TextPlacementProperty = AvaloniaProperty.Register<ToolBarButton, ToolbarTextPlacement>(nameof(TextPlacement));
    public static readonly StyledProperty<bool> PreferTextProperty = AvaloniaProperty.Register<ToolBarButton, bool>(nameof(PreferText));
    public static readonly StyledProperty<bool> GrayscaleIconProperty = AvaloniaProperty.Register<ToolBar, bool>(nameof(GrayscaleIcon));

    public static readonly StyledProperty<bool> IsCheckedProperty = AvaloniaProperty.Register<ToolBarButton, bool>(nameof(IsChecked), false, defaultBindingMode: BindingMode.TwoWay);
    public static readonly StyledProperty<bool> IsToggleButtonProperty = AvaloniaProperty.Register<ToolBarButton, bool>(nameof(IsToggleButton), false);

    public Bitmap? SmallIcon
    {
        get => GetValue(SmallIconProperty);
        set => SetValue(SmallIconProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Bitmap? LargeIcon
    {
        get => GetValue(LargeIconProperty);
        set => SetValue(LargeIconProperty, value);
    }

    public ToolbarSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
    public ToolbarTextPlacement TextPlacement
    {
        get => GetValue(TextPlacementProperty);
        set => SetValue(TextPlacementProperty, value);
    }

    public bool PreferText
    {
        get => GetValue(PreferTextProperty);
        set => SetValue(PreferTextProperty, value);
    }

    public bool GrayscaleIcon
    {
        get => GetValue(GrayscaleIconProperty);
        set => SetValue(GrayscaleIconProperty, value);
    }

    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public bool IsToggleButton
    {
        get => GetValue(IsToggleButtonProperty);
        set => SetValue(IsToggleButtonProperty, value);
    }

    static ToolBarButton()
    {
        TextProperty.Changed.AddClassHandler<ToolBarButton>((button, e) => button.UpdatePseudoClass());
        SmallIconProperty.Changed.AddClassHandler<ToolBarButton>((button, e) => button.UpdatePseudoClass());
        LargeIconProperty.Changed.AddClassHandler<ToolBarButton>((button, e) => button.UpdatePseudoClass());
        IsCheckedProperty.Changed.AddClassHandler<ToolBarButton>((button, e) => button.UpdatePseudoClass());
        IsToggleButtonProperty.Changed.AddClassHandler<ToolBarButton>((button, e) => button.UpdatePseudoClass());
        FlyoutProperty.Changed.AddClassHandler<ToolBarButton>((button, e) => button.UpdatePseudoClass());
    }

    public ToolBarButton()
    {
        UpdatePseudoClass();
    }

    protected override void OnClick()
    {
        if (IsToggleButton)
        {
            SetCurrentValue(IsCheckedProperty, !IsChecked);
        }
        base.OnClick();
    }

    private void UpdatePseudoClass()
    {
        PseudoClasses.Set(":text", !string.IsNullOrEmpty(Text));
        PseudoClasses.Set(":icon", SmallIcon != null || LargeIcon != null);
        PseudoClasses.Set(":noicon", SmallIcon == null && LargeIcon == null);
        PseudoClasses.Set(":checked", IsToggleButton && IsChecked);
        PseudoClasses.Set(":unchecked", IsToggleButton && !IsChecked);
        PseudoClasses.Set(":has-flyout", Flyout != null);
    }
}