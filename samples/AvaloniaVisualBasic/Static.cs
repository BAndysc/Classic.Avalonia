using System;

namespace AvaloniaVisualBasic;

public class Static
{
    public static bool SupportsWindowing { get; } = OperatingSystem.IsWindows() ||
                                             OperatingSystem.IsLinux() ||
                                             OperatingSystem.IsMacOS();
}