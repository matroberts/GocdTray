using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using GocdTray.App;
using GocdTray.Rest;
using NUnit.Framework;

namespace GocdTray.Test
{
    [TestFixture]
    public class TestConnectingGocd
    {
        // connect to go.cd and get the data back wot i want in an object

        [Test]
        public void RealConnection()
        {
            var gocdServer = new GocdServer(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors);
            var result = gocdServer.GetDashboard();
            
            Assert.That(result.HasData);
            Console.WriteLine(result.Data);

        }

        // remove passwords, so can check-in
        // Http client setup
        // Need to vary accept header on call by call basis
        // How deal with failure to connect, or interuption?
        // Not doing it now but may want to connect to more than one go.cd
        // Need to process the return data, and test the processing
        // async
        // request errors - catach exceptions
        // response errors - validation object
    }
}