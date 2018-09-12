using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly IServiceManager serviceManager;

        public PipelineViewModel(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            Pipelines = new ObservableCollection<Pipeline>();
            SortCommand = new FuncCommand<PipelineSortOrder>(Sort);
            OpenInBrowserCommand = new FuncCommand<Pipeline>(OpenInBrowser);
            this.serviceManager.OnStatusChange += Update;
        }

        public PipelineSortOrder PipelineSortOrder { get; set; } = PipelineSortOrder.BuildStatus;

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

        private void Update()
        {
            var pls = serviceManager.Estate.Pipelines;
            switch (PipelineSortOrder)
            {
                case PipelineSortOrder.BuildStatus:
                    Pipelines = new ObservableCollection<Pipeline>(pls.OrderBy(p => p.StatusAndPausedSortOrder).ThenBy(p => p.Name));
                    break;
                case PipelineSortOrder.AtoZ:
                    Pipelines = new ObservableCollection<Pipeline>(pls.OrderBy(p => p.Name));
                    break;
                case PipelineSortOrder.ZtoA:
                    Pipelines = new ObservableCollection<Pipeline>(pls.OrderByDescending(p => p.Name));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(PipelineSortOrder), PipelineSortOrder, null);
            }
        }

        public ICommand SortCommand { get; }
        private void Sort(PipelineSortOrder pipelineSortOrder)
        {
            PipelineSortOrder = pipelineSortOrder;
            Update();
        }

        public ICommand OpenInBrowserCommand { get; }
        private void OpenInBrowser(Pipeline pipeline)
        {
            Process.Start(GetPipelineUrl(pipeline));
        }

        public string GetPipelineUrl(Pipeline pipeline)
        {
            var baseUri = new Uri(serviceManager.GetConnectionInfo().GocdWebUri, UriKind.Absolute);
            return new Uri(baseUri, pipeline.WebsiteUrl).ToString();
        }
    }
}
