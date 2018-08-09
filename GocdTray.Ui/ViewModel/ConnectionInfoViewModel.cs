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
            GocdUrl = connectionInfo.GocdApiUri;
        }
        private string gocdUrl;
        public string GocdUrl
        {
            get => gocdUrl;
            set
            {
                gocdUrl = value;
                OnPropertyChanged("GocdUrl");
            }
        }

        public ICommand OkClick => new FuncCommand<object>(o =>
        {
            var connectionInfo = new ConnectionInfo()
            {
                GocdApiUri = GocdUrl,
            };
            serviceManager.SetConnectionInfo(connectionInfo);
            Close();
        });

        public ICommand CancelClick => new FuncCommand<object>(o =>
        {
            Close();
        });

        public event EventHandler CloseRequest;
        protected void Close() => CloseRequest?.Invoke(this, EventArgs.Empty);
    }
}