using Avalonia;
using System;
using Classic.Avalonia.Theme;
using Classic.CommonControls;
using Avalonia.Controls;
using Avalonia.Dialogs;

namespace Classic.Demo;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseMessageBoxSounds()
            .UseManagedSystemDialogs<ClassicWindow>();
}