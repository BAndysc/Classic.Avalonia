using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Classic.Avalonia.Theme;

namespace DockXamlSample;

public partial class MainWindow : ClassicWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
