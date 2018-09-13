using System;
using System.Collections.Generic;
using System.Linq;

namespace GocdTray.App.Abstractions
{
    public class Pipeline
    {
        public string Name { get; set; }
        public string PipelineGroupName { get; set; }
        public bool Locked { get; set; }
        public bool Paused { get; set; }
        public string PausedBy { get; set; } 
        public string PausedReason { get; set; }
        public List<PipelineInstance> PipelineInstances { get; set; } = new List<PipelineInstance>();
        public string WebsiteUrl => PipelineInstances.Any() ? $"go/pipelines/{Name}/{PipelineInstances.First().Label}/{PipelineInstances.First().Stages.First().Name}/{PipelineInstances.First().Stages.First().Run}" : "go/pipelines/";

        public PipelineStatus Status
        {
            get
            {
                if (PipelineInstances.Any(i => i.Status == PipelineStatus.Building))
                    return PipelineStatus.Building;
                else if (PipelineInstances.Any(i => i.Status == PipelineStatus.Failed))
                    return PipelineStatus.Failed;
                else
                    return PipelineStatus.Passed;
            }
        }
    }

    public enum PipelineStatus
    {
        Failed,
        Building,
        Passed,
    }

    public class PipelineInstance
    {
        public string Label { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string TriggeredBy { get; set; }
        public List<Stage> Stages { get; set; } = new List<Stage>();

        public PipelineStatus Status
        {
            get
            {
                if (Stages.Any(s => s.Status == StageStatus.Building))
                    return PipelineStatus.Building;
                else if (Stages.Any(s => s.Status == StageStatus.Cancelled || s.Status == StageStatus.Failed))
                    return PipelineStatus.Failed;
                else 
                    return PipelineStatus.Passed;
            }
        }
    }

    public class Stage
    {
        public string Name { get; set; }
        public StageStatus Status { get; set; }
        public int Run { get; set; }

        // Only defined when Status==Building
        public StageStatus? PreviousStatus { get; set; }
    }

    public enum StageStatus
    {
        Unknown,
        Passed,
        Building,
        Failed,
        Cancelled
    }
}