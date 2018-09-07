using System.Media;
using GocdTray.App.Abstractions;
using GocdTray.Ui.Properties;

namespace GocdTray.Ui.Code
{
    public class SoundNotifier
    {
        private readonly SoundPlayer failedSound = new SoundPlayer(Resources.SadTrombone);
        public SoundNotifier(IServiceManager serviceManager)
        {
            serviceManager.OnBuildFailed += PlayFailedSound;
        }

        public void PlayFailedSound() => failedSound.Play();
    }
}