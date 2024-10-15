using Avalonia.Controls;
using Avalonia.Layout;

namespace Classic.CommonControls;

public class StatusBarItem : ContentControl
{
    static StatusBarItem()
    {
        HorizontalContentAlignmentProperty.OverrideDefaultValue<StatusBarItem>(HorizontalAlignment.Stretch);
    }
}