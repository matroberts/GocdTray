namespace GocdTray.App.Abstractions
{
    public class ConnectionInfo
    {
        public string GocdApiUri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IgnoreCertificateErrors { get; set; }
        public int PollingIntervalSeconds { get; set; }
    }
}