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
            Pipelines = new ObservableCollection<UiPipeline>();
            SortCommand = new FuncCommand<PipelineSortOrder>(Sort);
            OpenInBrowserCommand = new FuncCommand<UiPipeline>(OpenInBrowser);
            ToggleDetailsVisibilityCommand = new FuncCommand<UiPipeline>(ToggleDetailsVisibility);
            this.serviceManager.OnStatusChange += Update;
        }

        public PipelineSortOrder PipelineSortOrder { get; set; } = PipelineSortOrder.BuildStatus;

        private ObservableCollection<UiPipeline> pipelines;
        public ObservableCollection<UiPipeline> Pipelines
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
            var pls = serviceManager.Estate.Pipelines.Select(p => new UiPipeline(p));
            switch (PipelineSortOrder)
            {
                case PipelineSortOrder.BuildStatus:
                    Pipelines = new ObservableCollection<UiPipeline>(pls.OrderBy(p => p.StatusAndPausedSortOrder).ThenBy(p => p.Name));
                    break;
                case PipelineSortOrder.AtoZ:
                    Pipelines = new ObservableCollection<UiPipeline>(pls.OrderBy(p => p.Name));
                    break;
                case PipelineSortOrder.ZtoA:
                    Pipelines = new ObservableCollection<UiPipeline>(pls.OrderByDescending(p => p.Name));
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
        private void OpenInBrowser(UiPipeline pipeline)
        {
            Process.Start(GetPipelineUrl(pipeline));
        }

        public ICommand ToggleDetailsVisibilityCommand { get; }
        private void ToggleDetailsVisibility(UiPipeline pipeline)
        {
            pipeline.IsExpanded = !pipeline.IsExpanded;
        }

        public string GetPipelineUrl(UiPipeline pipeline)
        {
            var baseUri = new Uri(serviceManager.GetConnectionInfo().GocdWebUri, UriKind.Absolute);
            return new Uri(baseUri, pipeline.WebsiteUrl).ToString();
        }
    }
}
