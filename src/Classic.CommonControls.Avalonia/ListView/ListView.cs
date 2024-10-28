using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Classic.CommonControls;

public class ListView : ListBox
{
    public static readonly StyledProperty<ListViewStyle> ViewProperty = AvaloniaProperty.Register<ListView, ListViewStyle>(nameof(View));

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<ListViewItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new ListViewItem();
    }

    public ListViewStyle View
    {
        get => GetValue(ViewProperty);
        set => SetValue(ViewProperty, value);
    }

    internal bool UpdateSelectionFromPointerEvent(Control source, PointerEventArgs e)
    {
        var hotkeys = Application.Current!.PlatformSettings?.HotkeyConfiguration;
        var toggle = hotkeys is not null && (e.KeyModifiers & hotkeys.CommandModifiers) == hotkeys.CommandModifiers;

        return UpdateSelectionFromEventSource(
            source,
            true,
            (e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift,
            toggle,
            e.GetCurrentPoint(source).Properties.IsRightButtonPressed);
    }
}