using System.Collections.Generic;
using Newtonsoft.Json;

namespace GocdTray.Rest
{
    public class GoEmbedded<T>
    {
        public T _embedded { get; set; }
    }

    public class PipelineGroupsList
    {
        [JsonProperty("pipeline_groups")]
        public List<PipelineGroup> PipelineGroups { get; set; }
    }

    public class PipelineGroup
    {
        public string Name { get; set; }
        public PipelineList _embedded { get; set; }
    }

    public class PipelineList
    {
        public List<Pipeline> pipelines { get; set; }
    }

    public class Pipeline
    {
        public string Name { get; set; }
    }
}