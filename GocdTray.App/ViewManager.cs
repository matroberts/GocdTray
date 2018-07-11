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
        // This allows code to be run on a GUI thread
        private Window _hiddenWindow;

        private IContainer _components;
        // The Windows system tray class
        private NotifyIcon _notifyIcon;
        IDeviceManager _deviceManager;

        private GocdTray.Ui.View.AboutView _aboutView;
        private GocdTray.Ui.ViewModel.AboutViewModel _aboutViewModel;
        private GocdTray.Ui.View.PipelineView pipelineView;
        private GocdTray.Ui.ViewModel.PipelineViewModel pipelineViewModel;

        private ToolStripMenuItem _exitMenuItem;

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

            _aboutViewModel = new GocdTray.Ui.ViewModel.AboutViewModel() { Icon = AppIcon };
            pipelineViewModel = new GocdTray.Ui.ViewModel.PipelineViewModel {Icon = AppIcon};

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
                pipelineView = new GocdTray.Ui.View.PipelineView
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

        private void showStatusItem_Click(object sender, EventArgs e)
        {
            ShowPipelineView();
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
            _aboutViewModel.AddVersionInfo("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void showHelpItem_Click(object sender, EventArgs e)
        {
            ShowAboutView();
        }
        
        private void exitItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
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
                _notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("Device S&tatus", "Shows the device status dialog", showStatusItem_Click));
                _notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("&About", "Shows the About dialog", showHelpItem_Click));
                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _exitMenuItem = CreateMenuItemWithHandler("&Exit", "Exits System Tray App", exitItem_Click);
                _notifyIcon.ContextMenuStrip.Items.Add(_exitMenuItem);
            }
        }

        private ToolStripMenuItem CreateMenuItemWithHandler(string displayText, string tooltipText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            if (eventHandler != null)
            {
                item.Click += eventHandler;
            }

            item.ToolTipText = tooltipText;
            return item;
        }
    }
}
