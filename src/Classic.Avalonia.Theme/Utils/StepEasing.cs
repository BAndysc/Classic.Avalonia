using Avalonia.Animation.Easings;

namespace Classic.Avalonia.Theme.Utils;

public class StepEasing : Easing
{
    public override double Ease(double progress)
    {
        return ((int)(progress * 50) * 1.0 / 50);
    }
}