using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Classic.Avalonia.Theme.Utils;

namespace AvaloniaVisualBasic.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_RestoreButton, typeof(Button))]
[TemplatePart(PART_MinimizeButton, typeof(Button))]
[PseudoClasses(pseudoClassMinimized, pseudoClassNormal, pseudoClassMaximized, pseudoClassFullscreen)]
public class MDICaptionButtons : TemplatedControl
{
    private const string PART_CloseButton = "PART_CloseButton";
    private const string PART_RestoreButton = "PART_RestoreButton";
    private const string PART_MinimizeButton = "PART_MinimizeButton";
    
    private const string pseudoClassMinimized = ":minimized";
    private const string pseudoClassNormal = ":normal";
    private const string pseudoClassMaximized = ":maximized";
    private const string pseudoClassFullscreen = ":fullscreen";

    protected override Type StyleKeyOverride => typeof(CaptionButtonsEx);

    private IDisposable? _disposables;

    private Button? _closeButton;
    private Button? _minimizeButton;
    private Button? _restoreButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachButtonHandlers();

        _closeButton    = e.NameScope.Find<Button>(PART_CloseButton);
        _minimizeButton = e.NameScope.Find<Button>(PART_MinimizeButton);
        _restoreButton  = e.NameScope.Find<Button>(PART_RestoreButton);

        AttachButtonHandlers();
    }

    private void AttachButtonHandlers()
    {
        if (_closeButton    != null) _closeButton.Click    += OnCloseClick;
        if (_minimizeButton != null) _minimizeButton.Click += OnMinimizeClick;
        if (_restoreButton  != null) _restoreButton.Click  += OnRestoreClick;
    }

    private void DetachButtonHandlers()
    {
        if (_closeButton    != null) _closeButton.Click    -= OnCloseClick;
        if (_minimizeButton != null) _minimizeButton.Click -= OnMinimizeClick;
        if (_restoreButton  != null) _restoreButton.Click  -= OnRestoreClick;
    }

    private void Attach(MDIWindow hostWindow)
    {
        if (_disposables == null)
        {
            _disposables = hostWindow.GetObservable(MDIHostPanel.WindowStateProperty)
                .Subscribe(new ActionObserver<WindowState>(x =>
                {
                    PseudoClasses.Set(pseudoClassMinimized, x == WindowState.Minimized);
                    PseudoClasses.Set(pseudoClassNormal, x == WindowState.Normal);
                    PseudoClasses.Set(pseudoClassMaximized, x == WindowState.Maximized);
                    PseudoClasses.Set(pseudoClassFullscreen, x == WindowState.FullScreen);
                }));
        }
    }

    private void Detach()
    {
        if (_disposables != null)
        {
            _disposables.Dispose();
            _disposables = null;
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (FindHost() is { } host)
            Attach(host);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        DetachButtonHandlers();
        _closeButton = _minimizeButton = _restoreButton = null;
        Detach();
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        if (FindHost() is { } window)
            window.CloseCommand?.Execute(window.CloseCommandParameter);
    }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e)
    {
        if (FindHost() is { } window)
        {
            if (MDIHostPanel.GetWindowState(window) == WindowState.Minimized)
            {
                MDIHostPanel.SetOldMinimizedWindowLocation(window, MDIHostPanel.GetWindowLocation(window));
                MDIHostPanel.SetWindowLocation(window, MDIHostPanel.GetOldWindowLocation(window));
                MDIHostPanel.SetWindowState(window,  WindowState.Normal);
            }
            else
            {
                MDIHostPanel.SetOldWindowLocation(window, MDIHostPanel.GetWindowLocation(window));
                MDIHostPanel.SetWindowState(window,  WindowState.Minimized);
                if (MDIHostPanel.GetOldMinimizedWindowLocation(window) is { } oldMinimizedWindowLocation)
                    MDIHostPanel.SetWindowLocation(window, oldMinimizedWindowLocation);
                else if (this.FindAncestorOfType<MDIHostPanel>() is { } hostPanel)
                    MDIHostPanel.SetWindowLocation(window, new Point(0, hostPanel.Bounds.Height - window.MinHeight));
            }
        }
    }

    private void OnRestoreClick(object? sender, RoutedEventArgs e)
    {
        if (FindHost() is { } window)
        {
            if (MDIHostPanel.GetWindowState(window) == WindowState.Maximized)
            {
                MDIHostPanel.SetWindowState(window,  WindowState.Normal);
            }
            else
            {
                MDIHostPanel.SetWindowState(window,  WindowState.Maximized);
            }
        }
    }

    private MDIWindow? FindHost()
    {
        return this.FindAncestorOfType<MDIWindow>();
    }
}