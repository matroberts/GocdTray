using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using GocdTray.App.Abstractions;
using GocdTray.Rest;

namespace GocdTray.App
{
    public class ServiceManager : IServiceManager
    {
        private GocdService gocdService;
        private DispatcherTimer pollingTimer;
        public delegate void StatusChangeEvent();
        public event StatusChangeEvent OnStatusChange;

        public Estate Estate { get; set; } = new Estate(Result<List<Pipeline>>.Invalid("Initialising"));
        public ConnectionInfo GetConnectionInfo()
        {
            return new ConnectionInfo()
            {
                GocdApiUri = Properties.Settings.Default.GocdApiUri,
                IgnoreCertificateErrors = Properties.Settings.Default.IgnoreCertificateErrors,
                Password = Properties.Settings.Default.Password,
                Username = Properties.Settings.Default.Username,
                PollingIntervalSeconds = Properties.Settings.Default.PollingIntervalSeconds,
            };
        }

        public ValidationResult SetConnectionInfo(ConnectionInfo connectionInfo)
        {

            var validationResult = new ValidationResult();

            if(connectionInfo.GocdApiUri.IsTrimmedNullOrEmpty() || Uri.TryCreate(connectionInfo.GocdApiUri, UriKind.Absolute, out _) == false)
                validationResult.Add("You must supply a valid Gocd Url.", nameof(connectionInfo.GocdApiUri));

            if (connectionInfo.Username.IsTrimmedNullOrEmpty())
                validationResult.Add("You must supply a username.", nameof(connectionInfo.Username));

            if (connectionInfo.Password.IsTrimmedNullOrEmpty())
                validationResult.Add("You must supply a password.", nameof(connectionInfo.Password));

            if (connectionInfo.PollingIntervalSeconds < 5)
                validationResult.Add("Polling interval must be at least 5 seconds.", nameof(connectionInfo.PollingIntervalSeconds));

            if (validationResult.IsValid == false)
                return validationResult;

            Properties.Settings.Default.GocdApiUri = connectionInfo.GocdApiUri;
            Properties.Settings.Default.IgnoreCertificateErrors = connectionInfo.IgnoreCertificateErrors;
            Properties.Settings.Default.Password = connectionInfo.Password;
            Properties.Settings.Default.PollingIntervalSeconds = connectionInfo.PollingIntervalSeconds;
            Properties.Settings.Default.Username = connectionInfo.Username;
            Properties.Settings.Default.Save();

            return validationResult;
        }

        public void Initialise()
        {
            gocdService = new GocdService(new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors));
            // timer does not cause re-entry
            pollingTimer = new DispatcherTimer(new TimeSpan(0, 0, 15), 
                                                DispatcherPriority.Normal,
                                                (sender, args) =>
                                                {
                                                    Estate = new Estate(gocdService.GetPipelines());
                                                    OnStatusChange?.Invoke();
                                                },
                                                Dispatcher.CurrentDispatcher);
        }

        public void Terminate()
        {
            pollingTimer?.Stop();
            pollingTimer = null;
            gocdService?.Dispose();
            gocdService = null;
        }
    }
}
