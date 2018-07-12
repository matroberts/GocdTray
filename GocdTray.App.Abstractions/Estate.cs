using System.Collections.Generic;

namespace GocdTray.App.Abstractions
{
    public class Estate
    {
        List<Pipeline> Pipelines { get; }
        public EstateState State { get; set; }
    }

    public enum EstateState
    {
        NotConnected,
        Building,
        Passed,
        Failed
    }
}