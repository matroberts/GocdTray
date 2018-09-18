using System;
using System.Collections.Generic;
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

        public string DisplayText => $"Build {pipelineInstance.Label} triggered by {pipelineInstance.TriggeredBy} {pipelineInstance.ScheduledAt.TimeAgo()}";
        public IEnumerable<Stage> Stages => pipelineInstance.Stages;
    }
}