using System;
using Avalonia;
using Avalonia.Controls;
using Classic.Avalonia.Theme;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Classic.CommonControls.Dialogs;

namespace AvaloniaExplorer;

public partial class MainWindow : ClassicWindow
{
    private IDisposable? finishLoadingBar;

    public MainWindow()
    {
        InitializeComponent();
        WebView.NewWindowRequested += WebViewOnWebViewNewWindowRequested;
        WebView.NavigationCompleted += NavigationCompleted;
        WebView.NavigationStarted += NavigationStarting;
        LoadUrl(new Uri("https://avaloniaui.net/"));
    }

    private void NavigationStarting(object? sender, WebViewNavigationStartingEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm)
            return;
        vm.Progress = 10;
        finishLoadingBar?.Dispose();
        finishLoadingBar = null;
        UpdateBackForward();

        vm.Url = e.Request?.ToString();
    }

    private void NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs e)
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
            vm.CanGoBack = WebView.CanGoBack;
            vm.CanGoForward = WebView.CanGoForward;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is MainWindowViewModel vm)
        {
            vm.LoadUriRequest += url => WebView.Source = url;
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

    private void WebViewOnWebViewNewWindowRequested(object? sender, WebViewNewWindowRequestedEventArgs e)
    {
        e.Handled = true;
        if (e.Request is not null)
        {
            var newWindow = new MainWindow();
            newWindow.ShowActivated = true;
            newWindow.DataContext = new MainWindowViewModel();
            newWindow.Show();
            newWindow.LoadUrl(e.Request);
        }
    }

    private void LoadUrl(Uri uri)
    {
        WebView.Source = uri;
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
        WebView.Refresh();
    }

    private void Home(object? sender, RoutedEventArgs e)
    {
        WebView.Source = new Uri("https://avaloniaui.net/");
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