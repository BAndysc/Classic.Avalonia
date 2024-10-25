using Avalonia.Media;

namespace AvaloniaVisualBasic.Forms.ViewModels;

public abstract class BaseEditorWindowViewModel
{
    public abstract string Title { get; }
    public abstract IImage Icon { get; }
}