using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using GocdTray.App.Abstractions;
using GocdTray.Ui.Code;
using GocdTray.Ui.Converters;

namespace GocdTray.Ui.ViewModel
{
    public class PipelineViewModel : ViewModelBase
    {
        public PipelineViewModel()
        {
            Pipelines = new ObservableCollection<Pipeline>();
            SortCommand = new FuncCommand<PipelineSortOrder>(Sort);
        }

        public ICommand SortCommand { get; set; }

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

        public void Sort(PipelineSortOrder buildStatus)
        {
            switch (buildStatus)
            {
                case PipelineSortOrder.BuildStatus:
                    Pipelines = new ObservableCollection<Pipeline>(Pipelines.OrderBy(p => p.Status).ThenBy(p => p.Name));
                    break;
                case PipelineSortOrder.AtoZ:
                    Pipelines = new ObservableCollection<Pipeline>(Pipelines.OrderBy(p => p.Name));
                    break;
                case PipelineSortOrder.ZtoA:
                    Pipelines = new ObservableCollection<Pipeline>(Pipelines.OrderByDescending(p => p.Name));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildStatus), buildStatus, null);
            }
        }
    }
}
