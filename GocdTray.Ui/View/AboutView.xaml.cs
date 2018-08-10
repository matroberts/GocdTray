using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GocdTray.Ui.ViewModel;

namespace GocdTray.Ui.View
{
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
            this.Style = (Style)this.Resources["CustomWindowStyle"];
        }

        public AboutView(AboutViewModel aboutViewModel) : this()
        {
            DataContext = aboutViewModel;
            aboutViewModel.CloseRequest += (sender, e) => this.Close();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
