using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Classic.Avalonia.Theme.Utils;

internal class WindowIconRenderer : Control
{
    public static readonly StyledProperty<WindowIcon?> SourceProperty = AvaloniaProperty.Register<WindowIconRenderer, WindowIcon?>(nameof(Source));

    private IImage? cachedImage = null;

    public WindowIcon? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    static WindowIconRenderer()
    {
        SourceProperty.Changed.AddClassHandler<WindowIconRenderer>((renderer, e) =>
        {
            if (renderer.Source != null)
            {
                var memoryStream = new MemoryStream();
                renderer.Source.Save(memoryStream);
                memoryStream.Position = 0;
                renderer.cachedImage = new Bitmap(memoryStream);
            }
            else
            {
                renderer.cachedImage = null;
            }
            renderer.InvalidateVisual();
        });
    }

    public override void Render(DrawingContext context)
    {
        if (cachedImage != null)
        {
            context.DrawImage(cachedImage, new Rect(0, 0, Bounds.Width, Bounds.Height));
        }
    }
}