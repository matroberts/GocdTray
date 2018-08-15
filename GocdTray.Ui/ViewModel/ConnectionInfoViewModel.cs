using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GocdTray.App.Abstractions;
using GocdTray.Ui.Code;

namespace GocdTray.Ui.ViewModel
{
    public class ConnectionInfoViewModel : ViewModelBase
    {
        private readonly IServiceManager serviceManager;

        public ConnectionInfoViewModel(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            var connectionInfo = serviceManager.GetConnectionInfo();
            GocdApiUri = connectionInfo.GocdApiUri;
            GocdWebUri = connectionInfo.GocdWebUri;
            Username = connectionInfo.Username;
            Password = connectionInfo.Password;
            IgnoreCertificateErrors = connectionInfo.IgnoreCertificateErrors;
            PollingIntervalSeconds = connectionInfo.PollingIntervalSeconds;
        }

        private string gocdApiUri;
        public string GocdApiUri
        {
            get => gocdApiUri;
            set
            {
                gocdApiUri = value;
                OnPropertyChanged(nameof(GocdApiUri));
            }
        }

        private string gocdWebUri;
        public string GocdWebUri
        {
            get => gocdWebUri;
            set
            {
                gocdWebUri = value;
                OnPropertyChanged(nameof(GocdWebUri));
            }
        }

        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool ignoreCertificateErrors;
        public bool IgnoreCertificateErrors
        {
            get => ignoreCertificateErrors;
            set
            {
                ignoreCertificateErrors = value;
                OnPropertyChanged(nameof(IgnoreCertificateErrors));
            }
        }

        private int pollingIntervalSeconds;
        public int PollingIntervalSeconds
        {
            get => pollingIntervalSeconds;
            set
            {
                pollingIntervalSeconds = value;
                OnPropertyChanged(nameof(PollingIntervalSeconds));
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand OkClick => new FuncCommand<object>(o =>
        {
            var connectionInfo = new ConnectionInfo()
            {
                GocdApiUri = GocdApiUri,
                GocdWebUri = GocdWebUri,
                PollingIntervalSeconds = PollingIntervalSeconds,
                Username = Username,
                Password = Password,
                IgnoreCertificateErrors = IgnoreCertificateErrors,
            };

            var result = serviceManager.SetConnectionInfo(connectionInfo);
            if (result.IsValid)
            {
                Close();
            }
            else
            {
                ErrorMessage = result.ToString();
            }
        });

        public ICommand CancelClick => new FuncCommand<object>(o =>
        {
            Close();
        });

        public event EventHandler CloseRequest;
        protected void Close() => CloseRequest?.Invoke(this, EventArgs.Empty);
    }
}