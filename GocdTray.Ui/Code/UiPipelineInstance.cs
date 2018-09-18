using System;
using GocdTray.App.Abstractions;

namespace GocdTray.Ui.Code
{
    public class UiPipelineInstance
    {
        private readonly PipelineInstance pipelineInstance;

        public UiPipelineInstance(PipelineInstance pipelineInstance)
        {
            this.pipelineInstance = pipelineInstance;
        }

        public string DisplayText => $"Triggered by {pipelineInstance.TriggeredBy} {pipelineInstance.ScheduledAt.TimeAgo()}";
        public string Label => pipelineInstance.Label;
    }
}