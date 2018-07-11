using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using GocdTray.App.Abstractions;

namespace GocdTray.Ui.ViewModel
{
    public class PipelineViewModel : ViewModelBase
    {
        public PipelineViewModel()
        {
            Pipelines = new ObservableCollection<Pipeline>();
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

        private ObservableCollection<Pipeline> pipelines;
        public ObservableCollection<Pipeline> Pipelines
        {
            get => pipelines;
            set
            {
                pipelines = value;
                OnPropertyChanged("Pipelines");
            }
        }

        public void PopulateTable(List<Pipeline> pipelines) => Pipelines = new ObservableCollection<Pipeline>(pipelines);
    }
}
