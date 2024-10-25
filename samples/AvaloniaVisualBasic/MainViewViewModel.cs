using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.VisualTree;
using AvaloniaVisualBasic.Forms.ViewModels;
using Classic.Avalonia.Theme;

namespace AvaloniaVisualBasic;

public class MainViewViewModel : INotifyPropertyChanged
{
    public List<PropertyViewModel> Properties { get; } = new()
    {
        new("(Name)", "Form1"),
        new("Appearance", "1 - 3D"),
        new("AutoRedraw", "False"),
        new("BackColor", "&H8000000"),
        new("BorderStyle", "2 - Sizable"),
        new("Caption", "Form1"),
        new("ClipControls", "True"),
        new("ControlBox", "True"),
        new("DrawMode", "13 - Copy Pen"),
        new("DrawStyle", "0 - Solid"),
        new("DrawWidth", "1"),
        new("Enabled", "True"),
        new("FillColor", "&H0000000"),
        new("FillStyle", "1 - Transparent"),
        new("Font", "MS Sans Serif"),
        new("FontTransparent", "True"),
        new("ForeColor", "&H8000001"),
        new("HasDC", "True"),
        new("Height", "3600"),
        new("HelpContextID", "0"),
        new("Icon", "(Icon)"),
        new("KeyPreview", "False"),
        new("Left", "0"),
        new("LinkMode", "0 - None"),
        new("LinkTopic", "Form1"),
        new("MaxButton", "True"),
        new("MDIChild", "False"),
        new("MinButton", "True"),
        new("MouseIcon", "(None)"),
        new("MousePointer", "0 - Default"),
        new("Moveable", "True"),
        new("NegotiateMenus", "True"),
        new("OLEDropMode", "0 - None"),
        new("Palette", "(None)"),
        new("PaletteMode", "0 - Halftone"),
        new("Picture", "(None)"),
        new("RightToLeft", "False"),
        new("ScaleHeight", "3195"),
        new("ScaleLeft", "0"),
        new("ScaleMode", "1 - Twip"),
        new("ScaleTop", "0"),
        new("ScaleWidth", "4680"),
        new("ShowInTaskbar", "True"),
        new("StartUpPosition", "3 - Windows Default"),
        new("Tag", ""),
        new("Top", "0"),
        new("Visible", "True"),
        new("WhatsThisButton", "False"),
        new("WhatsThisHelp", "False"),
        new("Width", "4800"),
        new("WindowState", "0 - Normal"),
    };

    private PropertyViewModel? selectedProperty;
    public PropertyViewModel? SelectedProperty
    {
        get => selectedProperty;
        set => SetField(ref selectedProperty, value);
    }

    public DelegateCommand<BaseEditorWindowViewModel> CloseWindowCommand { get; }
    public DelegateCommand<Control> OpenMenuEditorCommand { get; }

    public ObservableCollection<BaseEditorWindowViewModel> Windows { get; } = new()
    {
        new FormEditViewModel(),
        new CodeEditorViewModel()
    };

    public MainViewViewModel()
    {
        SelectedProperty = Properties.FirstOrDefault();
        CloseWindowCommand = new DelegateCommand<BaseEditorWindowViewModel>(window => Windows.Remove(window), _ => true);
        OpenMenuEditorCommand = new DelegateCommand<Control>(host =>
        {
            new ClassicWindow()
            {
                Title = "Menu editor",
                CanResize = false,
                Content = new MenuEditorViewModel(),
                SizeToContent = SizeToContent.WidthAndHeight
            }.ShowDialog(host.GetVisualRoot() as Window);
        }, _ => true);
    }

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
}


public class PropertyViewModel
{
    public PropertyViewModel(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public string Value { get; }
}