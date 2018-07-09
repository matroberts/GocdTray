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
    }
}