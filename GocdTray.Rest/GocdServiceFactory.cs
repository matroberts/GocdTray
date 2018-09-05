using GocdTray.App.Abstractions;

namespace GocdTray.Rest
{
    public interface IGocdServiceFactory
    {
        GocdService Create(ConnectionInfo connectionInfo);
    }

    public class GocdServiceFactory : IGocdServiceFactory
    {
        public GocdService Create(ConnectionInfo connectionInfo)
        {
            return new GocdService(new RestClient(connectionInfo.GocdApiUri, connectionInfo.Username, connectionInfo.Password, connectionInfo.IgnoreCertificateErrors));
        }
    }
}