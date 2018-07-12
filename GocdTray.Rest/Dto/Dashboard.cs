using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GocdTray.Rest.Dto
{
    public class DtoEmbedded<T>
    {
        public T _embedded { get; set; }
    }

    public class DtoPipelineGroupsList
    {
        [JsonProperty("pipeline_groups")]
        public List<DtoPipelineGroup> PipelineGroups { get; set; }
    }

    public class DtoPipelineGroup
    {
        public string Name { get; set; }
        public DtoPipelineList _embedded { get; set; }
    }

    public class DtoPipelineList
    {
        public List<DtoPipeline> pipelines { get; set; }
    }

    public class DtoPipeline
    {
        public string Name { get; set; }
        public bool locked { get; set; }
        public DtoPauseInfo pause_info { get; set; }
        public DtoInstanceList _embedded { get; set; }
    }

    public class DtoPauseInfo
    {
        public bool paused { get; set; }
        public string paused_by { get; set; }
        public string pause_reason { get; set; }
    }

    public class DtoInstanceList
    {
        public List<DtoInstance> instances { get; set; }
    }

    public class DtoInstance
    {
        public string label { get; set; }
        public DateTime schedule_at { get; set; }
        public string triggered_by { get; set; }
        public DtoStageList _embedded { get; set; }
    }

    public class DtoStageList
    {
        public List<DtoStage> stages { get; set; }
    }

    public class DtoStage
    {
        public string name { get; set; }
        public string status { get; set; }
        public DtoStage previous_stage { get; set; }
    }
}