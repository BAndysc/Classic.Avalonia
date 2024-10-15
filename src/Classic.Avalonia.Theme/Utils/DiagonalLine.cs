using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Classic.Avalonia.Theme.Utils;

internal class DiagonalLine : Shape
{
    static DiagonalLine()
    {
        AffectsGeometry<DiagonalLine>(
            BoundsProperty);
        StrokeThicknessProperty.OverrideDefaultValue<Line>(1);
    }

    protected override Geometry CreateDefiningGeometry()
    {
        return new LineGeometry(new Point(0, 0), new Point(Bounds.Width, Bounds.Height));
    }
}