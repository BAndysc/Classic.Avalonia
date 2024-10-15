using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;

namespace Classic.CommonControls;

public class RebarHandle : TemplatedControl
{
    static RebarHandle()
    {
        CursorProperty.OverrideDefaultValue<RebarHandle>(new Cursor(StandardCursorType.Hand));
        HorizontalAlignmentProperty.OverrideDefaultValue<RebarHandle>(HorizontalAlignment.Left);
    }
}