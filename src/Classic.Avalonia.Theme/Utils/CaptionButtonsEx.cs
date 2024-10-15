using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;

namespace Classic.Avalonia.Theme.Utils;

internal class CaptionButtonsEx : CaptionButtons
{
    private static FieldInfo? showingAsDialogField;

    static CaptionButtonsEx()
    {
        showingAsDialogField = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    protected override Type StyleKeyOverride => typeof(CaptionButtons);
    private IDisposable? disposable;

    public override void Attach(Window hostWindow)
    {
        base.Attach(hostWindow);

        if (showingAsDialogField != null)
        {
            PseudoClasses.Set(":dialog", showingAsDialogField.GetValue(hostWindow) is true);
        }

        disposable = hostWindow.GetObservable(Window.CanResizeProperty).Subscribe(x =>
        {
            PseudoClasses.Set(":cantresize", !x);
        });
    }

    public override void Detach()
    {
        disposable?.Dispose();
        disposable = null;
        base.Detach();
    }
}