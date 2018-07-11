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

namespace GocdTray.App
{
    public class ViewManager
    {
        public ViewManager(IDeviceManager deviceManager)
        {
            System.Diagnostics.Debug.Assert(deviceManager != null);

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

            _aboutViewModel = new GocdTray.Ui.ViewModel.AboutViewModel();
            _statusViewModel = new GocdTray.Ui.ViewModel.StatusViewModel();

            _statusViewModel.Icon = AppIcon;
            _aboutViewModel.Icon = _statusViewModel.Icon;

            _hiddenWindow = new Window();
            _hiddenWindow.Hide();
        }

        ImageSource AppIcon
        {
            get
            {
                Icon icon = (_deviceManager.Status == DeviceStatus.Running) ? Properties.Resources.ReadyIcon : Properties.Resources.NotReadyIcon;
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        // This allows code to be run on a GUI thread
        private Window _hiddenWindow;

        private IContainer _components;
        // The Windows system tray class
        private NotifyIcon _notifyIcon;  
        IDeviceManager _deviceManager;

        private GocdTray.Ui.View.AboutView _aboutView;
        private GocdTray.Ui.ViewModel.AboutViewModel _aboutViewModel;
        private GocdTray.Ui.View.StatusView _statusView;
        private GocdTray.Ui.ViewModel.StatusViewModel _statusViewModel;

        private ToolStripMenuItem _startDeviceMenuItem;
        private ToolStripMenuItem _stopDeviceMenuItem;
        private ToolStripMenuItem _exitMenuItem;

        private void DisplayStatusMessage(string text)
        {
            _hiddenWindow.Dispatcher.Invoke(delegate
            {
                _notifyIcon.BalloonTipText = _deviceManager.DeviceName + ": " + text;
                // The timeout is ignored on recent Windows
                _notifyIcon.ShowBalloonTip(3000);
            });
        }

        private void UpdateStatusView()
        {
            if ((_statusViewModel != null) && (_deviceManager != null))
            {
                _statusViewModel.PopulateTable(_deviceManager.Pipelines);
            }
        }

        public void OnStatusChange()
        {
            UpdateStatusView();

            switch (_deviceManager.Status)
            {
                case DeviceStatus.Initialised:
                    _notifyIcon.Text = _deviceManager.DeviceName + ": Ready";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    DisplayStatusMessage("Idle");
                    break;
                case DeviceStatus.Running:
                    _notifyIcon.Text = _deviceManager.DeviceName + ": Running";
                    _notifyIcon.Icon = Properties.Resources.ReadyIcon;
                    DisplayStatusMessage("Running");
                    break;
                case DeviceStatus.Starting:
                    _notifyIcon.Text = _deviceManager.DeviceName + ": Starting";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    DisplayStatusMessage("Starting");
                    break;
                case DeviceStatus.Uninitialised:
                    _notifyIcon.Text = _deviceManager.DeviceName + ": Not Ready";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    break;
                case DeviceStatus.Error:
                    _notifyIcon.Text = _deviceManager.DeviceName + ": Error Detected";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    break;
                default:
                    _notifyIcon.Text = _deviceManager.DeviceName + ": -";
                    _notifyIcon.Icon = Properties.Resources.NotReadyIcon;
                    break;
            }
            if (_aboutView != null)
            {
                _aboutView.Icon = AppIcon;
            }
            if (_statusView != null)
            {
                _statusView.Icon = AppIcon;
            }
        }

        private void startStopReaderItem_Click(object sender, EventArgs e)
        {
            if (_deviceManager.Status == DeviceStatus.Running)
            {
                _deviceManager.Stop();
            }
            else
            {
                _deviceManager.Start();
            }
        }
        
        private ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string tooltipText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            if (eventHandler != null)
            {
                item.Click += eventHandler;
            }

            item.ToolTipText = tooltipText;
            return item;
        }
        
        private void ShowStatusView()
        {
            if (_statusView == null)
            {
                _statusView = new GocdTray.Ui.View.StatusView();
                _statusView.DataContext = _statusViewModel;

                _statusView.Closing += ((arg_1, arg_2) => _statusView = null);
                _statusView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _statusView.Show();
                UpdateStatusView();
            }
            else
            {
                _statusView.Activate();
            }
            _statusView.Icon = AppIcon;
        }

        private void showStatusItem_Click(object sender, EventArgs e)
        {
            ShowStatusView();
        }

        private void ShowAboutView()
        {
            if (_aboutView == null)
            {
                _aboutView = new GocdTray.Ui.View.AboutView();
                _aboutView.DataContext = _aboutViewModel;
                _aboutView.Closing += ((arg_1, arg_2) => _aboutView = null);
                _aboutView.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                _aboutView.Show();
            }
            else
            {
                _aboutView.Activate();
            }
            _aboutView.Icon = AppIcon;

            _aboutViewModel.AddVersionInfo("Hardware", _deviceManager.DeviceName);
            _aboutViewModel.AddVersionInfo("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            _aboutViewModel.AddVersionInfo("Serial Number", "142573462354");
        }

        private void showHelpItem_Click(object sender, EventArgs e)
        {
            ShowAboutView();
        }

        private void showWebSite_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.CodeProject.com/");
        }
        
        private void exitItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowAboutView();
        }

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(_notifyIcon, null);
            }
        }
        
        private void SetMenuItems()
        {
            switch (_deviceManager.Status)
            {
                case DeviceStatus.Initialised:
                    _startDeviceMenuItem.Enabled = true;
                    _stopDeviceMenuItem.Enabled = false;
                    _exitMenuItem.Enabled = true;
                    break;
                case DeviceStatus.Starting:
                    _startDeviceMenuItem.Enabled = false;
                    _stopDeviceMenuItem.Enabled = false;
                    _exitMenuItem.Enabled = false;
                    break;
                case DeviceStatus.Running:
                    _startDeviceMenuItem.Enabled = false;
                    _stopDeviceMenuItem.Enabled = true;
                    _exitMenuItem.Enabled = true;
                    break;
                case DeviceStatus.Uninitialised:
                    _startDeviceMenuItem.Enabled = false;
                    _stopDeviceMenuItem.Enabled = false;
                    _exitMenuItem.Enabled = true;
                    break;
                case DeviceStatus.Error:
                    _startDeviceMenuItem.Enabled = false;
                    _stopDeviceMenuItem.Enabled = false;
                    _exitMenuItem.Enabled = true;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "SetButtonStatus() => Unknown state");
                    break;
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            if (_notifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                _startDeviceMenuItem = ToolStripMenuItemWithHandler(
                    "Start Device",
                    "Starts the device",
                    startStopReaderItem_Click);
                _notifyIcon.ContextMenuStrip.Items.Add(_startDeviceMenuItem);
                _stopDeviceMenuItem = ToolStripMenuItemWithHandler(
                    "Stop Device",
                    "Stops the device",
                    startStopReaderItem_Click);
                _notifyIcon.ContextMenuStrip.Items.Add(_stopDeviceMenuItem);
                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("Device S&tatus", "Shows the device status dialog", showStatusItem_Click));
                _notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&About", "Shows the About dialog", showHelpItem_Click));
                _notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("Code Project &Web Site", "Navigates to the Code Project Web Site", showWebSite_Click));
                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _exitMenuItem = ToolStripMenuItemWithHandler("&Exit", "Exits System Tray App", exitItem_Click);
                _notifyIcon.ContextMenuStrip.Items.Add(_exitMenuItem);
            }

            SetMenuItems();
        }
    }
}
