using System;
using System.Collections.Generic;
using System.Linq;
using GocdTray.Rest.Dto;

namespace GocdTray.Rest
{
    public class GocdServer : IDisposable
    {
        private readonly IRestClient restClient;

        public GocdServer(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public RestResult<GoEmbedded<GoPipelineGroupsList>> GetPipelines()
        {
            return restClient.Get<GoEmbedded<GoPipelineGroupsList>>("/go/api/dashboard", "application/vnd.go.cd.v1+json");
        }

        public void Dispose()
        {
            restClient?.Dispose();
        }
    }
}