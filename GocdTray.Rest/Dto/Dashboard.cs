using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GocdTray.Rest.Dto
{
    public class GoEmbedded<T>
    {
        public T _embedded { get; set; }
    }

    public class GoPipelineGroupsList
    {
        [JsonProperty("pipeline_groups")]
        public List<GoPipelineGroup> PipelineGroups { get; set; }
    }

    public class GoPipelineGroup
    {
        public string Name { get; set; }
        public GoPipelineList _embedded { get; set; }
    }

    public class GoPipelineList
    {
        public List<GoPipeline> pipelines { get; set; }
    }

    public class GoPipeline
    {
        public string Name { get; set; }
    }
}