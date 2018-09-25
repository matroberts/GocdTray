using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using GocdTray.App.Abstractions;
using GocdTray.Rest;

namespace GocdTray.App
{
    public class ServiceManager : IServiceManager, IDisposable
    {
        private readonly IGocdServiceFactory gocdServiceFactory;
        private IGocdService gocdService;
        private DispatcherTimer pollingTimer;
        public event Action OnStatusChange;
        public event Action OnBuildFailed;

        public ServiceManager(IGocdServiceFactory gocdServiceFactory)
        {
            this.gocdServiceFactory = gocdServiceFactory;
            this.gocdService = gocdServiceFactory.Create(GetConnectionInfo());
        }

        public Estate Estate { get; set; } = new Estate(Result<List<Pipeline>>.Invalid("Initialising"));
        public Estate PreviousEstate { get; set; } = new Estate(Result<List<Pipeline>>.Invalid("Initialising"));
        public ConnectionInfo GetConnectionInfo()
        {
            return new ConnectionInfo()
            {
                GocdApiUri = Properties.Settings.Default.GocdApiUri,
                GocdWebUri = Properties.Settings.Default.GocdWebUri,
                IgnoreCertificateErrors = Properties.Settings.Default.IgnoreCertificateErrors,
                Password = Properties.Settings.Default.Password,
                Username = Properties.Settings.Default.Username,
                PollingIntervalSeconds = Properties.Settings.Default.PollingIntervalSeconds,
            };
        }

        public ValidationResult SetConnectionInfo(ConnectionInfo connectionInfo)
        {
            var validationResult = ValidateConnectionInfo(connectionInfo);
            if (validationResult.IsValid == false)
                return validationResult;

            Properties.Settings.Default.GocdApiUri = connectionInfo.GocdApiUri;
            Properties.Settings.Default.GocdWebUri = connectionInfo.GocdWebUri;
            Properties.Settings.Default.IgnoreCertificateErrors = connectionInfo.IgnoreCertificateErrors;
            Properties.Settings.Default.Password = connectionInfo.Password;
            Properties.Settings.Default.PollingIntervalSeconds = connectionInfo.PollingIntervalSeconds;
            Properties.Settings.Default.Username = connectionInfo.Username;
            Properties.Settings.Default.Save();
            Restart();

            return validationResult;
        }

        public ValidationResult TestConnectionInfo(ConnectionInfo connectionInfo)
        {
            var validationResult = ValidateConnectionInfo(connectionInfo);
            if (validationResult.IsValid == false)
                return validationResult;

            Result<List<Pipeline>> restResult = null;
            using (var tempGocdServive = gocdServiceFactory.Create(connectionInfo))
            {
                restResult = tempGocdServive.GetPipelines();
            }

            if(restResult.IsValid == false)
                validationResult.Add(restResult.ErrorMessage);
             
            return validationResult;
        }

        private ValidationResult ValidateConnectionInfo(ConnectionInfo connectionInfo)
        {
            var validationResult = new ValidationResult();

            if (connectionInfo.GocdApiUri.IsTrimmedNullOrEmpty() || Uri.TryCreate(connectionInfo.GocdApiUri, UriKind.Absolute, out _) == false)
                validationResult.Add("You must supply a valid url for the Gocd Api.", nameof(connectionInfo.GocdApiUri));

            if (connectionInfo.GocdWebUri.IsTrimmedNullOrEmpty() || Uri.TryCreate(connectionInfo.GocdWebUri, UriKind.Absolute, out _) == false)
                validationResult.Add("You must supply a valid url for the Gocd Website.", nameof(connectionInfo.GocdWebUri));

            if (connectionInfo.Username.IsTrimmedNullOrEmpty())
                validationResult.Add("You must supply a username.", nameof(connectionInfo.Username));

            if (connectionInfo.Password.IsTrimmedNullOrEmpty())
                validationResult.Add("You must supply a password.", nameof(connectionInfo.Password));

            if (connectionInfo.PollingIntervalSeconds < 5)
                validationResult.Add("Polling interval must be at least 5 seconds.", nameof(connectionInfo.PollingIntervalSeconds));

            return validationResult;
        }

        public void Restart()
        {
            pollingTimer?.Stop();
            gocdService?.Dispose();
            var connectionInfo = GetConnectionInfo();
            gocdService = gocdServiceFactory.Create(connectionInfo);
            PollAndUpdate();
            // timer does not cause re-entry
            pollingTimer = new DispatcherTimer(TimeSpan.FromSeconds(connectionInfo.PollingIntervalSeconds), DispatcherPriority.Normal, (sender, args) => PollAndUpdate(), Dispatcher.CurrentDispatcher);
        }

        public void PollAndUpdate()
        {
            PreviousEstate = Estate;
            Estate = new Estate(gocdService.GetPipelines());
            OnStatusChange?.Invoke();
            foreach (var pipeline in Estate.Pipelines)
            {
                var previousPipeline = PreviousEstate.Pipelines.FirstOrDefault(p => p.Name == pipeline.Name);
                if(previousPipeline==null)
                    continue;

                if ((previousPipeline.Status == PipelineStatus.Building || previousPipeline.Status == PipelineStatus.Passed) && pipeline.Status == PipelineStatus.Failed)
                {
                    OnBuildFailed?.Invoke();
                    break;
                }
            }
        }

        public void Dispose()
        {
            pollingTimer?.Stop();
            pollingTimer = null;
            gocdService?.Dispose();
            gocdService = null;
        }
    }
}
