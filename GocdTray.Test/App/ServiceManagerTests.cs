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

        [Test]
        public void PollAndUpdate_KeepsThePreviousEstate_InPreviousEstate()
        {
            // Arrange
            var gocdServiceFactory = new GocdServiceFactoryFake { GocdService = { Pipelines = new Result<List<Pipeline>>() } };

            var estate = new Estate(Result<List<Pipeline>>.Invalid("error"));
            var service = new ServiceManager(gocdServiceFactory) { Estate = estate };

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(service.PreviousEstate, Is.SameAs(estate));
        }

        [TestCase(PipelineStatus.Building, PipelineStatus.Building, false)]
        [TestCase(PipelineStatus.Building, PipelineStatus.Failed,   true)]
        [TestCase(PipelineStatus.Building, PipelineStatus.Passed,   false)]
        [TestCase(PipelineStatus.Passed,   PipelineStatus.Building, false)]
        [TestCase(PipelineStatus.Passed,   PipelineStatus.Failed,   true)]
        [TestCase(PipelineStatus.Passed,   PipelineStatus.Passed,   false)]
        [TestCase(PipelineStatus.Failed,   PipelineStatus.Building, false)]
        [TestCase(PipelineStatus.Failed,   PipelineStatus.Failed,   false)]
        [TestCase(PipelineStatus.Failed,   PipelineStatus.Passed,   false)]
        public void PollAndUpdate_ShouldRaiseAnOnBuildFailedEvent_AndSetJustFailed_WhenAPipelineStatusChangesToFailed(PipelineStatus previousStatus, PipelineStatus nextStatus, bool expectedOnBuildFailedCalled)
        {
            // Arrange
            Result<List<Pipeline>> previousPipelines = null;
            switch (previousStatus)
            {
                case PipelineStatus.Failed:
                    previousPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } } });
                    break;
                case PipelineStatus.Building:
                    previousPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Building } } } } } });
                    break;
                case PipelineStatus.Passed:
                    previousPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Passed } } } } } });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(previousStatus), previousStatus, null);
            }

            Result<List<Pipeline>> nextPipelines = null;
            switch (nextStatus)
            {
                case PipelineStatus.Failed:
                    nextPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } } });
                    break;
                case PipelineStatus.Building:
                    nextPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Building } } } } } });
                    break;
                case PipelineStatus.Passed:
                    nextPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Passed } } } } } });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextStatus), nextStatus, null);
            }

            var gocdServiceFactory = new GocdServiceFactoryFake { GocdService = { Pipelines = nextPipelines } };
            var service = new ServiceManager(gocdServiceFactory) { Estate = new Estate(previousPipelines) };
            bool onBuildFailedCalled = false;
            service.OnBuildFailed += () => onBuildFailedCalled = true;

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(onBuildFailedCalled, Is.EqualTo(expectedOnBuildFailedCalled));
            Assert.That(service.Estate.Pipelines.Single(p => p.Name=="1").JustFailed, Is.EqualTo(expectedOnBuildFailedCalled));
        }

        [Test]
        public void PollAndUpdate_DoesNotRaiseAn_OnBuildFailedEvent_IfANewPipelineIsAddedWhichIsFailed()
        {
            // Arrange
            var previousPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Passed } } } } } });
            var nextPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>() { new Pipeline() { Name = "2", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } } });

            var gocdServiceFactory = new GocdServiceFactoryFake { GocdService = { Pipelines = nextPipelines } };
            var service = new ServiceManager(gocdServiceFactory) { Estate = new Estate(previousPipelines) };
            bool onBuildFailedCalled = false;
            service.OnBuildFailed += () => onBuildFailedCalled = true;

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(onBuildFailedCalled, Is.False);
            Assert.That(service.Estate.Pipelines.Single(p => p.Name == "2").JustFailed, Is.False);
        }

        [Test]
        public void PollAndUpdate_RaisesASingle_OnBuildFailedEvent_EvenIfTwoPipelinesFail()
        {
            // Arrange
            var previousPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>()
            {
                new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Passed } } } } },
                new Pipeline() { Name = "2", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Passed } } } } },
            });
            var nextPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>()
            {
                new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } },
                new Pipeline() { Name = "2", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } },
            });

            var gocdServiceFactory = new GocdServiceFactoryFake { GocdService = { Pipelines = nextPipelines } };
            var service = new ServiceManager(gocdServiceFactory) { Estate = new Estate(previousPipelines) };
            int numTimesOnBuildFailedCalled = 0;
            service.OnBuildFailed += () => numTimesOnBuildFailedCalled++;

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(numTimesOnBuildFailedCalled, Is.EqualTo(1));
            Assert.That(service.Estate.Pipelines.Select(p => p.JustFailed), Has.All.True);
        }

        [Test]
        public void PollAndUpdate_RaisesAn_OnBuildFailedEvent_IfANewBuildFails_EvenIfAnotherBuildIsAlreadyFailed()
        {
            // Arrange
            var previousPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>()
            {
                new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } },
                new Pipeline() { Name = "2", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Passed } } } } },
            });
            var nextPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>()
            {
                new Pipeline() { Name = "1", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } },
                new Pipeline() { Name = "2", PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } },
            });

            var gocdServiceFactory = new GocdServiceFactoryFake { GocdService = { Pipelines = nextPipelines } };
            var service = new ServiceManager(gocdServiceFactory) { Estate = new Estate(previousPipelines) };
            bool onBuildFailedCalled = false;
            service.OnBuildFailed += () => onBuildFailedCalled = true;

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(onBuildFailedCalled, Is.True);
            Assert.That(service.Estate.Pipelines.Single(p => p.Name == "1").JustFailed, Is.False);
            Assert.That(service.Estate.Pipelines.Single(p => p.Name == "2").JustFailed, Is.True);
        }

        [Test]
        public void PollAndUpdate_DoesNotRaisedAn_OnBuildFailedEvent_IfAFailedBuildIsUnPaused()
        {
            // Arrange
            var previousPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>()
            {
                new Pipeline() { Name = "1", Paused = true, PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } },
            });
            var nextPipelines = Result<List<Pipeline>>.Valid(new List<Pipeline>()
            {
                new Pipeline() { Name = "1", Paused = false, PipelineInstances = { new PipelineInstance() { Stages = new List<Stage>() { new Stage() { Status = StageStatus.Failed } } } } },
            });

            var gocdServiceFactory = new GocdServiceFactoryFake { GocdService = { Pipelines = nextPipelines } };
            var service = new ServiceManager(gocdServiceFactory) { Estate = new Estate(previousPipelines) };
            bool onBuildFailedCalled = false;
            service.OnBuildFailed += () => onBuildFailedCalled = true;

            // Act
            service.PollAndUpdate();

            // Assert
            Assert.That(onBuildFailedCalled, Is.False);
            Assert.That(service.Estate.Pipelines.Single(p => p.Name == "1").JustFailed, Is.False);
        }

        #endregion
    }
}