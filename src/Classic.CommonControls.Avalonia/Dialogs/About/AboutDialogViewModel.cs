using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Avalonia.Media;

namespace Classic.CommonControls.Dialogs;

public class AboutDialogViewModel : INotifyPropertyChanged
{
    private string? title;
    public string? Title
    {
        get => title;
        set => SetField(ref title, value);
    }

    private string? subTitle;
    public string? SubTitle
    {
        get => subTitle;
        set => SetField(ref subTitle, value);
    }

    private string? copyright;
    public string? Copyright
    {
        get => copyright;
        set => SetField(ref copyright, value);
    }

    private IImage? icon;
    public IImage? Icon
    {
        get => icon;
        set => SetField(ref icon, value);
    }

    public string OperatingSystemName { get; }

    public string OperatingSystem { get; }

    public string Username { get; }

    public int FreeResources { get; }

    public int TotalMemory { get; }

    public event Action? CloseRequested;

    public AboutDialogViewModel(AboutDialogOptions options)
    {
        OperatingSystemName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
            (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" :
            (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "macOS" : "Other"));
        OperatingSystem = $"{OperatingSystemName} {Environment.OSVersion.Version.Major}";
        Username = Environment.UserName;
        var memoryMetrics = new MemoryMetricsClient().GetMetrics();
        FreeResources = (int)(memoryMetrics.Free / memoryMetrics.Total * 100);
        TotalMemory = (int)(memoryMetrics.Total / 1024 / 1024);

        Title = options.Title;
        SubTitle = options.SubTitle;
        Copyright = options.Copyright;
        Icon = options.Icon;
    }

    public void Close()
    {
        CloseRequested?.Invoke();
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