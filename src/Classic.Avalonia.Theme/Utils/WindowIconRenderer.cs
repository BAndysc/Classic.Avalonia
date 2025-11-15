using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Classic.Avalonia.Theme.Utils;

internal class WindowIconRenderer : Control
{
    public static readonly StyledProperty<WindowIcon?> SourceProperty = AvaloniaProperty.Register<WindowIconRenderer, WindowIcon?>(nameof(Source));

    public static readonly StyledProperty<WindowIcon?> SourceOverrideProperty = AvaloniaProperty.Register<WindowIconRenderer, WindowIcon?>(nameof(SourceOverride));

    private IImage? cachedImage = null;

    public WindowIcon? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public WindowIcon? SourceOverride
    {
        get => GetValue(SourceOverrideProperty);
        set => SetValue(SourceOverrideProperty, value);
    }

    static WindowIconRenderer()
    {
        SourceProperty.Changed.AddClassHandler<WindowIconRenderer>((renderer, e) =>
        {
            renderer.UpdateIcon();
        });
        SourceOverrideProperty.Changed.AddClassHandler<WindowIconRenderer>((renderer, e) =>
        {
            renderer.UpdateIcon();
        });
    }

    private void UpdateIcon()
    {
        if (SourceOverride != null || Source != null)
        {
            var memoryStream = new MemoryStream();
            (SourceOverride ?? Source)!.Save(memoryStream);
            memoryStream.Position = 0;
            cachedImage = new Bitmap(memoryStream);
            memoryStream.Position = 0;
            try
            {
                if (IcoIcon.TryParse(memoryStream, out var icoIcon) &&
                    icoIcon.GetIconExactSize(16) is { } icon16 &&
                    icoIcon.IsBmp(icon16) &&
                    IcoToBitmap.TryExtractBitmapFromIcon(icon16, icoIcon.GetImageData(icon16).Span, out var bitmap))
                {
                    cachedImage = bitmap;
                }
            } catch (Exception ex)
            {
                Debug.WriteLine("Error parsing ICO: " + ex);
            }
        }
        else
        {
            cachedImage = null;
        }
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        if (cachedImage != null)
        {
            context.DrawImage(cachedImage, new Rect(0, 0, Bounds.Width, Bounds.Height));
        }
    }
}