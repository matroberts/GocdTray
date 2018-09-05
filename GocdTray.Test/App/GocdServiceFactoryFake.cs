using GocdTray.App.Abstractions;
using GocdTray.Rest;

namespace GocdTray.Test.App
{
    public class GocdServiceFactoryFake : IGocdServiceFactory
    {
        public GocdServiceFake GocdService { get; set; } = new GocdServiceFake();
        public IGocdService Create(ConnectionInfo connectionInfo)
        {
            return GocdService;
        }
    }
}