using Classic.CommonControls.Utils;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;

namespace Classic.CommonControls.Dialogs;

public class AboutDialogOptions
{
    public string Title { get; set; } = "";
    public string? SubTitle { get; set; } = null;
    public string Copyright { get; set; } = "";
    public IImage? Icon { get; set; }
}

public class AboutDialog : TemplatedControl
{
    public static async Task ShowDialog(Window owner, AboutDialogOptions options)
    {
        AboutDialogViewModel viewModel = new(options);

        var window = owner.CreateDefaultWindow();
        window.Title = "About " + options.Title;
        window.DataContext = viewModel;
        window.Content = new AboutDialog();
        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.CanResize = false;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        viewModel.CloseRequested += () =>
        {
            window.Close(null);
        };

        await window.ShowDialog(owner);
    }
}