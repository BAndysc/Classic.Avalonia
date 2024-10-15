using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Classic.CommonControls;

public class StatusBar : ItemsControl
{
    public static readonly AttachedProperty<GridLength?> SizeProperty =
        AvaloniaProperty.RegisterAttached<StatusBar, Control, GridLength?>("Size");

    public static GridLength? GetSize(Visual control)
    {
        return control.GetValue(SizeProperty);
    }

    public static void SetSize(Visual control, GridLength? size)
    {
        control.SetValue(SizeProperty, size);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new StatusBarItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<StatusBarItem>(item, out recycleKey);
    }

    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new StatusBarPanel() { Spacing = 2 });

    static StatusBar()
    {
        ItemsPanelProperty.OverrideDefaultValue<StatusBar>(DefaultPanel);
        HorizontalAlignmentProperty.OverrideDefaultValue<StatusBar>(HorizontalAlignment.Stretch);
    }

    public class StatusBarPanel : Panel
    {
        public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<StatusBarPanel, double>(nameof (Spacing));

        public double Spacing
        {
            get => GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        static StatusBarPanel()
        {
            AffectsArrange<StatusBarPanel>(SpacingProperty);
            HorizontalAlignmentProperty.OverrideDefaultValue<StatusBarPanel>(HorizontalAlignment.Stretch);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double absoluteWidth = 0;
            double starCount = 0;
            int visibleItems = 0;

            foreach (var child in Children)
            {
                if (!child.IsVisible)
                    continue;

                visibleItems++;

                var size = GetSize(child);
                if (!size.HasValue || size.Value.IsAuto)
                {
                    absoluteWidth += child.DesiredSize.Width;
                }
                else if (size.Value.IsAbsolute)
                {
                    absoluteWidth += size.Value.Value;
                }
                else
                {
                    starCount += size.Value.Value;
                }
            }

            double totalSpaceForSpacing = Math.Max((visibleItems - 1) * Spacing, 0);

            double widthForStar = Math.Max(0, finalSize.Width - absoluteWidth - totalSpaceForSpacing);
            double widthPerStar = starCount == 0 ? 0 : (widthForStar / starCount);

            foreach (var child in Children)
            {
                var size = GetSize(child);
                double width;
                if (!size.HasValue || size.Value.IsAuto)
                {
                    width = child.DesiredSize.Width;
                }
                else if (size.Value.IsAbsolute)
                {
                    width = size.Value.Value;
                }
                else
                {
                    width = widthPerStar * size.Value.Value;
                }

                child.Arrange(new Rect(x, 0, width, finalSize.Height));
                x += width;
                x += Spacing;
            }

            return new Size(finalSize.Width, finalSize.Height);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }
    }
}