using System;
using Avalonia;
using Avalonia.Controls;
using AvaloniaVisualBasic.Forms.ViewModels;
using AvaloniaVisualBasic.Forms.Views;
using Classic.Avalonia.Theme;

namespace AvaloniaVisualBasic;

public partial class MainWindow : ClassicWindow
{
    private bool projectWindowsShown;
    public MainWindow()
    {
        InitializeComponent();

        Activated += OnActivated;

        void OnActivated(object? sender, EventArgs e)
        {
            if (projectWindowsShown)
                return;

            projectWindowsShown = true;

            var window = new ClassicWindow()
            {
                Content = new NewProjectView()
                {
                    DataContext = new NewProjectViewModel()
                },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "New Project",
                CanResize = false,
            };
            window.ShowDialog(this);
        }
    }
}