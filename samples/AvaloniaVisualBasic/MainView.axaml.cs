using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using AvaloniaVisualBasic.Forms.ViewModels;
using AvaloniaVisualBasic.Forms.Views;
using Classic.Avalonia.Theme;

namespace AvaloniaVisualBasic;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var window = new ClassicWindow()
        {
            Content = new NewProjectView()
            {
                DataContext = new NewProjectViewModel()
            },
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Title = "New Project",
            CanResize = false,
        };
        window.ShowDialog(this.GetVisualRoot() as Window);
    }
}