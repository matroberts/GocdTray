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
    }

    public class PipelineInstance
    {
        public string Label { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string TriggeredBy { get; set; }
        public List<Stage> Stages { get; set; } = new List<Stage>();
    }

    public class Stage
    {
        public string Name { get; set; }
        public StageStatus Status { get; set; }
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