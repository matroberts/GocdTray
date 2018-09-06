using System;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace GocdTray.App
{
    // Initial version of the system tray code from: https://www.codeproject.com/Articles/1173686/A-Csharp-System-Tray-Application-using-WPF-Forms
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            string mutexName = Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            using (new Mutex(false, mutexName, out var createdNew))
            {
                if (!createdNew)
                {
                    return;
                }

                AddGocdTrayToStartup();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    Application.Run(new GocdTrayApplicationContext());
                }
                catch (Exception ex)
                {
                    var date = DateTime.UtcNow;
                    var folder = ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.DataDirectory : Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
                    var filename = $"CrashReport-{date:yyyyMMddHHmmss}.txt";
                    var path = Path.Combine(folder, filename);

                    var body = $@"
GocdTray Crash Report
=====================
Version : {Assembly.GetExecutingAssembly().GetName().Version}
Date    : {date:s}
 
{ex}
";
                    File.WriteAllText(path, body);
                    var message = $"Error '{ex.Message}'{Environment.NewLine}{Environment.NewLine}Crash report written to '{path}'";
                    MessageBox.Show(message, "GocdTray Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void AddGocdTrayToStartup()
        {
            if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                string startupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Gocd Tray" + ".appref-ms");

                if (!File.Exists(startupPath))
                {
                    string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "Gocd Tray", "Gocd Tray" + ".appref-ms");
                    File.Copy(shortcutPath, startupPath);
                }
            }
        }
    }
}
