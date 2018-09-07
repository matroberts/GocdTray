using System;
using System.Collections.Generic;
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
            var gocdServiceFactory = new GocdServiceFactoryFake();
            gocdServiceFactory.GocdService.Pipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>());
            var service = new ServiceManager(gocdServiceFactory);

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

        [Test]
        public void TestConnectionInfo_ShouldReturnError_IfGetPipelinesCallFails()
        {
            // Arrange
            var gocdServiceFactory = new GocdServiceFactoryFake();
            gocdServiceFactory.GocdService.Pipelines = Result<List<Pipeline>>.Invalid("An error occurred while sending the request. The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel.");
            var service = new ServiceManager(gocdServiceFactory);

            // Act
            var result = service.TestConnectionInfo(new ConnectionInfo { GocdApiUri = "https://example.com", GocdWebUri = "http://example.com", IgnoreCertificateErrors = false, Password = "mypassword", PollingIntervalSeconds = 5, Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));

            Assert.That(result.Messages[0].Message, Is.EqualTo("An error occurred while sending the request. The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel."));
        }

        [Test]
        public void TestConnectionInfo_ShouldReturnValid_IfGetPipelinesCallSuceeds()
        {
            // Arrange
            var gocdServiceFactory = new GocdServiceFactoryFake();
            gocdServiceFactory.GocdService.Pipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>());
            var service = new ServiceManager(gocdServiceFactory);

            // Act
            var result = service.TestConnectionInfo(new ConnectionInfo { GocdApiUri = "https://example.com", GocdWebUri = "http://example.com", IgnoreCertificateErrors = false, Password = "mypassword", PollingIntervalSeconds = 5, Username = "myusername" });

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Messages.Count, Is.EqualTo(0));
        }

        #endregion

        #region PollAndUpdate

        [Test]
        public void PollAndUpdate_ShouldRaiseAnOnStatusChangedEvent_WhenItIsCalled()
        {

            // Arrange
            var gocdServiceFactory = new GocdServiceFactoryFake();
            gocdServiceFactory.GocdService.Pipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>());
            var service = new ServiceManager(gocdServiceFactory);
            bool onStatusChangeCalled = false;
            service.OnStatusChange += () => onStatusChangeCalled = true;

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(onStatusChangeCalled, Is.True);
        }

        [TestCase(EstateStatus.NotConnected, EstateStatus.NotConnected, false)]
        [TestCase(EstateStatus.NotConnected, EstateStatus.Building,     false)]
        [TestCase(EstateStatus.NotConnected, EstateStatus.Failed,       false)]
        [TestCase(EstateStatus.NotConnected, EstateStatus.Passed,       false)]
        [TestCase(EstateStatus.Building,     EstateStatus.NotConnected, false)]
        [TestCase(EstateStatus.Building,     EstateStatus.Building,     false)]
        [TestCase(EstateStatus.Building,     EstateStatus.Failed,       true)]
        [TestCase(EstateStatus.Building,     EstateStatus.Passed,       false)]
        [TestCase(EstateStatus.Passed,       EstateStatus.NotConnected, false)]
        [TestCase(EstateStatus.Passed,       EstateStatus.Building,     false)]
        [TestCase(EstateStatus.Passed,       EstateStatus.Failed,       true)]
        [TestCase(EstateStatus.Passed,       EstateStatus.Passed,       false)]
        [TestCase(EstateStatus.Failed,       EstateStatus.NotConnected, false)]
        [TestCase(EstateStatus.Failed,       EstateStatus.Building,     false)]
        [TestCase(EstateStatus.Failed,       EstateStatus.Failed,       false)]
        [TestCase(EstateStatus.Failed,       EstateStatus.Passed,       false)]
        public void PollAndUpdate_ShouldSetTheLastEstateStatus_And_RaiseAnOnBuildFailedEvent_WhenTheBuilldStatusChangesToFailed(EstateStatus previousStatus, EstateStatus nextStatus, bool expectedOnBuildFailedCalled)
        {
            // Arrange
            Result<List<Pipeline>> pipelines = null;
            switch (nextStatus)
            {
                case EstateStatus.NotConnected:
                    pipelines = Result<List<Pipeline>>.Invalid("error");
                    break;
                case EstateStatus.Failed:
                    pipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>(){new Pipeline(){PipelineInstances = {new PipelineInstance(){Stages = new List<Stage>(){new Stage(){Status = StageStatus.Failed}}}}}});
                    break;
                case EstateStatus.Building:
                    pipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Building } } } } } });
                    break;
                case EstateStatus.Passed:
                    pipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Passed } } } } } });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextStatus), nextStatus, null);
            }

            var gocdServiceFactory = new GocdServiceFactoryFake {GocdService = {Pipelines = pipelines } };
            var service = new ServiceManager(gocdServiceFactory) { LastEstateStatus = previousStatus };
            bool onBuildFailedCalled = false;
            service.OnBuildFailed += () => onBuildFailedCalled = true;

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(service.LastEstateStatus, Is.EqualTo(nextStatus));
            Assert.That(onBuildFailedCalled, Is.EqualTo(expectedOnBuildFailedCalled));
        }

        #endregion
    }
}