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
        private ServiceManager serviceManager;

        public STAApplicationContext()
        {
            serviceManager = new ServiceManager();
            viewManager = new ViewManager(serviceManager);
            serviceManager.OnStatusChange += viewManager.OnStatusChange;
            serviceManager.Initialise();
        }

        protected override void Dispose(bool disposing)
        {
            if (serviceManager != null && viewManager != null)
            {
                serviceManager.OnStatusChange -= viewManager.OnStatusChange;
            }

            serviceManager?.Terminate();
            serviceManager = null;
            viewManager = null;
        }
    }
}
