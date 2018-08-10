using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GocdTray.Ui.Code;

namespace GocdTray.App
{
    public class GocdTrayApplicationContext : ApplicationContext
    {
        private ViewManager viewManager;
        private ServiceManager serviceManager;

        public GocdTrayApplicationContext()
        {
            serviceManager = new ServiceManager();
            viewManager = new ViewManager(serviceManager);
            serviceManager.OnStatusChange += viewManager.OnStatusChange;
            serviceManager.Restart();
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
