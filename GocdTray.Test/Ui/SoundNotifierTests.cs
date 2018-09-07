using System;
using System.Linq;
using System.Media;
using System.Threading;
using GocdTray.App;
using GocdTray.Test.App;
using GocdTray.Ui.Code;
using NUnit.Framework;

namespace GocdTray.Test.Ui
{
    [TestFixture]
    public class SoundNotifierTests
    {
        [Test]
        public void Test()
        {
            var soundNotifier = new SoundNotifier(new ServiceManager(new GocdServiceFactoryFake()));
            soundNotifier.PlayFailedSound();
            Thread.Sleep(5000);
        }
    }
}