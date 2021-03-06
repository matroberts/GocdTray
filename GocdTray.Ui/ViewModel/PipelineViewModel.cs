﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GocdTray.App.Abstractions;
using GocdTray.Ui.Code;

namespace GocdTray.Ui.ViewModel
{
    public class PipelineViewModel : ViewModelBase
    {
        private readonly IServiceManager serviceManager;
        private Dictionary<string, PreservedState> preservedStates = new Dictionary<string, PreservedState>();

        public PipelineViewModel(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            Pipelines = new ObservableCollection<UiPipeline>();
            SortCommand = new FuncCommand<PipelineSortOrder>(Sort);
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
            var uiPipelines = serviceManager.Estate.Pipelines.Select(p => new UiPipeline(p, preservedStates, serviceManager.GetConnectionInfo().GocdWebUri));
            switch (PipelineSortOrder)
            {
                case PipelineSortOrder.BuildStatus:
                    Pipelines = new ObservableCollection<UiPipeline>(uiPipelines.OrderBy(p => p.StatusAndPausedSortOrder).ThenBy(p => p.Name));
                    break;
                case PipelineSortOrder.AtoZ:
                    Pipelines = new ObservableCollection<UiPipeline>(uiPipelines.OrderBy(p => p.Name));
                    break;
                case PipelineSortOrder.ZtoA:
                    Pipelines = new ObservableCollection<UiPipeline>(uiPipelines.OrderByDescending(p => p.Name));
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

        public ICommand OpenInBrowserCommand { get; } = new FuncCommand<UiPipeline>(pipeline => Process.Start(pipeline.WebsiteUrl));

        public ICommand ToggleDetailsVisibilityCommand { get; } = new FuncCommand<UiPipeline>(pipeline => pipeline.IsExpanded = !pipeline.IsExpanded);
    }
}
