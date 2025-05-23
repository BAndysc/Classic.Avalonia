using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Classic.CommonControls;

public class IconRenderer : Control
{
    private static float[] grayscaleMatrix = {
        0.43f, 0.43f, 0.43f, 0.0f, 0.0f,
        0.43f, 0.43f, 0.43f, 0.0f, 0.0f,
        0.43f, 0.43f, 0.43f, 0.0f, 0.0f,
        0.0f,  0.0f,  0.0f,  1.0f, 0.0f
    };

    public static readonly StyledProperty<Bitmap?> SourceProperty =
        AvaloniaProperty.Register<IconRenderer, Bitmap?>(nameof(Source));

    public static readonly StyledProperty<Bitmap?> LargeSourceProperty =
        AvaloniaProperty.Register<IconRenderer, Bitmap?>(nameof(LargeSource));

    public static readonly StyledProperty<IconStyle> IconStyleProperty =
        AvaloniaProperty.Register<IconRenderer, IconStyle>(nameof(IconStyle));

    public static readonly StyledProperty<Color> SelectedColorProperty = AvaloniaProperty.Register<IconRenderer, Color>(nameof(SelectedColorProperty), Colors.DarkBlue);
    public static readonly StyledProperty<Color> DisabledColorProperty = AvaloniaProperty.Register<IconRenderer, Color>(nameof(DisabledColor), Colors.Gray);
    public static readonly StyledProperty<Color> DisabledShadowColorProperty = AvaloniaProperty.Register<IconRenderer, Color>(nameof(DisabledShadowColor), Colors.White);

