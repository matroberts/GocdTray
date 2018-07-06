using System;
using System.Linq;
using GocdTray.App;
using NUnit.Framework;

namespace GocdTray.Test
{
    [TestFixture]
    public class AppConfigTests
    {
        [Test]
        public void AppConfig_ContainsTheConnectionInfo_AndItIsPopulatedFromFile()
        {
            Console.WriteLine(AppConfig.GocdApiUri);
            Console.WriteLine(AppConfig.Username);
            Console.WriteLine(AppConfig.Password);
            Console.WriteLine(AppConfig.IgnoreCertificateErrors);

            Assert.That(AppConfig.GocdApiUri, Is.Not.Empty);
            Assert.That(AppConfig.Username, Is.Not.Empty);
            Assert.That(AppConfig.Password, Is.Not.Empty);
            Assert.That(AppConfig.IgnoreCertificateErrors, Is.True);
        }
    }
}