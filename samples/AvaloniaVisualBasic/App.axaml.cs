using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Classic.Avalonia.Theme;

namespace AvaloniaVisualBasic;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = new ClassicWindow()
            {
                Title = "Project1 - Avalonia Visual Basic [design]",
                Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://AvaloniaVisualBasic/Icons/vb6.gif"))),
                Content = new MainView()
                {
                    DataContext = new MainViewViewModel()
                }
            };
            window.AttachDevTools();
            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}