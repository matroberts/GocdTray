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
using GocdTray.Ui.View;
using GocdTray.Ui.ViewModel;

namespace GocdTray.App
{

    public class ViewManager
    {
        // This allows code to be run on a GUI thread
        private Window _hiddenWindow;

        private IContainer _components;
        // The Windows system tray class
        private NotifyIcon _notifyIcon;
        IDeviceManager _deviceManager;

        private AboutView _aboutView;
        private AboutViewModel _aboutViewModel;
        private PipelineView pipelineView;
        private PipelineViewModel pipelineViewModel;

        public ViewManager(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;

            _components = new Container();
            _notifyIcon = new NotifyIcon(_components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.NotReadyIcon,
                Text = "System Tray App: Device Not Present",
                Visible = true,
            };

            _notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            _notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            _notifyIcon.MouseUp += notifyIcon_MouseUp;

            _aboutViewModel = new AboutViewModel() { Icon = AppIcon };
            pipelineViewModel = new PipelineViewModel {Icon = AppIcon};

            _hiddenWindow = new Window();
            _hiddenWindow.Hide();
        }

        ImageSource AppIcon
        {
            get
            {
                Icon icon = _deviceManager.Status == DeviceStatus.Running ? Properties.Resources.ReadyIcon : Properties.Resources.NotReadyIcon;
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private void DisplayStatusMessage(string text)
        {
            _hiddenWindow.Dispatcher.Invoke(delegate
            {
                _notifyIcon.BalloonTipText = text;
                // The timeout is ignored on recent Windows
                _notifyIcon.ShowBalloonTip(3000);
            });
        }

        private void UpdatePipelineView()
        {
            if ((pipelineViewModel != null) && (_deviceManager != null))
            {
                pipelineViewModel.PopulateTable(_deviceManager.Pipelines);
            }
        }

        public void OnStatusChange()
        {
            UpdatePipelineView();

            switch (_deviceManager.Status)
            {
                case DeviceStatus.Initialised:
                    _notifyIcon.Text = "Ready";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    DisplayStatusMessage("Idle");
                    break;
                case DeviceStatus.Running:
                    _notifyIcon.Text = "Running";
                    _notifyIcon.Icon = Properties.Resources.ReadyIcon;
                    DisplayStatusMessage("Running");
                    break;
                case DeviceStatus.Starting:
                    _notifyIcon.Text = "Starting";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    DisplayStatusMessage("Starting");
                    break;
                case DeviceStatus.Uninitialised:
                    _notifyIcon.Text = "Not Ready";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    break;
                case DeviceStatus.Error:
                    _notifyIcon.Text = "Error Detected";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    break;
                default:
                    _notifyIcon.Text = "-";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    break;
            }
            if (_aboutView != null)
            {
                _aboutView.Icon = AppIcon;
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
                pipelineView.Closing += ((arg_1, arg_2) => pipelineView = null);
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
            if (_aboutView == null)
            {
                _aboutView = new AboutView
                {
                    DataContext = _aboutViewModel,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                _aboutView.Closing += (sender, e) => _aboutView = null;
                _aboutView.Show();
            }
            else
            {
                _aboutView.Activate();
            }
            _aboutView.Icon = AppIcon;
            _aboutViewModel.AddVersionInfo("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowPipelineView();
        }

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // https://stackoverflow.com/a/2208910
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(_notifyIcon, null);
            }
        }
        


        /*
         *  Context Menu
         */

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (_notifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                _notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("Pipelines", "Show the status of the Go.cd pipelines", (sender1, e1) => ShowPipelineView()));
                _notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("About", "Show the About dialog", (sender1, e1) => ShowAboutView()));
                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("Exit", "Exits the application", (sender1, e1) => System.Windows.Forms.Application.Exit()));
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
