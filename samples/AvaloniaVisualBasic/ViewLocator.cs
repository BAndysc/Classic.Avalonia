using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaVisualBasic.Forms.ViewModels;
using AvaloniaVisualBasic.Forms.Views;

namespace AvaloniaVisualBasic;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is CodeEditorViewModel)
            return new CodeEditorView();
        if (param is FormEditViewModel)
            return new FormEditView();
        if (param is MenuEditorViewModel)
            return new MenuEditorView();
        return null;
    }

    public bool Match(object? data)
    {
        return data is CodeEditorViewModel or FormEditViewModel or MenuEditorViewModel;
    }
}