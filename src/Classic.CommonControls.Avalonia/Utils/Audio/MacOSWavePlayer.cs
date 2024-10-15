namespace Classic.CommonControls.Utils.Audio;

internal class MacOSWavePlayer : IWavePlayer
{
    public void Play(string path)
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "afplay",
                Arguments = "\"" + path + "\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
    }
}