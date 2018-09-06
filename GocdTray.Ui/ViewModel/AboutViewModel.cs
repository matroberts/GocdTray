using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GocdTray.Ui.Code;

namespace GocdTray.Ui.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public ICommand CloseClick => new FuncCommand<object>(o => Close());
        public ICommand ThrowException => new FuncCommand<object>(o => throw new Exception("Test Exception thrown by UI"));

        public event EventHandler CloseRequest;
        protected void Close() => CloseRequest?.Invoke(this, EventArgs.Empty);
    }
}
