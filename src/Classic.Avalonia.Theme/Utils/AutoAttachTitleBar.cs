using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Classic.Avalonia.Theme.Utils;

[TemplatePart("PART_CaptionButtons", typeof(CaptionButtons), IsRequired = true)]
[PseudoClasses(":minimized", ":normal", ":maximized", ":fullscreen")]
internal class AutoAttachTitleBar : TemplatedControl
{
    private IDisposable? _disposables;
    private CaptionButtons? _captionButtons;

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _captionButtons?.Detach();

        _captionButtons = e.NameScope.Get<CaptionButtons>("PART_CaptionButtons");

        if (VisualRoot is Window window)
        {
            _captionButtons?.Attach(window);
        }
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (VisualRoot is Window window)
        {
            _disposables = new CompositeDisposable()
            {
                window.GetObservable(Window.WindowStateProperty)
                    .Subscribe(x =>
                    {
                        PseudoClasses.Set(":minimized", x == WindowState.Minimized);
                        PseudoClasses.Set(":normal", x == WindowState.Normal);
                        PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                        PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
                    }),
                window.GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(x =>
                    {
                        PseudoClasses.Set(":active", x);
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

    public class CompositeDisposable : List<IDisposable>, IDisposable
    {
        public void Dispose()
        {
            foreach (var x in this)
                x.Dispose();
        }
    }
}