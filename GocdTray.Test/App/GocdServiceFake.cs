using System.Collections.Generic;
using GocdTray.App.Abstractions;
using GocdTray.Rest;

namespace GocdTray.Test.App
{
    public class GocdServiceFake : IGocdService
    {
        public void Dispose()
        {
        }

        public Result<List<Pipeline>> Pipelines { get; set; } = new Result<List<Pipeline>>();
        public Result<List<Pipeline>> GetPipelines()
        {
            return Pipelines;
        }
    }
}