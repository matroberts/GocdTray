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

        public PipelineSortOrder PipelineSortOrder { get; set; } = PipelineSortOrder.BuildStatus;
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

        public void PopulateTable(IEnumerable<Pipeline> pipelines) => Pipelines = Sort(pipelines);

        private ObservableCollection<Pipeline> Sort(IEnumerable<Pipeline> pipelines)
        {
            switch (PipelineSortOrder)
            {
                case PipelineSortOrder.BuildStatus:
                    return new ObservableCollection<Pipeline>(pipelines.OrderBy(p => p.Status).ThenBy(p => p.Name));
                case PipelineSortOrder.AtoZ:
                    return new ObservableCollection<Pipeline>(pipelines.OrderBy(p => p.Name));
                case PipelineSortOrder.ZtoA:
                    return new ObservableCollection<Pipeline>(pipelines.OrderByDescending(p => p.Name));
                default:
                    throw new ArgumentOutOfRangeException(nameof(PipelineSortOrder), PipelineSortOrder, null);
            }
        }

        public void Sort(PipelineSortOrder pipelineSortOrder)
        {
            PipelineSortOrder = pipelineSortOrder;
            PopulateTable(Pipelines);
        }
    }
}
