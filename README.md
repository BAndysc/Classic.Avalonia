# ![icon](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/icon.png) Classic.Avalonia

### 📢 If you have used Classic.Avalonia, share your app [here](https://github.com/BAndysc/Classic.Avalonia/discussions/2)!

## Overview

This project brings the classic Windows 9x theme to Avalonia. It is inspired by the [WPF Classic Theme](https://github.com/dotnet/wpf/tree/main/src/Microsoft.DotNet.Wpf/src/Themes/PresentationFramework.Classic/Themes), with enhancements and additional controls for a more authentic look and feel. Check out the screenshots below for a preview!

## Installation

1. Install the `Classic.Avalonia.Theme` package via NuGet:
   ```
   Install-Package Classic.Avalonia.Theme
   ```
2. In your `App.axaml`, replace the existing theme (e.g., `<FluentTheme />` or `<SimpleTheme />`) with the Classic theme:
   ```xml
   <Application ...>
       <Application.Styles>
           <ClassicTheme />
       </Application.Styles>
   </Application>
   ```

### DataGrid
To use the DataGrid, install the `Classic.Avalonia.Theme.DataGrid` package and include the style in `App.axaml`:
```xml
<StyleInclude Source="avares://Classic.Avalonia.Theme.DataGrid/Classic.axaml"/>
```

### ColorPicker
For the ColorPicker, install the `Classic.Avalonia.Theme.ColorPicker` package and include the style in `App.axaml`:
```xml
<StyleInclude Source="avares://Classic.Avalonia.Theme.ColorPicker/Classic.axaml"/>
```

### Dock
There is also a classic theme for a fantastic [Wieslaw's Dock control](https://github.com/wieslawsoltes/Dock), install the `Classic.Avalonia.Theme.Dock` package and include the style in `App.axaml`:
```xml
<StyleInclude Source="avares://Classic.Avalonia.Theme.Dock/Classic.axaml" />
```

### Color Scheme

To customize the color scheme, set the `RequestedThemeVariant` in `App.axaml` or for individual controls:
```xml
RequestedThemeVariant="{x:Static ClassicTheme.Brick}"
```

### Font Usage

While Windows 9x used the bitmap font MS Sans Serif for its distinctive appearance, bitmap fonts have compatibility issues. Instead, the Classic.Avalonia theme uses Tahoma (introduced in Windows XP) without anti-aliasing for a similar look. Though it’s not perfect—letters may sometimes appear too close together, especially on macOS—it’s the closest match for now.

Tahoma is available on Windows and macOS, but not Linux. A free alternative, 'Wine Tahoma Regular,' is distributed under the GNU Lesser General Public License, but I’m unsure if it’s compatible with the MIT license. Any insights would be appreciated.

## Custom Controls

`Classic.Avalonia` introduces several custom controls with a classic appearance and behavior.

These controls (excluding `ClassicWindow`) are part of the `Classic.CommonControls.Avalonia` assembly and can be used independently of `Classic.Avalonia.Theme`. Who knows, maybe a Luna theme is coming next?

### ClassicWindow

By default, windows don't have the classic appearance. To enable a classic titlebar, inherit from `ClassicWindow` instead of the `Window` class.

If inheriting from `ClassicWindow` isn't feasible (for example, if Classic is just one of multiple themes), you can apply the `ClassicWindow` theme like this:
```xml
Theme="{StaticResource ClassicWindow}"
```

**Note**: On Windows 11, the default `Window` class results in rounded corners, which may detract from the classic look. Inherit from `ClassicWindow` to avoid this.

### MessageBox

For a classic Windows-style MessageBox:
```csharp
var result = await MessageBox.ShowDialog(parentWindow, "This is a message box", "Title", MessageBoxButtons.Ok, MessageBoxIcon.Information);
```

To enable classic Windows sounds, add `.UseMessageBoxSounds()` in `Program.cs`:
```csharp
public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
        ...
        .UseMessageBoxSounds()
        ...
```

![MessageBox example](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/messagebox.png)

*(If you can't hear the screenshot above, you might be too young!)*

### ToolBar

MSDN defines a toolbar as "a control containing one or more buttons."

Use `<commonControls:ToolBar>` to host any controls. To add buttons, use `<commonControls:ToolBarButton>` with properties such as `Text`, `SmallIcon` (16px), or `LargeIcon` (24px). Set `IsToggleButton` to enable toggle functionality.

The ToolBar supports small and large sizes, various text placements (Down, Right, or NoText), and can automatically grayscale icons (`GrayscaleIcons`).

![ToolBar Example](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/toolbar.png)

### AboutDialog

For a classic Windows 9x-style 'About' dialog:
```csharp
var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://YourAssembly/YourPathToIcon.png")));
await AboutDialog.ShowDialog(parentWindow, "Notepad", "Copyright (C) 1985-1999", bitmap);
```

![AboutDialog Example](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/aboutdialog.png)

### FontDialog

For a classic Windows Font picker dialog:
```csharp
var font = await FontDialog.ShowDialog(parentWindow);
if (font != null)
{
    Font = font.Family;
    FontStyle = font.Style;
    FontWeight = font.Weight;
    FontSize = font.Size;
}
```

![FontDialog Example](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/fontdialog.png)

## Example Screenshots

![Avalonia Visual Basic](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_basic.gif)
![Avalonia Internet Explorer](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_explorer1.png)
![Avalonia Notepad](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/notepad.png)
![Avalonia Internet Explorer Internet Options](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_explorer2.png)
![Avalonia Internet Explorer Internet Options](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_explorer3.png)
![Avalonia Internet Explorer Internet Options](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_explorer4.png)
![Avalonia Internet Explorer Internet Options](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_explorer5.png)
![Avalonia Internet Explorer Internet Options](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_explorer6.png)
![Avalonia Internet Explorer Internet Options](https://raw.githubusercontent.com/BAndysc/Classic.Avalonia/refs/heads/master/samples/examples/avalonia_explorer7.png)