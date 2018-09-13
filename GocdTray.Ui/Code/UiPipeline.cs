using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GocdTray.App.Abstractions;

namespace GocdTray.Ui.Code
{
    public class UiPipeline : INotifyPropertyChanged
    {
        private readonly Pipeline pipeline;
        private readonly string websiteBaseUri;

        public UiPipeline(Pipeline pipeline, string websiteBaseUri)
        {
            this.pipeline = pipeline;
            this.websiteBaseUri = websiteBaseUri;
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        public string Name => pipeline.Name;
        public string PipelineGroupName => pipeline.PipelineGroupName;
        public bool Locked => pipeline.Locked;
        public bool Paused => pipeline.Paused;
        public string PausedBy => pipeline.PausedBy;
        public string PausedReason => pipeline.PausedReason;
        public string WebsiteUrl => new Uri(new Uri(websiteBaseUri, UriKind.Absolute), pipeline.WebsiteUrl).ToString();
        public PipelineStatus Status => pipeline.Status;

        public int StatusAndPausedSortOrder
        {
            get
            {
                if (Status == PipelineStatus.Failed)
                    return 1;
                if (Status == PipelineStatus.Building)
                    return 2;
                if (Paused == true)
                    return 3;
                return 4;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}