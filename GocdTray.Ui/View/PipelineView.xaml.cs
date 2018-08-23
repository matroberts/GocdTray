using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GocdTray.Ui.Code;
using GocdTray.Ui.ViewModel;

namespace GocdTray.Ui.View
{
    public partial class PipelineView : Window
    {
        public PipelineView()
        {
            InitializeComponent();
            this.Style = (Style)this.Resources["CustomWindowStyle"];

            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Top = SystemParameters.WorkArea.Height - this.Height;
            this.Left = SystemParameters.WorkArea.Width - this.Width;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.ContextMenu.IsEnabled = true;
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.Placement = PlacementMode.Bottom;
            button.ContextMenu.IsOpen = true;
        }
    }
}
