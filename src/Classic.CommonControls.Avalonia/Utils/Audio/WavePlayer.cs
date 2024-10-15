using System.Runtime.InteropServices;

namespace Classic.CommonControls.Utils.Audio;

internal class WavePlayer : IWavePlayer
{
    private IWavePlayer impl;

    public static IWavePlayer Player { get; } = new WavePlayer();

    public WavePlayer()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            impl = new WindowsWavePlayer();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            impl = new MacOSWavePlayer();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            impl = new LinuxWavePlayer();
        else
            impl = new NullWavePlayer();
    }

    public void Play(string path)
    {
        try
        {
            impl.Play(path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}