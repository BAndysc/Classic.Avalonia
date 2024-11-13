using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Threading;
using Classic.CommonControls.Utils;

namespace Classic.CommonControls.Dialogs;

public class InputBox : TemplatedControl, ICommand
{
    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<InputBox, string?>("Text");
    public static readonly StyledProperty<string?> PromptProperty = AvaloniaProperty.Register<InputBox, string?>("Prompt");

    public event Action<string?>? TextRequest;

    private TextBox? textBox;

    public string? Prompt
    {
        get => GetValue(PromptProperty);
        set => SetValue(PromptProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var okButton = e.NameScope.Get<Button>("PART_OkButton");
        var cancelButton = e.NameScope.Get<Button>("PART_CancelButton");
        textBox = e.NameScope.Get<TextBox>("PART_Text");

        okButton.Click += (_, _) => TextRequest?.Invoke(Text);
        cancelButton.Click += (_, _) => TextRequest?.Invoke(null);
        textBox.KeyBindings.Add(new KeyBinding()
        {
            Gesture = new KeyGesture(Key.Enter),
            Command = this
        });
        textBox.Focus();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        textBox?.Focus();
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => TextRequest?.Invoke(Text);

    public event EventHandler? CanExecuteChanged;

    public static async Task<string?> ShowDialog(Window owner, string prompt, string caption, string defaultText)
    {
        var window = owner.CreateDefaultWindow();
        window.Title = caption;
        var content = new InputBox()
        {
            Text = defaultText,
            Prompt = prompt
        };
        window.Content = content;
        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.CanResize = false;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        content.TextRequest += result =>
        {
            window.Close(result);
        };

        var result =  await window.ShowDialog<string?>(owner);
        return result;
    }
}