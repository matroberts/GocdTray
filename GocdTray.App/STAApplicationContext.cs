using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GocdTray.Ui.Code;

namespace GocdTray.App
{
    public class STAApplicationContext : ApplicationContext
    {
        private ViewManager viewManager;
        private DeviceManager deviceManager;

        public STAApplicationContext()
        {
            deviceManager = new DeviceManager();
            viewManager = new ViewManager(deviceManager);
            deviceManager.OnStatusChange += viewManager.OnStatusChange;
            deviceManager.Initialise();
        }

        protected override void Dispose(bool disposing)
        {
            if (deviceManager != null && viewManager != null)
            {
                deviceManager.OnStatusChange -= viewManager.OnStatusChange;
            }

            deviceManager?.Terminate();
            deviceManager = null;
            viewManager = null;
        }
    }
}
