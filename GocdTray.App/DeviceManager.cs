using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using GocdTray.App.Abstractions;
using GocdTray.Rest;

namespace GocdTray.App
{
    public class DeviceManager : IDeviceManager
    {
        private GocdServer gocdServer;
        private DispatcherTimer pollingTimer;
        public delegate void StatusChangeEvent();
        public event StatusChangeEvent OnStatusChange;

        public DeviceManager()
        {
            Status = DeviceStatus.Uninitialised;
        }

        public DeviceStatus Status { get; private set; }

        public Estate Estate { get; set; } = new Estate(Result<List<Pipeline>>.Invalid("Initialising"));


        public void Initialise()
        {
            if (Status == DeviceStatus.Uninitialised)
            {
                Status = DeviceStatus.Initialised;
            }
            gocdServer = new GocdServer(new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors));
            // timer does not cause re-entry
            pollingTimer = new DispatcherTimer(new TimeSpan(0, 0, 15), 
                                                DispatcherPriority.Normal,
                                                (sender, args) =>
                                                {
                                                    Estate = new Estate(gocdServer.GetPipelines());
                                                    OnStatusChange?.Invoke();
                                                },
                                                Dispatcher.CurrentDispatcher);
        }

        public void Terminate()
        {
            pollingTimer?.Stop();
            pollingTimer = null;
            Status = DeviceStatus.Uninitialised;
            gocdServer?.Dispose();
            gocdServer = null;
        }
    }
}
