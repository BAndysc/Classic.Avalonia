/*
 https://github.com/dotnet/wpf/blob/main/src/Microsoft.DotNet.Wpf/src/Themes/PresentationFramework.Classic/Microsoft/Windows/Themes/ClassicBorderDecorator.cs
 * The MIT License (MIT)

   Copyright (c) .NET Foundation and Contributors

   All rights reserved.

   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included in all
   copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
   SOFTWARE.
 */

using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Classic.CommonControls;
using Color = Avalonia.Media.Color;
// ReSharper disable InconsistentNaming

namespace Classic.Avalonia.Theme;

/// <summary>
/// Specifies the type of Border to draw
/// </summary>
public enum ClassicBorderStyle
{
    /// <summary>
    /// No Classic Border
    /// </summary>
    None,

    /// <summary>
    /// Normal Button Style
    /// </summary>
    Raised,

    /// <summary>
    /// Pressed Button Style
    /// </summary>
    RaisedPressed,

    /// <summary>
    /// Focused or Defaulted Button Style
    /// </summary>
    RaisedFocused,

    /// <summary>
    /// ListBox, TextBox, CheckBox, etc... Style
    /// </summary>
    Sunken,

    /// <summary>
    /// GroupBox Style
    /// </summary>
    Etched,

    /// <summary>
    /// Separator Style (Horizontal)
    /// </summary>
    HorizontalLine,

    /// <summary>
    /// Separator Style (Vertical)
    /// </summary>
    VerticalLine,

    /// <summary>
    /// Right Tab Style
    /// </summary>
    TabRight,

    /// <summary>
    /// Top Tab Style
    /// </summary>
    TabTop,

    /// <summary>
    /// Left Tab Style
    /// </summary>
    TabLeft,

    /// <summary>
    /// Bottom Tab Style
    /// </summary>
    TabBottom,

    /// <summary>
    /// Top-level MenuItems or ToolBar buttons in a hover state
    /// </summary>
    ThinRaised,

    /// <summary>
    /// Top-level MenuItems or ToolBars buttons in a pressed state
    /// </summary>
    ThinPressed,

    /// <summary>
    /// ScrollBar Button Style
    /// </summary>
    AltRaised,

    /// <summary>
    /// Pressed ScrollBar Button Style
    /// </summary>
    AltPressed,

    /// <summary>
    /// RadioButton Style
    /// </summary>
    RadioButton,

    //If you add any new Styles, update ClassicBorderDecorator.IsValidBorderStyle

    RaiseReversed,
    ThinHighContrastPressed,
    ThinHighContrastRaised,
    ThinPressedInnerShadow,
    Thin
}

/// <summary>
/// The ClassicBorderDecorator element
/// This element is a theme-specific type that is used as an optimization
/// for a common complex rendering used in Classic
///
/// To acheive the Classic look, specify ClassicBorderBrush as the BorderBrush
/// Any other brush will remove the Classic look and render a border of BorderThickness
/// (Tabs always render in the Classic look)
/// </summary>
/// <ExternalAPI/>

public sealed class ClassicBorderDecorator : Decorator
{

    #region Constructors

    static ClassicBorderDecorator()
    {
        AffectsRender<ClassicBorderDecorator>(BackgroundProperty, BorderBrushProperty, BorderDarkBrushProperty, BorderDarkDarkBrushProperty, BorderLightBrushProperty, BorderLightLightBrushProperty);
        BackgroundProperty.Changed.AddClassHandler<ClassicBorderDecorator>(BorderBrushesChanged);

        AffectsRender<ClassicBorderDecorator>(BorderThicknessProperty, BorderStyleProperty);
        AffectsArrange<ClassicBorderDecorator>(BorderThicknessProperty, BorderStyleProperty);
        AffectsMeasure<ClassicBorderDecorator>(BorderThicknessProperty, BorderStyleProperty);
        BorderStyleProperty.Changed.AddClassHandler<ClassicBorderDecorator>(BorderBrushesChanged);
        ControlColorProperty.Changed.AddClassHandler<ClassicBorderDecorator>(BorderBrushesChanged);
    }

    /// <summary>
    /// Instantiates a new instance of a ClassicBorderDecorator
    /// </summary>
    public ClassicBorderDecorator()
    {
        RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
    }

    #endregion Constructors

    #region Dynamic Properties

    /// <summary>
    /// AvaloniaProperty for <see cref="Background" /> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
            Panel.BackgroundProperty.AddOwner<ClassicBorderDecorator>();

