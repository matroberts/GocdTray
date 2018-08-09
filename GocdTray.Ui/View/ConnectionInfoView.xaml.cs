using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GocdTray.Ui.ViewModel;

namespace GocdTray.Ui.View
{
    /// <summary>
    /// Interaction logic for ConnectionInfo.xaml
    /// </summary>
    public partial class ConnectionInfoView : Window
    {
        public ConnectionInfoView()
        {
            InitializeComponent();
        }

        public ConnectionInfoView(ConnectionInfoViewModel connectionInfoViewModel) : this()
        {
            DataContext = connectionInfoViewModel;
            connectionInfoViewModel.CloseRequest += (sender, e) => this.Close();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ElementHost.EnableModelessKeyboardInterop(this);
        }

        private static readonly Regex intOnlyRegex =  new Regex("[^0-9]+");
        private void ValidateInt(object sender, TextCompositionEventArgs e) => e.Handled = intOnlyRegex.IsMatch(e.Text);
    }
}
