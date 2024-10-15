using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;

namespace Classic.CommonControls;

public class ToolBar : TemplatedControl
{
    public static readonly StyledProperty<bool> IsOverflowProperty = AvaloniaProperty.Register<ToolBar, bool>(nameof(IsOverflow));

    public bool IsOverflow
    {
        get => GetValue(IsOverflowProperty);
        set => SetValue(IsOverflowProperty, value);
    }

    private Panel? panel;
    public static readonly StyledProperty<ToolbarSize> SizeProperty = AvaloniaProperty.Register<ToolBar, ToolbarSize>(nameof(Size));
    public static readonly StyledProperty<ToolbarTextPlacement> TextPlacementProperty = AvaloniaProperty.Register<ToolBar, ToolbarTextPlacement>(nameof(TextPlacement));
    public static readonly StyledProperty<bool> GrayscaleIconsProperty = AvaloniaProperty.Register<ToolBar, bool>(nameof(GrayscaleIcons));

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        panel = e.NameScope.Get<ToolbarPanel>("PART_Panel");
        panel.Children.AddRange(Children);
        LogicalChildren.AddRange(Children);
    }

    public ToolBar() => Children.CollectionChanged += ChildrenChanged;

    private void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (panel == null)
            return;

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            LogicalChildren.AddRange(e.NewItems!.OfType<Control>());
            panel.Children.AddRange(e.NewItems!.OfType<Control>());
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            LogicalChildren.RemoveAll(e.OldItems!.OfType<Control>());
            panel.Children.RemoveAll(e.OldItems!.OfType<Control>());
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            for (int index1 = 0; index1 < e.OldItems!.Count; ++index1)
            {
                int index2 = index1 + e.OldStartingIndex;
                Control? newItem = (Control?) e.NewItems![index1];
                panel.Children[index2] = newItem!;
                LogicalChildren[index2] = newItem!;
            }
        }
        else
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
                return;
            panel.Children.Clear();
            panel.Children.AddRange(Children);
            LogicalChildren.Clear();
            LogicalChildren.AddRange(Children);
        }
    }

    [Content]
    public Controls Children { get; } = new();

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

    public bool GrayscaleIcons
    {
        get => GetValue(GrayscaleIconsProperty);
        set => SetValue(GrayscaleIconsProperty, value);
    }
}

public enum ToolbarSize
{
    Small,
    Large
}

public enum ToolbarTextPlacement
{
    Right,
    RightPreferNoText,
    Down,
    NoText
}