    /// <summary>
    /// The Background property defines the brush used to fill the border region.
    /// </summary>
    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }


    #region HlS Color Space Conversions
    // Provided by the Sparkle team's ColorUtility class

    private struct RgbColor
    {
        public byte Alpha;
        public float Red;
        public float Green;
        public float Blue;

        public RgbColor(byte alpha, float red, float green, float blue)
        {
            this.Alpha = alpha;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public RgbColor(Color color)
            : this(color.A, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f)
        {
        }

        public Color Color => Color.FromArgb(this.Alpha, (byte)(this.Red * 255), (byte)(this.Green * 255), (byte)(this.Blue * 255));
    }

    private struct HlsColor
    {
        public byte Alpha;
        public float Hue;
        public float Lightness;
        public float Saturation;

        public HlsColor(byte alpha, float hue, float lightness, float saturation)
        {
            this.Alpha = alpha;
            this.Hue = hue;
            this.Lightness = lightness;
            this.Saturation = saturation;
        }
    }
    /// <summary>
    /// Converts a color from RGB to HLS color space.
    /// If hue is undefined (because color is achromatic), it defaults to zero.
    /// If saturation is undefined (because lightness is zero or one), it defaults to zero.
    /// </summary>
    /// <param name="rgbColor">The input color.</param>
    /// <returns>An HLS color corresponding to the input RGB color.</returns>
    private static HlsColor RgbToHls(RgbColor rgbColor)
    {
        return RgbToHls(rgbColor, out _, out _);
    }

    /// <summary>
    /// Converts an RGB color to an HSB color, with output flags to indicate whether or not hue and/or saturation
    /// are defined after the conversion.
    /// </summary>
    /// <param name="rgbColor">An RGB color to convert to HSB color space.</param>
    /// <param name="isHueDefined">True if input color lies off the R=G=B line in the RGB cube, false if it lies on this line.</param>
    /// <param name="isSaturationDefined">True if input color isn't at either HLS cone tip, false if it is.</param>
    /// <returns>
    /// An HLS color corresponding to the input RGB color.
    /// If isHueDefined comes out false, then the resulting hue defaults to 0.
    /// If isSaturationDefined comes out false, then the resulting saturation defaults to 0.
    /// </returns>
    private static HlsColor RgbToHls(RgbColor rgbColor, out bool isHueDefined, out bool isSaturationDefined)
    {
        Debug.Assert(rgbColor.Red >= 0 && rgbColor.Red <= 1, "Red must be between 0 and 1.");
        Debug.Assert(rgbColor.Green >= 0 && rgbColor.Green <= 1, "Green must be between 0 and 1.");
        Debug.Assert(rgbColor.Blue >= 0 && rgbColor.Blue <= 1, "Blue must be between 0 and 1.");

        float min = Math.Min(rgbColor.Red, Math.Min(rgbColor.Green, rgbColor.Blue));
        float max = Math.Max(rgbColor.Red, Math.Max(rgbColor.Green, rgbColor.Blue));

        float hue = 0;
        float lightness = (min + max) / 2;
        float saturation = 0;

        // Three classifications of a point in the HLS double-cone:
        // (1) Point is on either cone's tip => hue & saturation both undefined
        // (2) Point is on the saturation=0 axis and isn't on either cone's tip => hue undefined, saturation=0
        // (3) Point lies off the saturation=0 axis and off both cone's tips => hue defined, saturation defined
        // So the only invalid value for hue & saturation defined flags is: (hue defined, saturation undefined).

        if (min == max)
        {
            // Red, green, and blue are equal (the achromatic case).
            // Hue is undefined, so we arbitrarily choose zero.
            // If lightness is zero or one, then saturation is also undefined, so we arbitrarily choose zero.
            // Otherwise, saturation is zero.
            isHueDefined = false;
            isSaturationDefined = (lightness > 0 && lightness < 1);
        }
        else
        {
            // Hue and saturation are both defined.
            // Point lies off the saturation=0 axis and isn't on either HLS cone tip.

            isHueDefined = isSaturationDefined = true;

            float delta = max - min;
            saturation = (lightness <= 0.5f ? delta / (min + max) : delta / (2 - min - max));

            // Force saturation to be between 0 and 1 in case of numerical instability
            saturation = Math.Max(0.0f, Math.Min(saturation, 1.0f));
            if (rgbColor.Red == max)
            {
                // between yellow & magenta
                hue = (rgbColor.Green - rgbColor.Blue) / delta;
            }
            else if (rgbColor.Green == max)
            {
                // between cyan & yellow
                hue = 2 + (rgbColor.Blue - rgbColor.Red) / delta;
            }
            else
            {
                // between magenta & cyan
                hue = 4 + (rgbColor.Red - rgbColor.Green) / delta;
            }

            // Make hue positive.
            if (hue < 0)
            {
                hue += 6;
            }

            // Scale hue to [0, 1].
            hue /= 6;
        }

        Debug.Assert(hue >= 0 && hue <= 1, "Output hue should be between 0 and 1.");
        Debug.Assert(lightness >= 0 && lightness <= 1, "Output lightness should be between 0 and 1.");
        Debug.Assert(saturation >= 0 && saturation <= 1, "Output saturation should be between 0 and 1.");

        Debug.Assert(!isHueDefined || isSaturationDefined, "If hue is defined, saturation must also be defined.");

        return new HlsColor(rgbColor.Alpha, hue, lightness, saturation);
    }

    /// <summary>
    /// Converts a color from HLS to RGB color space.
    /// </summary>
    /// <param name="hlsColor">The input color.</param>
    /// <returns>An RGB color corresponding to the input color.</returns>
    private static RgbColor HlsToRgb(HlsColor hlsColor)
    {
        Debug.Assert(hlsColor.Lightness >= 0 && hlsColor.Lightness <= 1, "Lightness must be between 0 and 1.");
        Debug.Assert(hlsColor.Saturation >= 0 && hlsColor.Saturation <= 1, "Saturation must be between 0 and 1.");

        // Check for the achromatic (gray) case.
        float red, green, blue;
        if (hlsColor.Saturation == 0.0)
        {
            red = green = blue = hlsColor.Lightness;
        }
        else
        {
            // Shift hue into the [0, 6) interval.
            float hue = (float)((hlsColor.Hue - Math.Floor(hlsColor.Hue)) * 6);

            float m2;
            if (hlsColor.Lightness <= 0.5f)
                m2 = hlsColor.Lightness * (1 + hlsColor.Saturation);
            else
                m2 = hlsColor.Lightness + hlsColor.Saturation - hlsColor.Lightness * hlsColor.Saturation;

            float m1 = 2 * hlsColor.Lightness - m2;
            red = HlsValue(m1, m2, hue + 2);
            green = HlsValue(m1, m2, hue);
            blue = HlsValue(m1, m2, hue - 2);
        }

        Debug.Assert(red >= 0 && red <= 1, "Output red should be between 0 and 1.");
        Debug.Assert(green >= 0 && green <= 1, "Output green should be between 0 and 1.");
        Debug.Assert(blue >= 0 && blue <= 1, "Output blue should be between 0 and 1.");

        return new RgbColor(hlsColor.Alpha, red, green, blue);
    }

    private static float HlsValue(float n1, float n2, float hue)
    {
        Debug.Assert(n1 >= 0 && n1 <= 1, "n1 must be between 0 and 1.");
        Debug.Assert(n2 >= 0 && n2 <= 1, "n2 must be between 0 and 1.");
        Debug.Assert(hue >= -6 && hue < 12, "Hue must be between -6 (inclusive) and 12 (exclusive).");

        // Shift hue into the [0, 6) interval.
        if (hue < 0)
        {
            hue += 6;
        }
        else if (hue >= 6)
        {
            hue -= 6;
        }

        if (hue < 1)
        {
            return n1 + (n2 - n1) * hue;
        }
        else if (hue < 3)
        {
            return n2;
        }
        else if (hue < 4)
        {
            return n1 + (n2 - n1) * (4 - hue);
        }
        else
        {
            return n1;
        }
    }

    #endregion

    // Computes the ControlLightLightColor from the controlColor using the same algorithm as
    // the Win32 appearance dialog
    private static Color GetControlLightLightColor(Color controlColor)
    {
        HlsColor hls = RgbToHls(new RgbColor(controlColor));
        hls.Lightness = (hls.Lightness + 1.0f) * 0.5f;
        return HlsToRgb(hls).Color;
    }

    // Computes the ControlDarkColor from the controlColor using the same algorithm as
    // the Win32 appearance dialog
    private static Color GetControlDarkColor(Color controlColor)
    {
        HlsColor hls = RgbToHls(new RgbColor(controlColor));
        hls.Lightness = hls.Lightness * 0.666f;
        return HlsToRgb(hls).Color;
    }

    // Set the highlight and shadow brushes used to draw the border
    private static void BorderBrushesChanged(ClassicBorderDecorator d, AvaloniaPropertyChangedEventArgs e)
    {
        ClassicBorderDecorator decorator = d;

        SolidColorBrush? controlBrush = decorator.Background as SolidColorBrush;
        Color controlColor;

        // When style changes, tab geometries are invalid
        decorator._tabCache = null;

        var appControlColor = d.ControlColor;

        // Calculate 3D highlights and shadows for solid color brushes that are not the control color
        // Don't recompute colors for the sunken styles (Sunken and RadioButton)
        if (decorator.BorderStyle != ClassicBorderStyle.Sunken && decorator.BorderStyle != ClassicBorderStyle.RadioButton &&
            !ReferenceEquals(d.BorderBrush, ClassicBorderBrush) &&
            controlBrush != null && (controlColor = controlBrush.Color) != appControlColor && controlColor.A > 0x00)
        {
            // If we haven't cached the colors, or if the background color changed, compute the new colors
            if (decorator._brushCache == null || controlColor != decorator._brushCache?.LightBrush?.Color)
            {
                if (decorator._brushCache == null)
                {
                    decorator._brushCache = new CustomBrushCache();
                }

                // Follow shell's method of computing highlight and shadows
                // Light = Control
                // LightLight = Control with increased luminance.  LightLight[luma] = (Control[luma] + 1.0)/2
                // Dark = Control with decreased luminance.  Dark[luma] = Control[luma] * 66.6%
                // DarkDark = Dark averaged with WindowFrame. DarkDark[r,g,b] = (Dark + WindowFrame)/2
                decorator._brushCache.LightBrush = controlBrush;
                decorator._brushCache.LightLightBrush = new SolidColorBrush(GetControlLightLightColor(controlColor));

                Color darkColor = GetControlDarkColor(controlColor);
                decorator._brushCache.DarkBrush = new SolidColorBrush(darkColor);

                var windowFrameColor = d.GetResourceOrDefault(SystemColors.WindowFrameColorKey, default(Color));

                // DarkDark = Average of DarkColor and WindowFrameColor
                Color darkDarkColor = new Color((byte)((darkColor.A + windowFrameColor.A) / 2),
                    (byte)((darkColor.R + windowFrameColor.R) / 2),
                    (byte)((darkColor.G + windowFrameColor.G) / 2),
                    (byte)((darkColor.B + windowFrameColor.B) / 2)
                );

                decorator._brushCache.DarkDarkBrush = new SolidColorBrush(darkDarkColor);
                decorator._brushCache.ControlBrush = controlBrush;
            }
        }
        else
        {
            decorator._brushCache = null;
        }
    }

    /// <summary>
    /// AvaloniaProperty for <see cref="BorderStyle" /> property.
    /// </summary>
    public static readonly StyledProperty<ClassicBorderStyle> BorderStyleProperty =
                AvaloniaProperty.Register<ClassicBorderDecorator, ClassicBorderStyle>(
                        nameof(BorderStyle),
                        ClassicBorderStyle.Raised,
                        validate: IsValidBorderStyle);

    /// <summary>
    /// The BorderStyle property defines the style used to draw the border
    /// </summary>
    public ClassicBorderStyle BorderStyle
    {
        get => GetValue(BorderStyleProperty);
        set => SetValue(BorderStyleProperty, value);
    }

    private static bool IsValidBorderStyle(ClassicBorderStyle style)
    {
        return style == ClassicBorderStyle.None ||
               style == ClassicBorderStyle.Raised ||
               style == ClassicBorderStyle.RaisedPressed ||
               style == ClassicBorderStyle.RaisedFocused ||
               style == ClassicBorderStyle.Sunken ||
               style == ClassicBorderStyle.Etched ||
               style == ClassicBorderStyle.HorizontalLine ||
               style == ClassicBorderStyle.VerticalLine ||
               style == ClassicBorderStyle.TabRight ||
               style == ClassicBorderStyle.TabTop ||
               style == ClassicBorderStyle.TabLeft ||
               style == ClassicBorderStyle.TabBottom ||
               style == ClassicBorderStyle.ThinRaised ||
               style == ClassicBorderStyle.ThinPressed ||
               style == ClassicBorderStyle.AltRaised ||
               style == ClassicBorderStyle.AltPressed ||
               style == ClassicBorderStyle.RadioButton ||
               style == ClassicBorderStyle.RaiseReversed |
               style == ClassicBorderStyle.ThinHighContrastPressed ||
               style == ClassicBorderStyle.ThinHighContrastRaised ||
               style == ClassicBorderStyle.ThinPressedInnerShadow ||
               style == ClassicBorderStyle.Thin;
    }


    /// <summary>
    /// This is a dummy brush used to tell ClassicBorderDecorator to draw the border according to the BorderStyle
    /// If BorderBrush is not set to ClassicBorderBrush, a flat style is drawn
    /// </summary>
    private static IBrush? _classicBorderBrush;
    private static object _brushLock = new object();

    public static IBrush ClassicBorderBrush
    {
        get
        {
            if (_classicBorderBrush == null)
            {
                lock (_brushLock)
                {
                    if (_classicBorderBrush == null)
                    {
                        SolidColorBrush temp = new SolidColorBrush();
                        _classicBorderBrush = temp;
                    }
                }
            }
            return _classicBorderBrush;
        }
    }

    /// <summary>
    /// AvaloniaProperty for <see cref="BorderBrush" /> property.
    /// </summary>
    public static readonly StyledProperty<IBrush> BorderBrushProperty =
            AvaloniaProperty.Register<ClassicBorderDecorator, IBrush>(
                    nameof(BorderBrush),
                    ClassicBorderBrush);

    /// <summary>
    /// The BorderBrush property defines the brush used to draw the border
    /// </summary>
    public IBrush? BorderBrush
    {
        get => (IBrush?) GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public static readonly StyledProperty<IBrush?> BorderLightBrushProperty = AvaloniaProperty.Register<ClassicBorderDecorator, IBrush?>(nameof(BorderLightBrush));

    public static readonly StyledProperty<IBrush?> BorderLightLightBrushProperty = AvaloniaProperty.Register<ClassicBorderDecorator, IBrush?>(nameof(BorderLightLightBrush));

    public static readonly StyledProperty<IBrush?> BorderDarkBrushProperty = AvaloniaProperty.Register<ClassicBorderDecorator, IBrush?>(nameof(BorderDarkBrush));

    public static readonly StyledProperty<IBrush?> BorderDarkDarkBrushProperty = AvaloniaProperty.Register<ClassicBorderDecorator, IBrush?>(nameof(BorderDarkDarkBrush));

    public static readonly StyledProperty<IBrush?> BorderControlBrushProperty = AvaloniaProperty.Register<ClassicBorderDecorator, IBrush?>(nameof(BorderControlBrush));

    public static readonly StyledProperty<Color> ControlColorProperty = AvaloniaProperty.Register<ClassicBorderDecorator, Color>(nameof(ControlColor));

    public IBrush? BorderLightBrush
    {
        get => GetValue(BorderLightBrushProperty);
        set => SetValue(BorderLightBrushProperty, value);
    }

    public IBrush? BorderLightLightBrush
    {
        get => GetValue(BorderLightLightBrushProperty);
        set => SetValue(BorderLightLightBrushProperty, value);
    }

    public IBrush? BorderDarkBrush
    {
        get => GetValue(BorderDarkBrushProperty);
        set => SetValue(BorderDarkBrushProperty, value);
    }

    public IBrush? BorderDarkDarkBrush
    {
        get => GetValue(BorderDarkDarkBrushProperty);
        set => SetValue(BorderDarkDarkBrushProperty, value);
    }

    public IBrush? BorderControlBrush
    {
        get => GetValue(BorderControlBrushProperty);
        set => SetValue(BorderControlBrushProperty, value);
    }

    public Color ControlColor
    {
        get => GetValue(ControlColorProperty);
        set => SetValue(ControlColorProperty, value);
    }

    /// <summary>
    /// AvaloniaProperty for <see cref="BorderThickness" /> property.
    /// </summary>
    public static readonly StyledProperty<Thickness> BorderThicknessProperty =
            AvaloniaProperty.Register<ClassicBorderDecorator, Thickness>(
                    nameof(BorderThickness),
                    new Thickness(0.0),
                    validate:IsValidBorderThickness);

    private static bool IsValidBorderThickness(Thickness o)
    {
          Thickness thickness = o;

          return (thickness.Left >= 0 && !double.IsPositiveInfinity(thickness.Left) &&
                  thickness.Right >= 0 && !double.IsPositiveInfinity(thickness.Right) &&
                  thickness.Top >= 0 && !double.IsPositiveInfinity(thickness.Top) &&
                  thickness.Bottom >= 0 && !double.IsPositiveInfinity(thickness.Bottom));

    }

    /// <summary>
    /// The BorderThickness property defines the thickness used to draw the border
    /// </summary>
    public Thickness BorderThickness
    {
        get => GetValue(BorderThicknessProperty);

        set => SetValue(BorderThicknessProperty, value);
    }

    #endregion Dynamic Properties

    #region Protected Methods

    // Helper function to add up the left and right size as width, as well as the top and bottom size as height
    private static Size HelperCollapseThickness(Thickness th)
    {
        return new Size(th.Left + th.Right, th.Top + th.Bottom);
    }

    /// Helper to deflate rectangle by thickness
    private static Rect HelperDeflateRect(Rect rt, Thickness thick)
    {
        return new Rect(rt.Left + thick.Left,
                        rt.Top + thick.Top,
                        Math.Max(0.0, rt.Width - thick.Left - thick.Right),
                        Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
    }

    /// <summary>
    /// Returns the thickness of the classic border for each of the styles
    /// used for Measure and Arrange
    /// </summary>
    private double GetClassicBorderThickness()
    {
        //(don't draw classic border if BorderBrush is not the ClassicBorderBrush)
        if (!ReferenceEquals(BorderBrush, ClassicBorderBrush))
            return 2.0;  // 1 for Focused border and 1 for offsetting content when pressed

        switch (BorderStyle)
        {
            case ClassicBorderStyle.Raised:
            case ClassicBorderStyle.RaisedPressed:
            case ClassicBorderStyle.RaisedFocused:
            case ClassicBorderStyle.RaiseReversed:
                return 3.0;
            case ClassicBorderStyle.Sunken:
            case ClassicBorderStyle.Etched:
                return 2.0;
            case ClassicBorderStyle.HorizontalLine:
            case ClassicBorderStyle.VerticalLine:
                return 1.0;
            case ClassicBorderStyle.TabRight:
            case ClassicBorderStyle.TabLeft:
            case ClassicBorderStyle.TabTop:
            case ClassicBorderStyle.TabBottom:
                return 2.0;
            case ClassicBorderStyle.None:
            case ClassicBorderStyle.ThinRaised:
            case ClassicBorderStyle.ThinPressed:
            case ClassicBorderStyle.ThinPressedInnerShadow:
            case ClassicBorderStyle.ThinHighContrastPressed:
            case ClassicBorderStyle.ThinHighContrastRaised:
            case ClassicBorderStyle.Thin:
                return 1.0;
            case ClassicBorderStyle.AltRaised:
            case ClassicBorderStyle.AltPressed:
                return 2.0;
        }

        return 0.0;
    }

    /// <summary>
    /// Updates DesiredSize of the ClassicBorderDecorator.  Called by parent Control.  This is the first pass of layout.
    /// </summary>
    /// <remarks>
    /// ClassicBorderDecorator basically inflates the desired size of its child by its style's thickness and BorderThickness
    /// </remarks>
    /// <param name="availableSize">Available size is an "upper limit" that the return value should not exceed.</param>
    /// <returns>The ClassicBorderDecorator's desired size.</returns>
    protected override Size MeasureOverride(Size availableSize)
    {
        Size borderSize = HelperCollapseThickness(BorderThickness);

        Size desired;
        Control? child = Child;
        if (child != null)
        {
            double width = 0;
            double height = 0;
            bool isWidthTooSmall = (availableSize.Width < borderSize.Width);
            bool isHeightTooSmall = (availableSize.Height < borderSize.Height);

            if (!isWidthTooSmall)
            {
                width = availableSize.Width - borderSize.Width;
            }
            if (!isHeightTooSmall)
            {
                height = availableSize.Height - borderSize.Height;
            }

            Size childConstraint = new Size(width, height);

            // Get the childs size after removing our border size from the available size
            child.Measure(childConstraint);

            desired = child.DesiredSize;

            if (!isWidthTooSmall)
            {
                desired = desired.WithWidth(desired.Width + borderSize.Width);
            }
            if (!isHeightTooSmall)
            {
                desired = desired.WithHeight(desired.Height + borderSize.Height);
            }
        }
        else
        {
            desired = new Size(Math.Min(borderSize.Width, availableSize.Width), Math.Min(borderSize.Height, availableSize.Height));
        }

        return desired;
    }

    /// <summary>
    /// ClassicBorderDecorator computes the position of its single child inside child's Margin and calls Arrange
    /// on the child.
    /// </summary>
    /// <remarks>
    /// ClassicBorderDecorator basically inflates the desired size of its child by its style's thickness and BorderThickness on all sides
    /// </remarks>
    /// <param name="finalSize">Size the ContentPresenter will assume.</param>
    protected override Size ArrangeOverride(Size finalSize)
    {
        Control? child = Child;
        if (child != null)
        {
            Thickness border = BorderThickness;

            double borderWidth = border.Left + border.Right + Padding.Left + Padding.Right;
            double borderHeight = border.Top + border.Bottom + Padding.Top + Padding.Bottom;

            double x = 0, y = 0, width = 0, height = 0;
            if ((finalSize.Width >= borderWidth) && (finalSize.Height >= borderHeight))
            {
                x = border.Left + Padding.Right;
                y = border.Top + Padding.Bottom;
                width = finalSize.Width - borderWidth;
                height = finalSize.Height - borderHeight;

                // The pressed style should have the child pushed down and right 1 px
                ClassicBorderStyle style = BorderStyle;
                if (style == ClassicBorderStyle.RaisedPressed ||
                    style == ClassicBorderStyle.ThinPressed    ||
                    style == ClassicBorderStyle.ThinPressedInnerShadow ||
                    style == ClassicBorderStyle.AltPressed ||
                    style == ClassicBorderStyle.ThinHighContrastPressed)
                {
                    x += 1.0;
                    y += 1.0;
                    width -= 1;
                    height -= 1;
                }
            }

            Rect childArrangeRect = new Rect(x, y, width, height);

            //Position the child
            child.Arrange(childArrangeRect);
        }

        return finalSize;
    }

    private static Thickness ScaleThickness(Thickness t, double s)
    {
        return new Thickness(t.Left * s, t.Top * s, t.Right * s, t.Bottom * s);
    }

    /// <summary>
    /// Render callback.
    /// </summary>
    public override void Render(DrawingContext drawingContext)
    {
        IBrush? borderBrush = BorderBrush;
        ClassicBorderStyle style = BorderStyle;
        Thickness borderThickness = BorderThickness;

        double classicThickness = GetClassicBorderThickness();
        // Thickness of a single border pair
        Thickness singleThickness = ScaleThickness(borderThickness, 1.0 / classicThickness);

        Rect bounds = new Rect(0, 0, Bounds.Width, Bounds.Height);

        // Draw a 1px border when focused or pressed (For consistency with Internet Explorer)
        if (style == ClassicBorderStyle.RaisedFocused || style == ClassicBorderStyle.RaisedPressed)
        {
            DrawBorder(GetResourceOrDefault(SystemColors.WindowFrameBrushKey, default(IBrush)), singleThickness, drawingContext, ref bounds);
        }

        bool isTabStyle = IsTabStyle(style);

        // Draw classic border if the border is set to ClassicBorderBrush
        if (ReferenceEquals(borderBrush, ClassicBorderBrush) || isTabStyle || style == ClassicBorderStyle.RadioButton)
        {
            switch (style)
            {
                case ClassicBorderStyle.Raised:
                case ClassicBorderStyle.RaisedFocused:
                    // Focused already has a 1px border drawn from above
                    DrawRaisedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.RaiseReversed:
                    DrawRaisedReversedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.RaisedPressed:
                    DrawRaisedPressedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.Sunken:
                    DrawSunkenBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.Etched:
                    DrawEtchedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.HorizontalLine:
                    DrawHorizontalLine(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.VerticalLine:
                    DrawVerticalLine(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.TabLeft:
                    DrawTabLeft(drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.TabTop:
                    DrawTabTop(drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.TabRight:
                    DrawTabRight(drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.TabBottom:
                    DrawTabBottom(drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.ThinRaised:
                    DrawThinRaisedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.ThinPressed:
                case ClassicBorderStyle.ThinPressedInnerShadow:
                    DrawThinPressedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.Thin:
                    DrawThinBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.ThinHighContrastRaised:
                    DrawThinRaisedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.ThinHighContrastPressed:
                    DrawThinPressedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.AltRaised:
                    DrawAltRaisedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.AltPressed:
                    DrawAltPressedBorder(singleThickness, drawingContext, ref bounds);
                    break;
                case ClassicBorderStyle.RadioButton:
                    DrawRadioButtonBorder(drawingContext, ref bounds);
                    return;
            }
        }
        else // BorderBrush != ClassicBorderBrush
        {
            //Draw a regular border
            DrawBorder(borderBrush, borderThickness, drawingContext, ref bounds);
        }

        // Fill the Background
        IBrush? background = Background;
        if (background != null && bounds.Width > 0.0 && bounds.Height > 0.0)
        {
            drawingContext.DrawRectangle(background, null, bounds);
        }
    }

    #region Complex Border Generation

    // Creates a rectangle figure
    private static PathFigure GenerateRectFigure(Rect rect)
    {
        PathFigure figure = new PathFigure();
        figure.StartPoint = rect.TopLeft;
        figure.Segments!.Add(new LineSegment() {Point = rect.TopRight});
        figure.Segments.Add(new LineSegment() {Point = rect.BottomRight});
        figure.Segments.Add(new LineSegment() {Point = rect.BottomLeft});
        figure.IsClosed = true;
        //figure.Freeze();

        return figure;
    }

    // Creates a border geometry used to render complex border brushes
    private static Geometry GenerateBorderGeometry(Rect rect, Thickness borderThickness)
    {
        PathGeometry geometry = new PathGeometry();

        // Add outer rectangle figure
        geometry.Figures!.Add(GenerateRectFigure(rect));

        // Subtract inner rectangle figure
        geometry.Figures.Add(GenerateRectFigure(HelperDeflateRect(rect, borderThickness)));

        //geometry.Freeze();

        return geometry;
    }

    private Geometry GetBorder(Rect bounds, Thickness borderThickness)
    {
        if (_borderGeometryCache == null)
            _borderGeometryCache = new BorderGeometryCache();

        if (_borderGeometryCache.Bounds != bounds || _borderGeometryCache.BorderThickness != borderThickness
            || _borderGeometryCache.BorderGeometry == null)
        {
            _borderGeometryCache.BorderGeometry = GenerateBorderGeometry(bounds, borderThickness);
            _borderGeometryCache.Bounds = bounds;
            _borderGeometryCache.BorderThickness = borderThickness;
        }

        return _borderGeometryCache.BorderGeometry;
    }

    #endregion

    // Returns true if the border can be drawn using overlapping rectangles
    private static bool IsSimpleBorderBrush(IBrush? borderBrush)
    {
        SolidColorBrush? solidBrush = borderBrush as SolidColorBrush;
        return (solidBrush != null && (solidBrush.Color.A == 0xFF || solidBrush.Color.A == 0x00));
    }

    // Draws a border around the control
    // Creates a path if the border is not a SolidColorBrush
    private void DrawBorder(IBrush? borderBrush, Thickness borderThickness, DrawingContext dc, ref Rect bounds)
    {
        Size borderSize = HelperCollapseThickness(borderThickness);

        if (borderSize.Width <= 0.0 && borderSize.Height <= 0.0)
            return;

        // If we don't have enough space for the entire border, just fill the area with the brush
        if (borderSize.Width > bounds.Width || borderSize.Height > bounds.Height)
        {
            if(borderBrush != null && bounds.Width > 0.0 && bounds.Height > 0.0)
            {
                dc.DrawRectangle(borderBrush, null, bounds);
            }
            bounds = default;
            return;
        }

        // Draw rectangles if borderBrush is solid(non-transparent), otherwise we need to draw a path
        if (IsSimpleBorderBrush(borderBrush))
        {
            // draw top
            if (borderThickness.Top > 0.0)
            {
                dc.DrawRectangle(borderBrush, null, new Rect(bounds.Left, bounds.Top, bounds.Width, borderThickness.Top));
            }

            // draw Left
            if (borderThickness.Left > 0.0)
            {
                dc.DrawRectangle(borderBrush, null, new Rect(bounds.Left, bounds.Top, borderThickness.Left, bounds.Height));
            }

            // draw Right
            if (borderThickness.Right > 0.0)
            {
                dc.DrawRectangle(borderBrush, null, new Rect(bounds.Right - borderThickness.Right, bounds.Top, borderThickness.Right, bounds.Height));
            }

            // draw Bottom
            if (borderThickness.Bottom > 0.0)
            {
                dc.DrawRectangle(borderBrush, null, new Rect(bounds.Left, bounds.Bottom - borderThickness.Bottom, bounds.Width, borderThickness.Bottom));
            }
        }
        else  // borderBrush is not solid, draw using a geometry
        {
            dc.DrawGeometry(borderBrush, null, GetBorder(bounds, borderThickness));
        }

        bounds = HelperDeflateRect(bounds, borderThickness);
    }

    // Draws a border with the top and left colored with highlight and the bottom and right colored with shadow
    private void DrawBorderPair(IBrush? highlight, IBrush? shadow, Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        DrawBorder(shadow,    new Thickness(0, 0, singleThickness.Right, singleThickness.Bottom), dc, ref bounds);
        DrawBorder(highlight, new Thickness(singleThickness.Left, singleThickness.Top, 0, 0), dc, ref bounds);
    }

    #region Draw Methods for all ClassicBorderStyles

    // Draw the 3D classic border
    private void DrawRaisedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 2.0 * (singleThickness.Left + singleThickness.Right) || bounds.Height < 2.0 * (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorderPair(LightLightBrush, DarkDarkBrush, singleThickness, dc, ref bounds);
        DrawBorderPair(LightBrush, DarkBrush, singleThickness, dc, ref bounds);
    }

    private void DrawRaisedReversedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 2.0 * (singleThickness.Left + singleThickness.Right) || bounds.Height < 2.0 * (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorderPair(DarkDarkBrush, LightLightBrush, singleThickness, dc, ref bounds);
        DrawBorderPair(DarkBrush, LightBrush, singleThickness, dc, ref bounds);
    }

    // Draw the 3D classic border
    private void DrawRaisedPressedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < (singleThickness.Left + singleThickness.Right) || bounds.Height < (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorder(DarkBrush, singleThickness, dc, ref bounds);
    }

    // Draw the sunken 3D classic border
    private void DrawSunkenBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 2.0 * (singleThickness.Left + singleThickness.Right) || bounds.Height < 2.0 * (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorderPair(DarkBrush, LightLightBrush, singleThickness, dc, ref bounds);
        DrawBorderPair(DarkDarkBrush, LightBrush, singleThickness, dc, ref bounds);
    }

    // Draw the etched border
    private void DrawEtchedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 2.0 * (singleThickness.Left + singleThickness.Right) || bounds.Height < 2.0 * (singleThickness.Top + singleThickness.Bottom))
            return;
        IBrush? dark = DarkBrush, lightLight = LightLightBrush;

        DrawBorderPair(dark, lightLight, singleThickness, dc, ref bounds);
        DrawBorderPair(lightLight, dark, singleThickness, dc, ref bounds);
    }

    // Draw a horizontal separator line
    private void DrawHorizontalLine(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Height < singleThickness.Top + singleThickness.Bottom)
            return;

        dc.DrawRectangle(DarkBrush, null, new Rect(bounds.Left, bounds.Top, bounds.Width, singleThickness.Top));
        dc.DrawRectangle(LightLightBrush, null, new Rect(bounds.Left, bounds.Bottom - singleThickness.Bottom, bounds.Width, singleThickness.Bottom));

        bounds = bounds.WithY(bounds.Y + singleThickness.Top)
            .WithHeight(bounds.Height - singleThickness.Top - singleThickness.Bottom);
    }

    // Draw a vertical separator line
    private void DrawVerticalLine(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < singleThickness.Left + singleThickness.Right)
            return;

        dc.DrawRectangle(DarkBrush, null, new Rect(bounds.Left, bounds.Top, singleThickness.Left, bounds.Height));
        dc.DrawRectangle(LightLightBrush, null, new Rect(bounds.Right - singleThickness.Right, bounds.Top, singleThickness.Right, bounds.Height));

        bounds = bounds.WithX(bounds.X + singleThickness.Left)
            .WithWidth(bounds.Width - singleThickness.Left - singleThickness.Right);
    }

    #region Tab Styles

    // Is this a tab style
    private bool IsTabStyle(ClassicBorderStyle style)
    {
        return style == ClassicBorderStyle.TabLeft ||
               style == ClassicBorderStyle.TabTop ||
               style == ClassicBorderStyle.TabRight ||
               style == ClassicBorderStyle.TabBottom;

    }

    // The highlight  geometry is the top and left borders with rounded corners
    private Geometry GenerateTabTopHighlightGeometry(Rect bounds, bool outerBorder)
    {
        double outerRadius = outerBorder ? 3.0 : 2.0;
        double innerRadius = outerRadius - 1.0;
        Size outerCorner = new Size(outerRadius, outerRadius), innerCorner = new Size(innerRadius, innerRadius);

        double left = bounds.Left, right = bounds.Right, top = bounds.Top, bottom = bounds.Bottom - 1.0;

        PathFigure figure = new PathFigure();
        //Start at bottom left, tracing the outside clockwise
        figure.StartPoint = new Point(left, bottom);
        figure.Segments!.Add(new LineSegment() { Point = new Point(left, top + outerRadius)});  //left side
        figure.Segments.Add(new ArcSegment() { Point = new Point(left + outerRadius, top), Size = outerCorner, RotationAngle = 0.0, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise}); //top left rounded corner
        figure.Segments.Add(new LineSegment() { Point = new Point(right - outerRadius, top)}); //top side
        figure.Segments.Add(new ArcSegment() { Point = new Point(right - outerRadius * 0.293, top + outerRadius * 0.293), Size = outerCorner, RotationAngle = 0.0, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise}); //top right corner
        figure.Segments.Add(new LineSegment() { Point = new Point(right - 1.0 - innerRadius * 0.293, top + 1.0 + innerRadius * 0.293)}); //inner top right corner
        figure.Segments.Add(new ArcSegment() { Point = new Point(right - outerRadius, top + 1.0), Size = innerCorner, RotationAngle = 0.0, IsLargeArc = false, SweepDirection = SweepDirection.CounterClockwise}); //inner top right rounded corner
        figure.Segments.Add(new LineSegment() { Point = new Point(left + outerRadius, top + 1.0)}); //inner top side
        figure.Segments.Add(new ArcSegment() { Point = new Point(left + 1.0, top + outerRadius), Size = innerCorner, RotationAngle = 0.0, IsLargeArc = false, SweepDirection = SweepDirection.CounterClockwise});//inner top left rounder corner
        figure.Segments.Add(new LineSegment() { Point = new Point(left + 1.0, bottom)}); //inner right side
        figure.IsClosed = true; //bottom side
        //figure.Freeze();

        PathGeometry geometry = new PathGeometry();
        geometry.Figures!.Add(figure);
        //geometry.Freeze();

        return geometry;
    }


    // The shadow geometry is the right borders with top rounded corner
    private Geometry GenerateTabTopShadowGeometry(Rect bounds, bool outerBorder)
    {
        double outerRadius = outerBorder ? 3.0 : 2.0;
        double innerRadius = outerRadius - 1.0;
        Size outerCorner = new Size(outerRadius, outerRadius), innerCorner = new Size(innerRadius, innerRadius);

        double right = bounds.Right, top = bounds.Top, bottom = bounds.Bottom - 1.0;

        PathFigure figure = new PathFigure();
        //Start at bottom left, tracing the outside clockwise
        figure.StartPoint = new Point(right - 1.0, bottom);
        figure.Segments!.Add(new LineSegment() { Point = new Point(right - 1.0, top + outerRadius)});  //left side
        figure.Segments.Add(new ArcSegment() { Point = new Point(right - 1.0 - innerRadius * 0.293, top + 1.0 + innerRadius * 0.293), Size = innerCorner, RotationAngle = 0.0, IsLargeArc = false, SweepDirection = SweepDirection.CounterClockwise}); //inner left rounded corner
        figure.Segments.Add(new LineSegment() { Point = new Point(right - outerRadius * 0.293, top + outerRadius * 0.293)}); //top right corner
        figure.Segments.Add(new ArcSegment() { Point = new Point(right, top + outerRadius), Size = outerCorner, RotationAngle = 0.0, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise}); //top right corner
        figure.Segments.Add(new LineSegment() { Point = new Point(right, bottom)}); //right side
        figure.IsClosed = true; //bottom side
        //figure.Freeze();

        PathGeometry geometry = new PathGeometry();
        geometry.Figures!.Add(figure);
        //geometry.Freeze();

        return geometry;
    }

    // Get the cache for the Highlight Geometry
    private Geometry GetHighlight1(Rect bounds)
    {
        if (_tabCache == null)
            _tabCache = new TabGeometryCache();

        if (_tabCache.Bounds != bounds || _tabCache.Highlight1 == null)
        {

            _tabCache.Highlight1 = GenerateTabTopHighlightGeometry(bounds, true);
            _tabCache.Bounds = bounds;
            _tabCache.Shadow1 = null;  //bounds changed, these are invalid
            _tabCache.Highlight2 = null;
            _tabCache.Shadow2 = null;
        }

        return _tabCache.Highlight1;
    }

    private Geometry GetShadow1(Rect bounds)
    {
        // Assumed to always be called after GetHighlight1
        Debug.Assert(_tabCache != null, "_tabCache is null.  GetShadow1 should only be called after GetHighlight1");

        if (_tabCache.Shadow1 == null)
        {
            _tabCache.Shadow1 = GenerateTabTopShadowGeometry(bounds, true);
        }

        return _tabCache.Shadow1;
    }

    private Geometry GetHighlight2(Rect bounds)
    {
        // Assumed to always be called after GetHighlight1
        Debug.Assert(_tabCache != null, "_tabCache is null.  GetHighlight2 should only be called after GetHighlight1");

        if (_tabCache.Highlight2 == null)
        {
            _tabCache.Highlight2 = GenerateTabTopHighlightGeometry(HelperDeflateRect(bounds, new Thickness(1, 1, 1, 0)), false);
        }

        return _tabCache.Highlight2;
    }

    private Geometry GetShadow2(Rect bounds)
    {
        // Assumed to always be called after GetHighlight1
        Debug.Assert(_tabCache != null, "_tabCache is null.  GetHighlight2 should only be called after GetHighlight1");

        if (_tabCache.Shadow2 == null)
        {
            _tabCache.Shadow2 = GenerateTabTopShadowGeometry(HelperDeflateRect(bounds, new Thickness(1, 1, 1, 0)), false);
        }

        return _tabCache.Shadow2;
    }

    // Gets the matrix that transforms the top tab geometries for position on left, right, or bottom
    private MatrixTransform GetTabTransform(ClassicBorderStyle style, double xOffset, double yOffset)
    {
        if (_tabCache == null)
            _tabCache = new TabGeometryCache();

        if (_tabCache.Transform == null || xOffset != _tabCache.xOffset || yOffset != _tabCache.yOffset)
        {
            switch (style)
            {
                case ClassicBorderStyle.TabLeft:
                    _tabCache.Transform = new MatrixTransform(new Matrix(0.0, 1.0, 1.0, 0.0, xOffset, yOffset));
                    break;
                case ClassicBorderStyle.TabRight:
                    _tabCache.Transform = new MatrixTransform(new Matrix(0.0, -1.0, -1.0, 0.0, xOffset, yOffset));
                    break;
                case ClassicBorderStyle.TabBottom:
                    _tabCache.Transform = new MatrixTransform(new Matrix(-1.0, 0.0, 0.0, -1.0, xOffset, yOffset));
                    break;
            }
            _tabCache.xOffset = xOffset;
            _tabCache.yOffset = yOffset;
        }
        return _tabCache.Transform;
    }

    // Draw the left tab
    private void DrawTabLeft(DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 6.0 || bounds.Height < 6.0)
            return;

        // Create a rotate bounds for use in generating the geometries
        Rect tempBounds = new Rect(0.0, 0.0, bounds.Height, bounds.Width);

        // Need to rotate and flip(for lighting) the top tab to generate the right tab
        var state = dc.PushTransform(GetTabTransform(ClassicBorderStyle.TabLeft, bounds.Left, bounds.Top).Matrix);

        dc.DrawGeometry(LightLightBrush, null, GetHighlight1(tempBounds));
        dc.DrawGeometry(DarkDarkBrush, null, GetShadow1(tempBounds));
        dc.DrawGeometry(LightBrush, null, GetHighlight2(tempBounds));
        dc.DrawGeometry(DarkBrush, null, GetShadow2(tempBounds));

        state.Dispose();

        bounds = HelperDeflateRect(bounds, new Thickness(2, 2, 0, 2));
    }

    // Draw the top tab
    private void DrawTabTop(DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 6.0 || bounds.Height < 6.0)
            return;

        dc.DrawGeometry(LightLightBrush, null, GetHighlight1(bounds));
        dc.DrawGeometry(DarkDarkBrush, null, GetShadow1(bounds));
        dc.DrawGeometry(LightBrush, null, GetHighlight2(bounds));
        dc.DrawGeometry(DarkBrush, null, GetShadow2(bounds));

        bounds = HelperDeflateRect(bounds, new Thickness(2, 2, 2, 0));
    }

    // Draw the right tab
    private void DrawTabRight(DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 6.0 || bounds.Height < 6.0)
            return;

        // Create a rotate bounds for use in generating the geometries
        Rect tempBounds = new Rect(0.0, 0.0, bounds.Height, bounds.Width) ;

        // Need to rotate and flip(for lighting) the top tab to generate the right tab
        var state = dc.PushTransform(GetTabTransform(ClassicBorderStyle.TabRight, bounds.Right, bounds.Bottom).Matrix);

        dc.DrawGeometry(DarkDarkBrush, null, GetHighlight1(tempBounds));
        dc.DrawGeometry(LightLightBrush, null, GetShadow1(tempBounds));
        dc.DrawGeometry(DarkBrush, null, GetHighlight2(tempBounds));
        dc.DrawGeometry(LightBrush, null, GetShadow2(tempBounds));

        state.Dispose();

        bounds = HelperDeflateRect(bounds, new Thickness(0, 2, 2, 2));
    }

    // Draw the bottom tab
    private void DrawTabBottom(DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 6.0 || bounds.Height < 6.0)
            return;

        Rect tempBounds = new Rect(0.0, 0.0, bounds.Width, bounds.Height);

        // Need to rotate the top tab to generate the bottom tab
        var state = dc.PushTransform(GetTabTransform(ClassicBorderStyle.TabBottom, bounds.Right, bounds.Bottom).Matrix);

        dc.DrawGeometry(DarkDarkBrush, null, GetHighlight1(tempBounds));
        dc.DrawGeometry(LightLightBrush, null, GetShadow1(tempBounds));
        dc.DrawGeometry(DarkBrush, null, GetHighlight2(tempBounds));
        dc.DrawGeometry(LightBrush, null, GetShadow2(tempBounds));

        state.Dispose();

        bounds = HelperDeflateRect(bounds, new Thickness(2, 0, 2, 2));
    }

    #endregion

    private void DrawThinRaisedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < (singleThickness.Left + singleThickness.Right) || bounds.Height < (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorderPair(LightLightBrush, BorderStyle == ClassicBorderStyle.ThinHighContrastRaised ? DarkDarkBrush : DarkBrush, singleThickness, dc, ref bounds);
    }

    private void DrawThinBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < (singleThickness.Left + singleThickness.Right) || bounds.Height < (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorderPair(DarkBrush, LightLightBrush, singleThickness, dc, ref bounds);
    }

    private void DrawThinPressedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < (singleThickness.Left + singleThickness.Right) || bounds.Height < (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorderPair(BorderStyle == ClassicBorderStyle.ThinHighContrastPressed ? DarkDarkBrush : DarkBrush, LightLightBrush, singleThickness, dc, ref bounds);
        if (BorderStyle == ClassicBorderStyle.ThinPressedInnerShadow)
        {
            DrawBorderPair(ControlBrush, ControlBrush, singleThickness, dc, ref bounds);
        }
    }

    private void DrawAltRaisedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 2.0 * (singleThickness.Left + singleThickness.Right) || bounds.Height < 2.0 * (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorderPair(LightBrush, DarkDarkBrush, singleThickness, dc, ref bounds);
        DrawBorderPair(LightLightBrush, DarkBrush, singleThickness, dc, ref bounds);
    }

    private void DrawAltPressedBorder(Thickness singleThickness, DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < (singleThickness.Left + singleThickness.Right) || bounds.Height < (singleThickness.Top + singleThickness.Bottom))
            return;

        DrawBorder(DarkBrush, singleThickness, dc, ref bounds);
    }

    private void DrawRadioButtonBorder(DrawingContext dc, ref Rect bounds)
    {
        if (bounds.Width < 12 || bounds.Height < 12)
            return;

        var isClassic = ReferenceEquals(BorderBrush, ClassicBorderBrush);

        dc.DrawGeometry(isClassic ? DarkDarkBrush : BorderBrush, new Pen(isClassic ? DarkBrush : BorderBrush, 1.0), TopLeftArcGeometry);
        dc.DrawGeometry(isClassic ? LightBrush : BorderBrush, new Pen(isClassic ? LightLightBrush : BorderBrush, 1.0), BottomRightArcGeometry);

        // DrawBackground
        dc.DrawEllipse(Background, null, new Point(6, 6), 4, 4);
    }

    #endregion

    #endregion

    private T GetResourceOrDefault<T>(object key, T defaultValue)
    {
        if (Application.Current.TryGetResource(key, out var res))
            if (res is T resT)
                return resT;
        return defaultValue;
    }

    #region Private Properties

    // Returns custom brush if avaliable, otherwise uses system brush
    private IBrush? LightBrush => _brushCache?.LightBrush ?? BorderLightBrush;

    // Returns custom brush if avaliable, otherwise uses system brush
    private IBrush? LightLightBrush => _brushCache?.LightLightBrush ?? BorderLightLightBrush;

    // Returns custom brush if avaliable, otherwise uses system brush
    private IBrush? DarkBrush => _brushCache?.DarkBrush ?? BorderDarkBrush;

    // Returns custom brush if avaliable, otherwise uses system brush
    private IBrush? DarkDarkBrush => _brushCache?.DarkDarkBrush ?? BorderDarkDarkBrush;

    // Returns custom brush if avaliable, otherwise uses system brush
    private IBrush? ControlBrush => _brushCache?.ControlBrush ?? BorderControlBrush;

    #endregion

    #region Data

    private static Geometry TopLeftArcGeometry
    {
        get
        {
            if (_topLeftArcGeometry == null)
            {
                lock (_resourceAccess)
                {
                    if (_topLeftArcGeometry == null)
                    {
                        StreamGeometry geometry = new StreamGeometry();

                        StreamGeometryContext sgc = geometry.Open();
                        sgc.BeginFigure(new Point(2, 10), true);
                        sgc.ArcTo(new Point(10, 2), new Size(4, 4), 0, false, SweepDirection.Clockwise);

                        sgc.EndFigure(true);

                        //geometry.Freeze();

                        _topLeftArcGeometry = geometry;
                    }
                }
            }

            return _topLeftArcGeometry;
        }
    }

    private static Geometry BottomRightArcGeometry
    {
        get
        {
            if (_bottomRightArcGeometry == null)
            {
                lock (_resourceAccess)
                {
                    if (_bottomRightArcGeometry == null)
                    {
                        StreamGeometry geometry = new StreamGeometry();

                        StreamGeometryContext sgc = geometry.Open();
                        sgc.BeginFigure(new Point(2, 10), true);
                        sgc.ArcTo(new Point(10, 2), new Size(4, 4), 0, false, SweepDirection.CounterClockwise);

                        sgc.EndFigure(true);

                        //geometry.Freeze();

                        _bottomRightArcGeometry = geometry;
                    }
                }
            }

            return _bottomRightArcGeometry;
        }
    }

    // Brushes computed from custom backgrounds
    private class CustomBrushCache
    {
        public SolidColorBrush? LightBrush;
        public SolidColorBrush? LightLightBrush;
        public SolidColorBrush? DarkBrush;
        public SolidColorBrush? DarkDarkBrush;
        public SolidColorBrush? ControlBrush;
    }

    // Cache of the border geometry used for drawing
    private class BorderGeometryCache
    {
        public Rect Bounds;  // bounds used to create geometry
        public Thickness BorderThickness;
        public Geometry? BorderGeometry;
    }

    // Cache of the tab geometry used for drawing
    private class TabGeometryCache
    {
        public Rect Bounds;  // bounds used to create geometry
        public Geometry? Highlight1;
        public Geometry? Shadow1;
        public Geometry? Highlight2;
        public Geometry? Shadow2;
        public double xOffset, yOffset;  // matrix offsets
        public MatrixTransform? Transform;
    }

    private CustomBrushCache? _brushCache;  //cache of custom colors

    private BorderGeometryCache? _borderGeometryCache;  //cache of geometry used for complex borders
    private TabGeometryCache? _tabCache; // cache of geometries for tab


    private static Geometry? _topLeftArcGeometry;
    private static Geometry? _bottomRightArcGeometry;

    private static object _resourceAccess = new object();
    #endregion
}