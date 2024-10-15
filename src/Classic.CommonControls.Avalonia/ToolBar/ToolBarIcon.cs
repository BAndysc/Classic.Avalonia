using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Classic.CommonControls;

public class ToolBarIcon : Control
{
    private static float[] grayscaleMatrix = {
        0.43f, 0.43f, 0.43f, 0.0f, 0.0f,
        0.43f, 0.43f, 0.43f, 0.0f, 0.0f,
        0.43f, 0.43f, 0.43f, 0.0f, 0.0f,
        0.0f,  0.0f,  0.0f,  1.0f, 0.0f
    };

    public static readonly StyledProperty<Bitmap?> SourceProperty =
        AvaloniaProperty.Register<ToolBarIcon, Bitmap?>(nameof(Source));

    public static readonly StyledProperty<Bitmap?> LargeSourceProperty =
        AvaloniaProperty.Register<ToolBarIcon, Bitmap?>(nameof(LargeSource));

    public static readonly StyledProperty<ToolBarIconStyle> IconStyleProperty =
        AvaloniaProperty.Register<ToolBarIcon, ToolBarIconStyle>(nameof(IconStyle));

    public static readonly StyledProperty<Color> DisabledColorProperty = AvaloniaProperty.Register<ToolBarIcon, Color>(nameof(DisabledColor), Colors.Gray);
    public static readonly StyledProperty<Color> DisabledShadowColorProperty = AvaloniaProperty.Register<ToolBarIcon, Color>(nameof(DisabledShadowColor), Colors.White);

    static ToolBarIcon()
    {
        AffectsRender<ToolBarIcon>(IconStyleProperty, DisabledColorProperty, DisabledShadowColorProperty, SourceProperty, LargeSourceProperty);
    }

    [Content]
    public Bitmap? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public Bitmap? LargeSource
    {
        get => GetValue(LargeSourceProperty);
        set => SetValue(LargeSourceProperty, value);
    }

    public ToolBarIconStyle IconStyle
    {
        get => GetValue(IconStyleProperty);
        set => SetValue(IconStyleProperty, value);
    }

    public Color DisabledColor
    {
        get => GetValue(DisabledColorProperty);
        set => SetValue(DisabledColorProperty, value);
    }

    public Color DisabledShadowColor
    {
        get => GetValue(DisabledShadowColorProperty);
        set => SetValue(DisabledShadowColorProperty, value);
    }

    class CustomDrawOp : ICustomDrawOperation
    {
        private readonly Bitmap bitmap;
        private readonly ToolBarIconStyle iconStyle;
        private readonly Color lightLightColor;
        private readonly Color grayTextColor;
        public Rect Bounds { get; }

        public CustomDrawOp(Bitmap source, ToolBarIconStyle iconStyle, Rect bounds, Color lightLightColor, Color grayTextColor)
        {
            this.bitmap = source;
            this.iconStyle = iconStyle;
            this.lightLightColor = lightLightColor;
            this.grayTextColor = grayTextColor;
            Bounds = bounds;
        }

        public void Dispose() { }

        public bool HitTest(Point p) => true;

        public bool Equals(ICustomDrawOperation other) => false;

        /// <summary>
        /// This method handles the rendering of the chart using SkiaSharp.
        /// </summary>
        /// <param name="context">The drawing context.</param>
        public void Render(ImmediateDrawingContext context)
        {
            if (context.TryGetFeature(typeof(ISkiaSharpApiLeaseFeature)) is ISkiaSharpApiLeaseFeature leaseFeature)
            {
                using var lease = leaseFeature.Lease();
                var canvas = lease.SkCanvas;
                canvas.Save();

                using SKColorFilter colorFilter = SKColorFilter.CreateColorMatrix(grayscaleMatrix);

                SKRect bounds = Bounds.ToSKRect();

                var stream = new MemoryStream();
                bitmap.Save(stream);
                stream.Position = 0;

                using var skBitmap = SKBitmap.Decode(stream);

                SKPaint? brush = null;
                if (iconStyle == ToolBarIconStyle.Disabled)
                {
                    canvas.DrawImage(SKImage.FromBitmap(skBitmap),Bounds.WithX(1).WithY(1).ToSKRect(), new SKPaint
                    {
                        ColorFilter = SKColorFilter.CreateBlendMode(lightLightColor.ToSKColor(), SKBlendMode.SrcIn)
                    });

                    brush = new SKPaint
                    {
                        ColorFilter = SKColorFilter.CreateBlendMode(grayTextColor.ToSKColor(), SKBlendMode.SrcIn)
                    };
                }
                else if (iconStyle == ToolBarIconStyle.Grayscale)
                {
                    brush = new SKPaint
                    {
                        ColorFilter = colorFilter
                    };
                }

                canvas.DrawImage(SKImage.FromBitmap(skBitmap), Bounds.ToSKRect(), brush);
                canvas.Restore();
            }
        }
    }

    public override void Render(DrawingContext context)
    {
        if (LargeSource == null && Source == null)
            return;

        Bitmap source = LargeSource ?? Source!;

        if (LargeSource != null && Source != null)
        {
            if (Bounds.Height >= LargeSource.PixelSize.Height)
                source = LargeSource;
            else
                source = Source;
        }

        context.Custom(new CustomDrawOp(source, IconStyle, new Rect(0, 0, Bounds.Width, Bounds.Height), DisabledShadowColor, DisabledColor));
    }
}