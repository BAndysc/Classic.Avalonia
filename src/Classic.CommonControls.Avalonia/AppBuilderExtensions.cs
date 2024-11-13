using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Classic.CommonControls.Dialogs;
using Classic.CommonControls.Utils.Audio;

namespace Classic.CommonControls;

public static class AppBuilderExtensions
{
    public static AppBuilder UseMessageBoxSounds(this AppBuilder builder)
    {
        //string? tempChimeFileName = null;
        string? tempChordFileName = null;
        string? tempDingFileName = null;

        Control.LoadedEvent.AddClassHandler<MessageBox>((msgBox, e) =>
        {
            string? fileName = msgBox.Icon switch
            {
                MessageBoxIcon.Error => tempChordFileName,
                MessageBoxIcon.Warning => tempChordFileName,
                MessageBoxIcon.Information => tempDingFileName,
                MessageBoxIcon.Question => tempDingFileName,
                _ => null
            };

            if (fileName == null)
            {
                Stream? resourceStream = msgBox.Icon switch
                {
                    MessageBoxIcon.Error => AssetLoader.Open(new Uri("avares://Classic.CommonControls.Avalonia/Audio/chord.wav")),
                    MessageBoxIcon.Warning => AssetLoader.Open(new Uri("avares://Classic.CommonControls.Avalonia/Audio/chord.wav")),
                    MessageBoxIcon.Information => AssetLoader.Open(new Uri("avares://Classic.CommonControls.Avalonia/Audio/ding.wav")),
                    MessageBoxIcon.Question => AssetLoader.Open(new Uri("avares://Classic.CommonControls.Avalonia/Audio/ding.wav")),
                    _ => null
                };
                if (resourceStream != null)
                {
                    var array = new byte[resourceStream.Length];
                    int read = resourceStream.Read(array, 0, array.Length);
                    fileName = Path.GetTempFileName();
                    File.WriteAllBytes(fileName, array);
                    if (msgBox.Icon is MessageBoxIcon.Error or MessageBoxIcon.Warning)
                        tempChordFileName = fileName;
                    else
                        tempDingFileName = fileName;
                }
            }

            if (fileName != null)
                WavePlayer.Player.Play(fileName);
        });
        return builder;
    }
}