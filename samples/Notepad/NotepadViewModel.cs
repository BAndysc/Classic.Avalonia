using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Classic.CommonControls.Dialogs;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Notepad;

public class NotepadViewModel : INotifyPropertyChanged
{
    public FontFamily Font { get; private set; } = FontManager.Current.DefaultFontFamily;
    public FontStyle FontStyle { get; private set; } = FontStyle.Normal;
    public FontWeight FontWeight { get; private set; } = FontWeight.Normal;
    public double FontSize { get; private set; } = 11;

    public async void SelectFont(Window parent)
    {
        var font = await FontDialog.ShowDialog(parent);
        if (font == null)
            return;

        Font = font.Family;
        FontStyle = font.Style;
        FontWeight = font.Weight;
        FontSize = font.Size;
        OnPropertyChanged(nameof(Font));
        OnPropertyChanged(nameof(FontStyle));
        OnPropertyChanged(nameof(FontWeight));
        OnPropertyChanged(nameof(FontSize));
    }

    public async void About(Window parent)
    {
        await AboutDialog.ShowDialog(parent, new AboutDialogOptions()
        {
            Title = "Notepad",
            Copyright = "Copyleft (R) 2024 BAndysc",
            Icon = new Bitmap(AssetLoader.Open(new Uri("avares://Notepad/icon32.png")))
        });
    }

    public async void NotImplemented(Window parent)
    {
        await MessageBox.ShowDialog(parent, "Not implemented", "Error", MessageBoxButtons.Ok, MessageBoxIcon.Error);
    }


    #region INPC

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}