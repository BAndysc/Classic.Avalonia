using Classic.CommonControls.Utils;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace Classic.CommonControls.Dialogs;

public class FontDialog : TemplatedControl
{
    public static async Task<FontDialogResult?> ShowDialog(Window owner)
    {
        FontDialogViewModel viewModel = new();

        var window = owner.CreateDefaultWindow();
        window.Title = "Font";
        window.DataContext = viewModel;
        window.Content = new FontDialog();
        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.CanResize = false;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        viewModel.CancelRequested += () =>
        {
            window.Close(null);
        };

        viewModel.AcceptRequested += result =>
        {
            window.Close(result);
        };

        return await window.ShowDialog<FontDialogResult>(owner);
    }
}