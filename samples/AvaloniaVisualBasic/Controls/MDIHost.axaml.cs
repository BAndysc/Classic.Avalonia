using Avalonia.Controls;

namespace AvaloniaVisualBasic.Controls;

public class MDIHost : ItemsControl
{
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MDIWindow>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MDIWindow();
    }
}