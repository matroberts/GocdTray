using System;
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

        public Result<List<Pipeline>> Pipelines { get; set; }
        public Result<List<Pipeline>> GetPipelines()
        {
            if(Pipelines==null)
                throw new ArgumentException("GocdServiceFake you need to set a return value for GetPipelines()");
            return Pipelines;
        }
    }
}