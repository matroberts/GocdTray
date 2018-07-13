using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GocdTray.Ui.Properties;
using GocdTray.Ui.View;
using GocdTray.Ui.ViewModel;

namespace GocdTray.App
{

    public class ViewManager
    {
        private Window hiddenWindow;
        private NotifyIcon notifyIcon;
        private IDeviceManager deviceManager;
        private AboutView aboutView;
        private AboutViewModel aboutViewModel;
        private PipelineView pipelineView;
        private PipelineViewModel pipelineViewModel;

        public ViewManager(IDeviceManager deviceManager)
        {
            this.deviceManager = deviceManager;

            notifyIcon = new NotifyIcon(new Container())
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Resources.Failed,
                Text = "Go.cd Tray Application",
                Visible = true,
            };

            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.DoubleClick += (sender, e) => ShowPipelineView();
            notifyIcon.MouseUp += NotifyIcon_MouseUp;

            aboutViewModel = new AboutViewModel() { Icon = AppIcon };
            pipelineViewModel = new PipelineViewModel {Icon = AppIcon};

            hiddenWindow = new Window();
            hiddenWindow.Hide();
        }

        ImageSource AppIcon
        {
            get
            {
                Icon icon = deviceManager.Status == DeviceStatus.Running ? Resources.Passed : Resources.Failed;
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private void DisplayStatusMessage(string text)
        {
            hiddenWindow.Dispatcher.Invoke(delegate
            {
                notifyIcon.BalloonTipText = text;
                // The timeout is ignored on recent Windows
                notifyIcon.ShowBalloonTip(3000);
            });
        }

        private void UpdatePipelineView()
        {
            if (pipelineViewModel != null && deviceManager != null)
            {
                pipelineViewModel.PopulateTable(deviceManager.Estate.Pipelines);
            }
        }

        public void OnStatusChange()
        {
            UpdatePipelineView();

            switch (deviceManager.Status)
            {
                case DeviceStatus.Initialised:
                    notifyIcon.Text = "Ready";
                    notifyIcon.Icon = Resources.Failed;
                    DisplayStatusMessage("Idle");
                    break;
                case DeviceStatus.Running:
                    notifyIcon.Text = "Running";
                    notifyIcon.Icon = Resources.Failed;
                    DisplayStatusMessage("Running");
                    break;
                case DeviceStatus.Starting:
                    notifyIcon.Text = "Starting";
                    notifyIcon.Icon = Resources.Failed;
                    DisplayStatusMessage("Starting");
                    break;
                case DeviceStatus.Uninitialised:
                    notifyIcon.Text = "Not Ready";
                    notifyIcon.Icon = Resources.Failed;
                    break;
                case DeviceStatus.Error:
                    notifyIcon.Text = "Error Detected";
                    notifyIcon.Icon = Resources.Failed;
                    break;
                default:
                    notifyIcon.Text = "-";
                    notifyIcon.Icon = Resources.Failed;
                    break;
            }
            if (aboutView != null)
            {
                aboutView.Icon = AppIcon;
            }
            if (pipelineView != null)
            {
                pipelineView.Icon = AppIcon;
            }
        }
        
        private void ShowPipelineView()
        {
            if (pipelineView == null)
            {
                pipelineView = new PipelineView
                {
                    DataContext = pipelineViewModel,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                pipelineView.Closing += (sender, e) => pipelineView = null;
                pipelineView.Show();
                UpdatePipelineView();
            }
            else
            {
                pipelineView.Activate();
            }
            pipelineView.Icon = AppIcon;
        }

        private void ShowAboutView()
        {
            if (aboutView == null)
            {
                aboutView = new AboutView
                {
                    DataContext = aboutViewModel,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                aboutView.Closing += (sender, e) => aboutView = null;
                aboutView.Show();
            }
            else
            {
                aboutView.Activate();
            }
            aboutView.Icon = AppIcon;
            aboutViewModel.AddVersionInfo("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void NotifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // https://stackoverflow.com/a/2208910
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }
        
        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (notifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("Pipelines", "Show the status of the Go.cd pipelines", (sender1, e1) => ShowPipelineView()));
                notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("About", "Show the About dialog", (sender1, e1) => ShowAboutView()));
                notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("Exit", "Exits the application", (sender1, e1) => System.Windows.Forms.Application.Exit()));
            }
        }

        private ToolStripMenuItem CreateMenuItemWithHandler(string displayText, string tooltipText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText) {ToolTipText = tooltipText};
            item.Click += eventHandler;
            return item;
        }
    }
}
