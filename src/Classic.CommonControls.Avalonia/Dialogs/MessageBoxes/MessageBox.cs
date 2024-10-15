using Avalonia;
using Classic.CommonControls.Utils;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace Classic.CommonControls.Dialogs;

public class MessageBox : TemplatedControl
{
    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<MessageBox, string?>(nameof(Text));
    public static readonly StyledProperty<MessageBoxIcon> IconProperty = AvaloniaProperty.Register<MessageBox, MessageBoxIcon>(nameof(Icon));
    public static readonly StyledProperty<MessageBoxButtons> ButtonsProperty = AvaloniaProperty.Register<MessageBox, MessageBoxButtons>(nameof(Buttons), MessageBoxButtons.Ok);
    private IReadOnlyList<MessageBoxResult> buttonsSource = [MessageBoxResult.Ok];
    public static readonly DirectProperty<MessageBox, IReadOnlyList<MessageBoxResult>> ButtonsSourceProperty = AvaloniaProperty.RegisterDirect<MessageBox, IReadOnlyList<MessageBoxResult>>("ButtonsSource", o => o.ButtonsSource, (o, v) => o.ButtonsSource = v);

    public event Action<MessageBoxResult>? AcceptRequest;

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public MessageBoxIcon Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public MessageBoxButtons Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public IReadOnlyList<MessageBoxResult> ButtonsSource
    {
        get => buttonsSource;
        set => SetAndRaise(ButtonsSourceProperty, ref buttonsSource, value);
    }

    public void PickOption(MessageBoxResult result)
    {
        AcceptRequest?.Invoke(result);
    }

    static MessageBox()
    {
        ButtonsProperty.Changed.AddClassHandler<MessageBox>((msgBox, _) =>
        {
            IReadOnlyList<MessageBoxResult> btns;
            switch (msgBox.Buttons)
            {
                case MessageBoxButtons.Ok:
                    btns = [MessageBoxResult.Ok];
                    break;
                case MessageBoxButtons.OkCancel:
                    btns = [MessageBoxResult.Ok, MessageBoxResult.Cancel];
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    btns = [MessageBoxResult.Abort, MessageBoxResult.Retry, MessageBoxResult.Ignore];
                    break;
                case MessageBoxButtons.YesNoCancel:
                    btns = [MessageBoxResult.Yes, MessageBoxResult.No, MessageBoxResult.Cancel];
                    break;
                case MessageBoxButtons.YesNo:
                    btns = [MessageBoxResult.Yes, MessageBoxResult.No];
                    break;
                case MessageBoxButtons.RetryCancel:
                    btns = [MessageBoxResult.Retry, MessageBoxResult.Cancel];
                    break;
                case MessageBoxButtons.CancelTryContinue:
                    btns = [MessageBoxResult.Cancel, MessageBoxResult.TryAgain, MessageBoxResult.Continue];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            msgBox.SetCurrentValue(ButtonsSourceProperty, btns);
        });
    }

    public static async Task<MessageBoxResult> ShowDialog(Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        var window = owner.CreateDefaultWindow();
        window.Title = caption;
        var content = new MessageBox()
        {
            Text = text,
            Buttons = buttons,
            Icon = icon
        };
        window.Content = content;
        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.CanResize = false;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        content.AcceptRequest += result =>
        {
            window.Close(result);
        };

        return await window.ShowDialog<MessageBoxResult>(owner);
    }

}