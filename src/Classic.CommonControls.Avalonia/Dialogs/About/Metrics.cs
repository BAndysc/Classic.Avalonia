using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Classic.CommonControls.Dialogs;

internal struct MemoryMetrics
{
    public double Total;
    public double Used;
    public double Free;
}

internal class MemoryMetricsClient
{
    public MemoryMetrics GetMetrics()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return GetWindowsMetrics();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return GetMacOsMetrics();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return GetLinuxMetrics();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return default;
    }

    private MemoryMetrics GetWindowsMetrics()
    {
        var output = "";

        var info = new ProcessStartInfo();
        info.FileName = "wmic";
        info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
        info.RedirectStandardOutput = true;
        info.CreateNoWindow = true;

        using(var process = Process.Start(info))
        {
            output = process?.StandardOutput.ReadToEnd();
        }

        var lines = output?.Trim().Split('\n');
        var freeMemoryParts = lines?[0].Split(['='], StringSplitOptions.RemoveEmptyEntries);
        var totalMemoryParts = lines?[1].Split(['='], StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics();
        metrics.Total = Math.Round(double.Parse(totalMemoryParts?[1] ?? "0") / 1024, 0);
        metrics.Free = Math.Round(double.Parse(freeMemoryParts?[1] ?? "0") / 1024, 0);
        metrics.Used = metrics.Total - metrics.Free;

        return metrics;
    }

    private MemoryMetrics GetMacOsMetrics()
    {
        var output = "";

        var info = new ProcessStartInfo("free -m");
        info.FileName = "/bin/bash";
        info.Arguments = "-c \"sysctl hw.memsize\"";
        info.RedirectStandardOutput = true;
        info.CreateNoWindow = true;

        using(var process = Process.Start(info))
        {
            output = process?.StandardOutput.ReadToEnd();
        }

        var memory = output?.Split([' '], StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics();
        metrics.Total = double.Parse(memory?[1] ?? "0");

        return metrics;
    }

    private MemoryMetrics GetLinuxMetrics()
    {
        var output = "";

        var info = new ProcessStartInfo("free -m");
        info.FileName = "/bin/bash";
        info.Arguments = "-c \"free -m\"";
        info.RedirectStandardOutput = true;
        info.CreateNoWindow = true;

        using(var process = Process.Start(info))
        {
            output = process?.StandardOutput.ReadToEnd();
        }

        var lines = output?.Split('\n');
        var memory = lines?[1].Split([' '], StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics();
        metrics.Total = double.Parse(memory?[1] ?? "0");

        return metrics;
    }
}