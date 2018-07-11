using System;
using System.Collections.Generic;
using System.Linq;
using GocdTray.App.Abstractions;
using GocdTray.Rest;

namespace GocdTray.App
{
    public class DeviceManager : IDeviceManager
    {
        private GocdServer gocdServer = null;
        public DeviceManager()
        {
            Status = DeviceStatus.Uninitialised;
        }

        private System.Windows.Threading.DispatcherTimer _statusTimer;

        private void KillTimer()
        {
            if (_statusTimer != null)
            {
                _statusTimer.Stop();
                _statusTimer = null;
            }
        }
        
        public delegate void StatusChangeEvent();
        public event StatusChangeEvent OnStatusChange;

        public string DeviceName { get; private set; }
        public DeviceStatus Status { get; private set; }

        public List<Pipeline> Pipelines { get; set; } = new List<Pipeline>();

        public void Initialise()
        {
            if (Status == DeviceStatus.Uninitialised)
            {
                Status = DeviceStatus.Initialised;
            }

            DeviceName = "Death Star";
            gocdServer = new GocdServer(new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors));
        }

        public void PollGocdForChanges()
        {
            // Todo: what to do about showing errors

            var result = gocdServer.GetPipelines();
            if(result.HasData == false)
                throw new ApplicationException(result.ErrorMessage);
            Pipelines = result.Data;
        }

        public void Start()
        {
            if (Status == DeviceStatus.Initialised)
            {
                Status = DeviceStatus.Starting;
                // Simulate a real device with a simple timer
                _statusTimer = new System.Windows.Threading.DispatcherTimer(
                    new TimeSpan(0, 0, 3), 
                    System.Windows.Threading.DispatcherPriority.Normal,
                    delegate 
                    {
                        KillTimer();
                        Status = DeviceStatus.Running;
                        PollGocdForChanges();
                        _statusTimer = null; 
                        if (OnStatusChange != null)
                        {
                            OnStatusChange();
                        }
                    }, 
                    System.Windows.Threading.Dispatcher.CurrentDispatcher);
                
            }
        }

        public void Stop()
        {
            if (Status == DeviceStatus.Running)
            {
                Status = DeviceStatus.Initialised;
                if (OnStatusChange != null)
                {
                    OnStatusChange();
                }
            }
        }
        
        public void Terminate()
        {
            KillTimer();
            Stop();
            Status = DeviceStatus.Uninitialised;
            gocdServer.Dispose();
        }
    }
}
