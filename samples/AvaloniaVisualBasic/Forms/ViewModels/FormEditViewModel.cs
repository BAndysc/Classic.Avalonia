using System;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AvaloniaVisualBasic.Forms.ViewModels;

public class FormEditViewModel : BaseEditorWindowViewModel
{
    public override string Title { get; } = "Project1 - Form1 (Form)";
    public override IImage Icon { get; } = new Bitmap(AssetLoader.Open(new Uri("avares://AvaloniaVisualBasic/Icons/form.gif")));
}