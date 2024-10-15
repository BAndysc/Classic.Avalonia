using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace AvaloniaExplorer.Panels;

public class IePanel : ContentControl
{
    public static readonly StyledProperty<string> HeaderProperty = AvaloniaProperty.Register<IePanel, string>("Header");

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
}