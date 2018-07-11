using System;
using System.Collections.Generic;
using System.Linq;
using GocdTray.App.Abstractions;

namespace GocdTray.App
{
    public interface IDeviceManager
    {
        string DeviceName { get; }
        DeviceStatus Status { get; }
        List<KeyValuePair<string, bool>> StatusFlags { get; }
        List<Pipeline> Pipelines { get; set; }
        void Initialise();
        void Start();
        void Stop();
        void Terminate();
    }
}
