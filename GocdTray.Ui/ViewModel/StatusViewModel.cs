using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace GocdTray.Ui.ViewModel
{
    public class StatusViewModel : ViewModelBase
    {
        public StatusViewModel()
        {
            StatusFlags = new ObservableCollection<KeyValuePair<string,string>>();
        }

        private ImageSource icon;
        public ImageSource Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        private bool isRunning = false;
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                isRunning = value;
                OnPropertyChanged("IsRunning");
            }
        }

        private ObservableCollection<KeyValuePair<string,string>> statusFlags;
        public ObservableCollection<KeyValuePair<string, string>> StatusFlags
        {
            get
            {
                return statusFlags;
            }
            set
            {
                statusFlags = value;
                OnPropertyChanged("StatusFlags");
            }
        }
        public void SetStatusFlags(List<KeyValuePair<string, string>> flags)
        {
            StatusFlags = new ObservableCollection<KeyValuePair<string, string>>(flags);
        }
    }
}
