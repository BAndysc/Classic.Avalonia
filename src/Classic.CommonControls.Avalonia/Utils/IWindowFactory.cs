using Avalonia;
using Avalonia.Controls;

namespace Classic.CommonControls.Utils;

public interface IWindowFactory
{
    Window Create();
}

internal static class WindowFactoryExtensions
{
    public static Window CreateDefaultWindow(this Window w)
    {
        object? factory = null;
        Application.Current?.TryGetResource(typeof(IWindowFactory), out factory);
        return (factory as IWindowFactory)?.Create() ?? new Window();
    }
}