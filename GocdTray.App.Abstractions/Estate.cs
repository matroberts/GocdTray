﻿using System.Collections.Generic;
using System.Linq;

namespace GocdTray.App.Abstractions
{
    public class Estate
    {
        private readonly Result<List<Pipeline>> result;
        public Estate(Result<List<Pipeline>> result)
        {
            this.result = result;
        }
        public List<Pipeline> Pipelines => result.IsValid ? result.Data : new List<Pipeline>();

        public EstateStatus Status
        {
            get
            {
                if (result.IsValid == false)
                    return EstateStatus.NotConnected;
                else if (Pipelines.Any(i => i.Status == PipelineStatus.Failed && i.Paused==false)) // don't count paused builds in global status
                    return EstateStatus.Failed;
                else if (Pipelines.Any(i => i.Status == PipelineStatus.Building && i.Paused==false))
                    return EstateStatus.Building;
                else
                    return EstateStatus.Passed;
            }
        }
    }

    public enum EstateStatus
    {
        NotConnected,
        Failed,
        Building,
        Passed,
    }
}