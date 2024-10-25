using System;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace AvaloniaVisualBasic.Controls;

public class MDIHostPanel : Panel
{
    public static readonly AttachedProperty<bool> IsActiveProperty = AvaloniaProperty.RegisterAttached<MDIHostPanel, Control, bool>("IsActive");
    public static readonly AttachedProperty<WindowState> WindowStateProperty = AvaloniaProperty.RegisterAttached<MDIHostPanel, Control, WindowState>("WindowState");
    public static readonly AttachedProperty<Point> WindowLocationProperty = AvaloniaProperty.RegisterAttached<MDIHostPanel, Control, Point>("WindowLocation");
    public static readonly AttachedProperty<Point> OldWindowLocationProperty = AvaloniaProperty.RegisterAttached<MDIHostPanel, Control, Point>("OldWindowLocation");
    public static readonly AttachedProperty<Point?> OldMinimizedWindowLocationProperty = AvaloniaProperty.RegisterAttached<MDIHostPanel, Control, Point?>("OldMinimizedWindowLocation");
    public static readonly AttachedProperty<Size> WindowSizeProperty = AvaloniaProperty.RegisterAttached<MDIHostPanel, Control, Size>("WindowSize", defaultValue:new Size(400, 200));

    public static bool GetIsActive(AvaloniaObject element) => element.GetValue(IsActiveProperty);

    public static void SetIsActive(AvaloniaObject element, bool value) => element.SetValue(IsActiveProperty, value);

    public static Point GetWindowLocation(AvaloniaObject element) => element.GetValue(WindowLocationProperty);

    public static void SetWindowLocation(AvaloniaObject element, Point value) => element.SetValue(WindowLocationProperty, value);

    public static Point? GetOldMinimizedWindowLocation(AvaloniaObject element) => element.GetValue(OldMinimizedWindowLocationProperty);

    public static void SetOldMinimizedWindowLocation(AvaloniaObject element, Point? value) => element.SetValue(OldMinimizedWindowLocationProperty, value);

    public static Point GetOldWindowLocation(AvaloniaObject element) => element.GetValue(OldWindowLocationProperty);

    public static void SetOldWindowLocation(AvaloniaObject element, Point value) => element.SetValue(OldWindowLocationProperty, value);

    public static WindowState GetWindowState(AvaloniaObject element) => element.GetValue(WindowStateProperty);

    public static void SetWindowState(AvaloniaObject element, WindowState value) => element.SetValue(WindowStateProperty, value);

    public static Size GetWindowSize(AvaloniaObject element) => element.GetValue(WindowSizeProperty);

    public static void SetWindowSize(AvaloniaObject element, Size value) => element.SetValue(WindowSizeProperty, value);

    static MDIHostPanel()
    {
        AffectsParentArrange<MDIHostPanel>(WindowLocationProperty, WindowSizeProperty, WindowStateProperty, ZIndexProperty);
    }

    private IDisposable? hostWindowIsActiveDisposable;

    public MDIHostPanel()
    {
        Children.CollectionChanged += OnChildrenChanged;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (this.GetVisualRoot() is Window window)
        {
            hostWindowIsActiveDisposable = window.GetObservable(Window.IsActiveProperty)
                .Subscribe(new ActionObserver<bool>(@is =>
                {
                    InvalidateArrange();
                }));
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        hostWindowIsActiveDisposable?.Dispose();
        hostWindowIsActiveDisposable = null;
    }

    private void OnChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var i = Children.Count - e.NewItems!.Count;
            foreach (Control @new in e.NewItems!)
            {
                var location = GetWindowLocation(@new);
                if (location == default)
                {
                    i++;
                    var point = new Point(30, 30) * i;
                    SetWindowLocation(@new, point);
                }
            }
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Control? topZ = null;
        foreach (var child in Children)
        {
            SetIsActive(child, false);
            if (topZ == null || topZ.ZIndex < child.ZIndex)
            {
                topZ = child;
            }
        }

        if (topZ != null && this.GetVisualRoot() is Window window && window.IsActive)
            SetIsActive(topZ, true);

        foreach (var child in Children)
        {
            var state = GetWindowState(child);
            var location = GetWindowLocation(child);
            var size = GetWindowSize(child);
            if (state == WindowState.Minimized)
            {
                size = new Size(160, 24);
            }
            else if (state == WindowState.Maximized)
            {
                location = new Point(0, 0);
                size = finalSize;
            }
            else
            {
                size = new Size(Math.Max(size.Width, child.MinWidth), Math.Max(size.Height, child.MinHeight));
            }
            child.Arrange(new Rect(location, size));
        }

        return finalSize;
    }
}