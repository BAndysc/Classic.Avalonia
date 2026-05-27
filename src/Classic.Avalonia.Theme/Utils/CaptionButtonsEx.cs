using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Classic.Avalonia.Theme.Utils;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_RestoreButton, typeof(Button))]
[TemplatePart(PART_MinimizeButton, typeof(Button))]
[TemplatePart(PART_FullScreenButton, typeof(Button))]
[PseudoClasses(pseudoClassMinimized, pseudoClassNormal, pseudoClassMaximized, pseudoClassFullscreen, pseudoClassDialog, pseudoClassCantResize)]
public class CaptionButtonsEx : TemplatedControl
{
    private const string PART_CloseButton = "PART_CloseButton";
    private const string PART_RestoreButton = "PART_RestoreButton";
    private const string PART_MinimizeButton = "PART_MinimizeButton";
    private const string PART_FullScreenButton = "PART_FullScreenButton";
    
    private const string pseudoClassMinimized = ":minimized";
    private const string pseudoClassNormal = ":normal";
    private const string pseudoClassMaximized = ":maximized";
    private const string pseudoClassFullscreen = ":fullscreen";
    private const string pseudoClassDialog = ":dialog";
    private const string pseudoClassCantResize = ":cantresize";
    
    private static FieldInfo? showingAsDialogField;

    static CaptionButtonsEx()
    {
        showingAsDialogField = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);
    }
    
    private CompositeDisposable? _disposables;

    private Window? _hostWindow;

    private Button? _closeButton;
    private Button? _minimizeButton;
    private Button? _restoreButton;
    private Button? _fullscreenButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachButtonHandlers();

        _closeButton      = e.NameScope.Find<Button>(PART_CloseButton);
        _minimizeButton   = e.NameScope.Find<Button>(PART_MinimizeButton);
        _restoreButton    = e.NameScope.Find<Button>(PART_RestoreButton);
        _fullscreenButton = e.NameScope.Find<Button>(PART_FullScreenButton);

        AttachButtonHandlers();
    }

    private void AttachButtonHandlers()
    {
        if (_closeButton      != null) _closeButton.Click      += OnCloseClick;
        if (_minimizeButton   != null) _minimizeButton.Click   += OnMinimizeClick;
        if (_restoreButton    != null) _restoreButton.Click    += OnRestoreClick;
        if (_fullscreenButton != null) _fullscreenButton.Click += OnFullscreenClick;
    }

    private void DetachButtonHandlers()
    {
        if (_closeButton      != null) _closeButton.Click      -= OnCloseClick;
        if (_minimizeButton   != null) _minimizeButton.Click   -= OnMinimizeClick;
        if (_restoreButton    != null) _restoreButton.Click    -= OnRestoreClick;
        if (_fullscreenButton != null) _fullscreenButton.Click -= OnFullscreenClick;
    }

    public void Attach(Window hostWindow)
    {
        if (_hostWindow == hostWindow) return;

        _disposables?.Dispose();

        _hostWindow = hostWindow;

        if (showingAsDialogField != null)
        {
            PseudoClasses.Set(pseudoClassDialog, showingAsDialogField.GetValue(hostWindow) is true);
        }

        _disposables = new CompositeDisposable()
        {
            hostWindow.GetObservable(Window.WindowStateProperty).Subscribe(x =>
            {
                PseudoClasses.Set(pseudoClassMinimized, x == WindowState.Minimized);
                PseudoClasses.Set(pseudoClassNormal, x == WindowState.Normal);
                PseudoClasses.Set(pseudoClassMaximized, x == WindowState.Maximized);
                PseudoClasses.Set(pseudoClassFullscreen, x == WindowState.FullScreen);
            }),
            hostWindow.GetObservable(Window.CanResizeProperty).Subscribe(x =>
            {
                PseudoClasses.Set(pseudoClassCantResize, !x);
            }),
        };
    }

    public void Detach()
    {
        DetachButtonHandlers();
        _closeButton = _minimizeButton = _restoreButton = _fullscreenButton = null;

        if (_disposables != null)
        {
            _disposables.Dispose();
            _disposables = null;
        }
        _hostWindow = null;
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        if (_hostWindow != null)
        {
            _hostWindow.Close();
        }
    }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e)
    {
        if (_hostWindow != null)
        {
            _hostWindow.WindowState = WindowState.Minimized;
        }
    }

    private void OnRestoreClick(object? sender, RoutedEventArgs e)
    {
        if (_hostWindow != null)
        {
            _hostWindow.WindowState = _hostWindow.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }

    private void OnFullscreenClick(object? sender, RoutedEventArgs e)
    {
        if (_hostWindow != null)
        {
            _hostWindow.WindowState = _hostWindow.WindowState == WindowState.FullScreen
                ? WindowState.Normal
                : WindowState.FullScreen;
        }
    }
}