using System.Configuration;

namespace GocdTray.App
{
    public class AppConfig
    {
        static AppConfig()
        {
            GocdApiUri = ConfigurationManager.AppSettings["GocdApiUri"];
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];
            bool.TryParse(ConfigurationManager.AppSettings["IgnoreCertificateErrors"], out bool ignoreCertificateErrors);
            IgnoreCertificateErrors = ignoreCertificateErrors;
        }

        public static string GocdApiUri { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static bool IgnoreCertificateErrors { get; set; }
    }
}