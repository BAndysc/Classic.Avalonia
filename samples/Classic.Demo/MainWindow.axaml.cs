using System;
using Avalonia;
using Classic.Avalonia.Theme;
using Classic.CommonControls.Dialogs;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Styling;

namespace Classic.Demo;

public partial class MainWindow : ClassicWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void Spinner_OnSpin(object? sender, SpinEventArgs e)
    {
        if (sender is not ButtonSpinner spinner)
            return;

        int num = (spinner.Tag as int?) ?? 0;
        num += e.Direction == SpinDirection.Decrease ? -1 : 1;
        if (num < 0)
            num += 5;
        num %= 5;
        spinner.Tag = num;
        spinner.Content = num switch
        {
            0 => "Newbie Avalonia",
            1 => "Moderate Avalonia",
            2 => "Advanced Avalonia",
            3 => "Avalonia Expert",
            4 => "Avalonia Nightmare",
        };
    }

    private async void OpenSaveDialog(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });
    }

    private async void OpenAboutDialog(object? sender, RoutedEventArgs e)
    {
        var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://Classic.Demo/icon16.png")));
        await AboutDialog.ShowDialog(this, new AboutDialogOptions()
        {
            Title = "Avalonia",
            Copyright = "Copyleft (R) 2024",
            Icon = bitmap
        });
    }

    private async void OpenFontDialog(object? sender, RoutedEventArgs e)
    {
        await FontDialog.ShowDialog(this);
    }

    private async void OpenMessageBox(object? sender, RoutedEventArgs e)
    {
        await MessageBox.ShowDialog(this,
            MessageBoxText.Text ?? "",
            MessageBoxCaption.Text ?? "",
            (MessageBoxButtons?)((ComboBoxItem?)MessageBoxButtons.SelectedItem)?.Content ?? default,
            (MessageBoxIcon?)((ComboBoxItem?)MessageBoxIcon.SelectedItem)?.Content ?? default);
    }

    private async void OpenFindDialog(object? sender, RoutedEventArgs e)
    {
    }

    private void OnThemeChanged(object? sender, SelectionChangedEventArgs e)
    {
        Application.Current.RequestedThemeVariant = e.AddedItems[0] as ThemeVariant;
    }
}