using GocdTray.App.Abstractions;

namespace GocdTray.Rest
{
    public interface IGocdServiceFactory
    {
        IGocdService Create(ConnectionInfo connectionInfo);
    }

    public class GocdServiceFactory : IGocdServiceFactory
    {
        public IGocdService Create(ConnectionInfo connectionInfo)
        {
            return new GocdService(new RestClient(connectionInfo.GocdApiUri, connectionInfo.Username, connectionInfo.Password, connectionInfo.IgnoreCertificateErrors));
        }
    }
}