using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GocdTray.Rest;
using GocdTray.Ui.Code;

namespace GocdTray.App
{
    public class GocdTrayApplicationContext : ApplicationContext
    {
        private ViewManager viewManager;
        private ServiceManager serviceManager;

        public GocdTrayApplicationContext()
        {
            serviceManager = new ServiceManager(new GocdServiceFactory());
            viewManager = new ViewManager(serviceManager);
            serviceManager.Restart();
        }

        protected override void Dispose(bool disposing)
        {
            serviceManager?.Dispose();
            serviceManager = null;
            viewManager = null;
        }
    }
}
