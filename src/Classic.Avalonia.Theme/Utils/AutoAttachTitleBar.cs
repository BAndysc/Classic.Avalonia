using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Classic.Avalonia.Theme.Utils;

[TemplatePart(PART_CaptionButtons, typeof(CaptionButtonsEx), IsRequired = true)]
[PseudoClasses(pseudoClassMinimized, pseudoClassNormal, pseudoClassMaximized, pseudoClassFullscreen, pseudoClassActive)]
public class AutoAttachTitleBar : TemplatedControl
{
    private const string PART_CaptionButtons = "PART_CaptionButtons";
    
    private const string pseudoClassMinimized = ":minimized";
    private const string pseudoClassNormal = ":normal";
    private const string pseudoClassMaximized = ":maximized";
    private const string pseudoClassFullscreen = ":fullscreen";
    private const string pseudoClassActive = ":active";
    
    private IDisposable? _disposables;
    private CaptionButtonsEx? _captionButtons;

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _captionButtons?.Detach();

        _captionButtons = e.NameScope.Get<CaptionButtonsEx>(PART_CaptionButtons);

        if (TopLevel.GetTopLevel(this) is Window window)
        {
            _captionButtons?.Attach(window);
        }
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (TopLevel.GetTopLevel(this) is Window window)
        {
            _captionButtons?.Attach(window);
            _disposables = new CompositeDisposable()
            {
                window.GetObservable(Window.WindowStateProperty)
                    .Subscribe(x =>
                    {
                        PseudoClasses.Set(pseudoClassMinimized, x == WindowState.Minimized);
                        PseudoClasses.Set(pseudoClassNormal, x == WindowState.Normal);
                        PseudoClasses.Set(pseudoClassMaximized, x == WindowState.Maximized);
                        PseudoClasses.Set(pseudoClassFullscreen, x == WindowState.FullScreen);
                    }),
                window.GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(x =>
                    {
                        PseudoClasses.Set(pseudoClassActive, x);
                    }),
            };
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _disposables?.Dispose();

        _captionButtons?.Detach();
        _captionButtons = null;
    }
}