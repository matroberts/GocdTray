﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GocdTray.App.Abstractions;
using GocdTray.Ui.Properties;
using GocdTray.Ui.View;
using GocdTray.Ui.ViewModel;

namespace GocdTray.Ui.Code
{

    public class ViewManager
    {
        private NotifyIcon notifyIcon;
        private IServiceManager serviceManager;
        private AboutView aboutView;
        private AboutViewModel aboutViewModel;
        private PipelineView pipelineView;
        private PipelineViewModel pipelineViewModel;
        private ConnectionInfoView connectionInfoView;

        public ViewManager(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;

            notifyIcon = new NotifyIcon(new Container())
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = AppIcon,
                Text = AppText,
                Visible = true,
            };

            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.DoubleClick += (sender, e) => ShowPipelineView();
            notifyIcon.MouseUp += NotifyIcon_MouseUp;

            aboutViewModel = new AboutViewModel() {Icon = AppImageSource};
            pipelineViewModel = new PipelineViewModel {Icon = AppImageSource};
        }

        private Icon AppIcon
        {
            get
            {
                switch (serviceManager.Estate.Status)
                {
                    case EstateStatus.NotConnected:
                        return Resources.NotConnected;
                    case EstateStatus.Building:
                        return Resources.Building;
                    case EstateStatus.Passed:
                        return Resources.Passed;
                    case EstateStatus.Failed:
                        return Resources.Failed;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private string AppText => serviceManager.Estate.Status.ToString();

        private ImageSource AppImageSource => Imaging.CreateBitmapSourceFromHIcon(AppIcon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        public void OnStatusChange() => UpdateViews();

        private void UpdateViews()
        {
            notifyIcon.Icon = AppIcon;
            notifyIcon.Text = AppText;

            if (aboutView != null)
            {
                aboutView.Icon = AppImageSource;
            }

            if (pipelineView != null)
            {
                pipelineView.Icon = AppImageSource;
            }
            aboutViewModel.AddVersionInfo("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            pipelineViewModel.PopulateTable(serviceManager.Estate.Pipelines);
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
            }
            else
            {
                pipelineView.Activate();
            }

            UpdateViews();
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

            UpdateViews();
        }

        private void ShowConnectionInfoView()
        {
            if (connectionInfoView == null)
            {
                connectionInfoView = new ConnectionInfoView(new ConnectionInfoViewModel(serviceManager));
                connectionInfoView.Closing += (sender, e) => connectionInfoView = null;
                connectionInfoView.Show();
            }
            else
            {
                connectionInfoView.Activate();
            }
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
                notifyIcon.ContextMenuStrip.Items.Add(CreateMenuItemWithHandler("Connection Info", "Configure Connection Information", (sender1, e1) => ShowConnectionInfoView()));
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
