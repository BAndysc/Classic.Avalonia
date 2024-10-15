using System;
using Avalonia;
using Avalonia.Controls;
using Classic.Avalonia.Theme;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Classic.CommonControls.Dialogs;
using WebKit;
using WebViewCore.Enums;
using WebViewCore.Events;

namespace AvaloniaExplorer;

public partial class MainWindow : ClassicWindow
{
    private IDisposable finishLoadingBar;

    public MainWindow()
    {
        InitializeComponent();
        WebView.WebViewNewWindowRequested += WebViewOnWebViewNewWindowRequested;
        WebView.NavigationCompleted += NavigationCompleted;
        WebView.NavigationStarting += NavigationStarting;
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void NavigationStarting(object? sender, WebViewUrlLoadingEventArg e)
    {
        if (DataContext is not MainWindowViewModel vm)
            return;
        vm.Progress = 10;
        finishLoadingBar?.Dispose();
        finishLoadingBar = null;
        UpdateBackForward();

        if (e.RawArgs is WKNavigationAction webkit)
        {
            if (webkit.NavigationType == WKNavigationType.Other)
                return;
        }
        vm.Url = e.Url?.ToString();
    }

    private void NavigationCompleted(object? sender, WebViewUrlLoadedEventArg e)
    {
        if (DataContext is not MainWindowViewModel vm)
            return;

        vm.Progress = 100;
        finishLoadingBar = DispatcherTimer.RunOnce(() =>
        {
            vm.Progress = 0;
        }, TimeSpan.FromMilliseconds(500));
        UpdateBackForward();
    }

    private void UpdateBackForward()
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.CanGoBack = WebView.IsCanGoBack;
            vm.CanGoForward = WebView.IsCanGoForward;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is MainWindowViewModel vm)
        {
            vm.LoadUriRequest += url => WebView.Url = url;
            vm.PropertyChanged += (_, prop) =>
            {
                if (prop.PropertyName == nameof(MainWindowViewModel.OpenedPanel))
                {
                    MainGrid.ColumnDefinitions = vm.OpenedPanel == PanelType.None ?
                            ColumnDefinitions.Parse("0,0,*") :
                            new ColumnDefinitions()
                            {
                                new ColumnDefinition(0, GridUnitType.Auto){MinWidth = 200},
                                new ColumnDefinition(3, GridUnitType.Pixel),
                                new ColumnDefinition(1, GridUnitType.Star)
                            };
                }
            };
            vm.IsFavoritesOpened = true;
        }
    }

    private void WebViewOnWebViewNewWindowRequested(object? sender, WebViewNewWindowEventArgs e)
    {
        if (e.UrlLoadingStrategy == UrlRequestStrategy.OpenInNewWindow)
        {
            var newWindow = new MainWindow();
            newWindow.ShowActivated = true;
            newWindow.Show();
            newWindow.LoadUrl(e.Url);
        }
    }

    private void LoadUrl(Uri uri)
    {
        WebView.Url = uri;
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        WebView.GoBack();
    }

    private void GoForward(object? sender, RoutedEventArgs e)
    {
        WebView.GoForward();
    }

    private void Stop(object? sender, RoutedEventArgs e)
    {
        finishLoadingBar?.Dispose();
        if (DataContext is MainWindowViewModel vm)
        {
            vm.Progress = 0;
        }

        WebView.Stop();
    }

    private void Refresh(object? sender, RoutedEventArgs e)
    {
        WebView.Reload();
    }

    private void Home(object? sender, RoutedEventArgs e)
    {
        WebView.Url = new Uri("https://avaloniaui.net/");
    }

    private void Search(object? sender, RoutedEventArgs e)
    {

    }

    private async void Print(object? sender, RoutedEventArgs e)
    {
        await MessageBox.ShowDialog(this,
            "Before you can print, you need to install a printer.\nTo do this, click Start, point to Settings, click Printers, and then double-click Add Printer.",
            "Printing Error", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
    }

    private async void OpenAbout(object? sender, RoutedEventArgs e)
    {
        await AboutDialog.ShowDialog(this, new AboutDialogOptions()
        {
            Title = "Avalonia Internet Explorer",
            Copyright = "Copyleft (R) 2024 bandysc",
            Icon = new Bitmap(AssetLoader.Open(new Uri("avares://AvaloniaExplorer/Icons/icon32.gif")))
        });
    }

    private void InternetOptions(object? sender, RoutedEventArgs e)
    {
        new OptionsDialog().ShowDialog(this);
    }
}