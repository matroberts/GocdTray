using System;
using System.Linq;
using GocdTray.App;
using GocdTray.App.Abstractions;
using NUnit.Framework;

namespace GocdTray.Test.App
{
    [TestFixture]
    public class ServiceManagerTests
    {
        [Test]
        public void SetConnectionInfo_IfConnectionInfoIsValid_ShouldBeSaved()
        {
            // Arrange
            var service = new ServiceManager();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { GocdApiUri = "https://example.com", IgnoreCertificateErrors = false, Password = "mypassword", PollingIntervalSeconds = 5, Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.True);
            var connectionInfo = service.GetConnectionInfo();
            Assert.That(connectionInfo.GocdApiUri, Is.EqualTo("https://example.com"));
            Assert.That(connectionInfo.IgnoreCertificateErrors, Is.False);
            Assert.That(connectionInfo.Password, Is.EqualTo("mypassword"));
            Assert.That(connectionInfo.PollingIntervalSeconds, Is.EqualTo(5));
            Assert.That(connectionInfo.Username, Is.EqualTo("myusername"));
        }

        [Test]
        public void SetConnectionInfo_ShouldReturnValidationError_IfGocdApiUrl_Username_Or_Password_IsNotSet()
        {
            // Arrange
            var service = new ServiceManager();
            var connectionInfo = service.GetConnectionInfo();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { GocdApiUri = string.Empty, Password = string.Empty, Username = string.Empty, IgnoreCertificateErrors = false, PollingIntervalSeconds = 30, });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(service.GetConnectionInfo(), Is.EqualTo(connectionInfo));
            Assert.That(result.Messages.Count, Is.EqualTo(3));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdApiUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid Gocd Url."));

            Assert.That(result.Messages[1].Property, Is.EqualTo(nameof(ConnectionInfo.Username)));
            Assert.That(result.Messages[1].Message, Is.EqualTo("You must supply a username."));

            Assert.That(result.Messages[2].Property, Is.EqualTo(nameof(ConnectionInfo.Password)));
            Assert.That(result.Messages[2].Message, Is.EqualTo("You must supply a password."));
        }

        [Test]
        public void SetConnectionInfo_ShouldReturnError_IfUrlIsNotValid()
        {
            // Arrange
            var service = new ServiceManager();
            var connectionInfo = service.GetConnectionInfo();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { GocdApiUri = "https://", PollingIntervalSeconds = 5, IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(service.GetConnectionInfo(), Is.EqualTo(connectionInfo));
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdApiUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid Gocd Url."));
        }

        [Test]
        public void SetConnectionInfo_ShouldReturnError_IfPollingIntervalIsLessThan5Seconds()
        {
            // Arrange
            var service = new ServiceManager();
            var connectionInfo = service.GetConnectionInfo();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { PollingIntervalSeconds = 4, GocdApiUri = "https://example.com", IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(service.GetConnectionInfo(), Is.EqualTo(connectionInfo));
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.PollingIntervalSeconds)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("Polling interval must be at least 5 seconds."));
        }
    }
}