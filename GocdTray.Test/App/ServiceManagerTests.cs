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
        #region SetConnectionInfo

        [Test]
        public void SetConnectionInfo_IfConnectionInfoIsValid_ShouldBeSaved()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { GocdApiUri = "https://example.com", GocdWebUri = "http://example.com", IgnoreCertificateErrors = false, Password = "mypassword", PollingIntervalSeconds = 5, Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.True);
            var connectionInfo = service.GetConnectionInfo();
            Assert.That(connectionInfo.GocdApiUri, Is.EqualTo("https://example.com"));
            Assert.That(connectionInfo.GocdWebUri, Is.EqualTo("http://example.com"));
            Assert.That(connectionInfo.IgnoreCertificateErrors, Is.False);
            Assert.That(connectionInfo.Password, Is.EqualTo("mypassword"));
            Assert.That(connectionInfo.PollingIntervalSeconds, Is.EqualTo(5));
            Assert.That(connectionInfo.Username, Is.EqualTo("myusername"));
        }

        [Test]
        public void SetConnectionInfo_ShouldReturnValidationError_If_ApiUrl_WebUrl_Username_Or_Password_IsNotSet()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());
            var connectionInfo = service.GetConnectionInfo();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { GocdApiUri = string.Empty, GocdWebUri = string.Empty, Password = string.Empty, Username = string.Empty, IgnoreCertificateErrors = false, PollingIntervalSeconds = 30, });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(service.GetConnectionInfo(), Is.EqualTo(connectionInfo));
            Assert.That(result.Messages.Count, Is.EqualTo(4));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdApiUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid url for the Gocd Api."));

            Assert.That(result.Messages[1].Property, Is.EqualTo(nameof(ConnectionInfo.GocdWebUri)));
            Assert.That(result.Messages[1].Message, Is.EqualTo("You must supply a valid url for the Gocd Website."));

            Assert.That(result.Messages[2].Property, Is.EqualTo(nameof(ConnectionInfo.Username)));
            Assert.That(result.Messages[2].Message, Is.EqualTo("You must supply a username."));

            Assert.That(result.Messages[3].Property, Is.EqualTo(nameof(ConnectionInfo.Password)));
            Assert.That(result.Messages[3].Message, Is.EqualTo("You must supply a password."));
        }

        [Test]
        public void SetConnectionInfo_ShouldReturnError_IfApiUrlIsNotValid()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());
            var connectionInfo = service.GetConnectionInfo();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { GocdApiUri = "https://", GocdWebUri = "http://example.com", PollingIntervalSeconds = 5, IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(service.GetConnectionInfo(), Is.EqualTo(connectionInfo));
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdApiUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid url for the Gocd Api."));
        }

        [Test]
        public void SetConnectionInfo_ShouldReturnError_IfWebUrlIsNotValid()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());
            var connectionInfo = service.GetConnectionInfo();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { GocdApiUri = "http://example.com", GocdWebUri = "http://", PollingIntervalSeconds = 5, IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(service.GetConnectionInfo(), Is.EqualTo(connectionInfo));
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdWebUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid url for the Gocd Website."));
        }

        [Test]
        public void SetConnectionInfo_ShouldReturnError_IfPollingIntervalIsLessThan5Seconds()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());
            var connectionInfo = service.GetConnectionInfo();

            // Act
            var result = service.SetConnectionInfo(new ConnectionInfo { PollingIntervalSeconds = 4, GocdApiUri = "https://example.com", GocdWebUri = "http://example.com", IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(service.GetConnectionInfo(), Is.EqualTo(connectionInfo));
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.PollingIntervalSeconds)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("Polling interval must be at least 5 seconds."));
        }

        #endregion

        #region TestConnectionInfo - Has same validation rules as SetConnectionInfo

        [Test]
        public void TestConnectionInfo_ShouldReturnValidationError_If_ApiUrl_WebUrl_Username_Or_Password_IsNotSet()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());

            // Act
            var result = service.TestConnectionInfo(new ConnectionInfo { GocdApiUri = string.Empty, GocdWebUri = string.Empty, Password = string.Empty, Username = string.Empty, IgnoreCertificateErrors = false, PollingIntervalSeconds = 30, });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(4));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdApiUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid url for the Gocd Api."));

            Assert.That(result.Messages[1].Property, Is.EqualTo(nameof(ConnectionInfo.GocdWebUri)));
            Assert.That(result.Messages[1].Message, Is.EqualTo("You must supply a valid url for the Gocd Website."));

            Assert.That(result.Messages[2].Property, Is.EqualTo(nameof(ConnectionInfo.Username)));
            Assert.That(result.Messages[2].Message, Is.EqualTo("You must supply a username."));

            Assert.That(result.Messages[3].Property, Is.EqualTo(nameof(ConnectionInfo.Password)));
            Assert.That(result.Messages[3].Message, Is.EqualTo("You must supply a password."));
        }

        [Test]
        public void TestConnectionInfo_ShouldReturnError_IfApiUrlIsNotValid()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());

            // Act
            var result = service.TestConnectionInfo(new ConnectionInfo { GocdApiUri = "https://", GocdWebUri = "http://example.com", PollingIntervalSeconds = 5, IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdApiUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid url for the Gocd Api."));
        }

        [Test]
        public void TestConnectionInfo_ShouldReturnError_IfWebUrlIsNotValid()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());

            // Act
            var result = service.TestConnectionInfo(new ConnectionInfo { GocdApiUri = "http://example.com", GocdWebUri = "http://", PollingIntervalSeconds = 5, IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.GocdWebUri)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("You must supply a valid url for the Gocd Website."));
        }

        [Test]
        public void TestConnectionInfo_ShouldReturnError_IfPollingIntervalIsLessThan5Seconds()
        {
            // Arrange
            var service = new ServiceManager(new GocdServiceFactoryFake());

            // Act
            var result = service.TestConnectionInfo(new ConnectionInfo { PollingIntervalSeconds = 4, GocdApiUri = "https://example.com", GocdWebUri = "http://example.com", IgnoreCertificateErrors = false, Password = "mypassword", Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Property, Is.EqualTo(nameof(ConnectionInfo.PollingIntervalSeconds)));
            Assert.That(result.Messages[0].Message, Is.EqualTo("Polling interval must be at least 5 seconds."));
        }

        #endregion
    }
}