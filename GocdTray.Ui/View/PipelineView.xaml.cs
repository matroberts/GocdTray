using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GocdTray.Ui.Code;
using GocdTray.Ui.ViewModel;

namespace GocdTray.Ui.View
{
    public partial class PipelineView : Window
    {
        public PipelineView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void SortByBuildStatusClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("SortByBuildStatusClick");
            FrameworkElement fe = sender as FrameworkElement;
            ((PipelineViewModel)fe.DataContext).Sort(PipelineSortOrder.BuildStatus);
        }

        private void SortByAtoZ(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            ((PipelineViewModel)fe.DataContext).Sort(PipelineSortOrder.AtoZ);
        }

        private void SortByZtoA(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            ((PipelineViewModel)fe.DataContext).Sort(PipelineSortOrder.ZtoA);
        }
    }
}
