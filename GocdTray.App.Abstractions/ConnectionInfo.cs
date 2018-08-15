namespace GocdTray.App.Abstractions
{
    public class ConnectionInfo
    {
        public string GocdApiUri { get; set; }
        public string GocdWebUri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IgnoreCertificateErrors { get; set; }
        public int PollingIntervalSeconds { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ConnectionInfo connectionInfo))
                return false;

            return GocdApiUri == connectionInfo.GocdApiUri
                   && Username == connectionInfo.Username
                   && Password == connectionInfo.Password
                   && IgnoreCertificateErrors == connectionInfo.IgnoreCertificateErrors
                   && PollingIntervalSeconds == connectionInfo.PollingIntervalSeconds;
        }

        public override string ToString()
        {
            return $"GocdApiUri '{GocdApiUri}' GocdWebUri'{GocdWebUri}' Username '{Username}' Password '{Password}' IgnoreCertificateErrors '{IgnoreCertificateErrors}' PollingIntervalSeconds '{PollingIntervalSeconds}'";
        }
    }
}