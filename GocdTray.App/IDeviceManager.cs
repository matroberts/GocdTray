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
        List<Pipeline> Pipelines { get; }
        void Initialise();
        void Start();
        void Stop();
        void Terminate();
    }
}