    static IconRenderer()
    {
        AffectsRender<IconRenderer>(IconStyleProperty, SelectedColorProperty, DisabledColorProperty, DisabledShadowColorProperty, SourceProperty, LargeSourceProperty);
        AffectsArrange<IconRenderer>(SourceProperty, LargeSourceProperty);
        AffectsMeasure<IconRenderer>(SourceProperty, LargeSourceProperty);
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

    public IconStyle IconStyle
    {
        get => GetValue(IconStyleProperty);
        set => SetValue(IconStyleProperty, value);
    }

    public Color DisabledColor
    {
        get => GetValue(DisabledColorProperty);
        set => SetValue(DisabledColorProperty, value);
    }

    public Color SelectedColor
    {
        get => GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    public Color DisabledShadowColor
    {
        get => GetValue(DisabledShadowColorProperty);
        set => SetValue(DisabledShadowColorProperty, value);
    }

    class CustomDrawOp : ICustomDrawOperation
    {
        private readonly Visual visual;
        private readonly Bitmap bitmap;
        private readonly IconStyle iconStyle;
        private readonly Color lightLightColor;
        private readonly Color grayTextColor;
        private readonly Color selectedColor;
        public Rect Bounds { get; }

        private static Dictionary<CacheKey, SKPaint> cache = new();
        private static SKPaint grayscalePaint;
        private record struct CacheKey(BitmapInterpolationMode mode, Color? colorFilter, SKBlendMode? blendMode);
        private static SKPaint GetPaint(BitmapInterpolationMode mode, Color? colorFilter, SKBlendMode? blendMode)
        {
            var key = new CacheKey(mode, colorFilter, blendMode);
            if (cache.TryGetValue(key, out var paint))
                return paint;

            paint = new SKPaint();
            if (colorFilter != null)
                paint.ColorFilter = SKColorFilter.CreateBlendMode(colorFilter.Value.ToSKColor(), blendMode ?? SKBlendMode.SrcIn);
            if (blendMode != null)
                paint.BlendMode = blendMode.Value;
            paint.FilterQuality = mode.ToSKFilterQuality();
            cache[key] = paint;
            return paint;
        }

        public CustomDrawOp(Visual visual,
            Bitmap source,
            IconStyle iconStyle,
            Rect bounds,
            Color lightLightColor,
            Color grayTextColor,
            Color selectedColor)
        {
            this.visual = visual;
            this.bitmap = source;
            this.iconStyle = iconStyle;
            this.lightLightColor = lightLightColor;
            this.grayTextColor = grayTextColor;
            this.selectedColor = selectedColor;
            Bounds = bounds;
        }

        public void Dispose() { }

        public bool HitTest(Point p) => true;

        public bool Equals(ICustomDrawOperation? other) => false;

        static CustomDrawOp()
        {
            grayscalePaint = new SKPaint
            {
                ColorFilter =  SKColorFilter.CreateColorMatrix(grayscaleMatrix)
            };
        }

        /// <summary>
        /// This method handles the rendering of the chart using SkiaSharp.
        /// </summary>
        /// <param name="context">The drawing context.</param>
        public void Render(ImmediateDrawingContext context)
        {
            if (context.TryGetFeature(typeof(ISkiaSharpApiLeaseFeature)) is ISkiaSharpApiLeaseFeature leaseFeature)
            {
                var interpolation = BitmapInterpolationMode.None;
                using var lease = leaseFeature.Lease();
                var canvas = lease.SkCanvas;
                canvas.Save();

                SKRect bounds = Bounds.ToSKRect();

                var stream = new MemoryStream();
                bitmap.Save(stream);
                stream.Position = 0;

                using var skBitmap = SKBitmap.Decode(stream);
                if (iconStyle == IconStyle.Disabled)
                {
                    var pixels = skBitmap.Pixels;
                    bool changed = false;
                    for (var index = 0; index < pixels.Length; index++)
                    {
                        var pixel = pixels[index];
                        if ((int)pixel.Red + pixel.Green + pixel.Blue >= (255*3 / 2))
                        {
                            pixels[index] = SKColor.Empty;
                            changed = true;
                        }
                    }
                    if (changed)
                       skBitmap.Pixels = pixels;
                }

                using var skImage = SKImage.FromBitmap(skBitmap);

                SKPaint? brush = GetPaint(interpolation, null, null);
                if (iconStyle == IconStyle.Disabled)
                {
                    canvas.DrawImage(skImage,Bounds.WithX(1).WithY(1).ToSKRect(),
                        GetPaint(interpolation, lightLightColor, default));

                    brush = GetPaint(interpolation, grayTextColor, default);
                }
                else if (iconStyle == IconStyle.Grayscale)
                {
                    brush = grayscalePaint;
                }

                canvas.DrawImage(skImage, Bounds.ToSKRect(), brush);
                if (iconStyle == IconStyle.SelectedItem)
                {
                    using var patternBitmap = new SKBitmap((int)Bounds.Width, (int)Bounds.Height);
                    using var patternCanvas = new SKCanvas(patternBitmap);
                    var paint = GetPaint(interpolation, default, default);
                    using (var pattern = new SKBitmap(2, 2))
                    {
                        pattern.SetPixel(0, 0, selectedColor.ToSKColor());
                        pattern.SetPixel(1, 1, selectedColor.ToSKColor());

                        // Create a repeating shader from the 2x2 pattern.
                        paint.Shader = SKShader.CreateBitmap(pattern,
                            SKShaderTileMode.Repeat,
                            SKShaderTileMode.Repeat);
                    }

                    // Step 4: Draw the repeating pattern onto the pattern bitmap.
                    patternCanvas.DrawRect(new SKRect(0, 0, patternBitmap.Width, patternBitmap.Height), paint);

                    // Step 5: Blend the pattern with the original image's alpha channel.
                    var alphaPaint = GetPaint(interpolation, default, SKBlendMode.DstIn);
                    alphaPaint.BlendMode = SKBlendMode.DstIn;
                    patternCanvas.DrawBitmap(skBitmap, Bounds.ToSKRect(), alphaPaint);

                    // Step 6: Draw the alpha-masked pattern onto the main canvas.
                    canvas.DrawBitmap(patternBitmap, new SKPoint(0, 0));
                }

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

        context.Custom(new CustomDrawOp(this, source, IconStyle, new Rect(0, 0, Bounds.Width, Bounds.Height), DisabledShadowColor, DisabledColor, SelectedColor));
    }

    /// <summary>Measures the control.</summary>
    /// <param name="availableSize">The available size.</param>
    /// <returns>The desired size of the control.</returns>
    protected override Size MeasureOverride(Size availableSize)
    {
        var stretch = Stretch.Fill;
        IImage? source = this.Source ?? LargeSource;
        Size size = new Size();
        if (source != null)
            size = stretch.CalculateSize(availableSize, source.Size);
        return size;
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(Size finalSize)
    {
        var stretch = Stretch.Fill;
        IImage? source = this.Source ?? LargeSource;
        if (source == null)
            return new Size();
        Size size = source.Size;
        return stretch.CalculateSize(finalSize, size);
    }
}