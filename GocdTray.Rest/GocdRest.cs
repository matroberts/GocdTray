using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GocdTray.Rest
{
    public class RestClient
    {
        // httpClient

        // baseuri is the primary key
        public RestClient(string baseuri, string username, string password, bool ignoreCertificatErrors = true)
        {
            
        }

        public string Get(string relativeUri, string acceptHeaders = null)
        {
            return null;
        }
    }

    public class GocdServer
    {
        private readonly string baseuri;
        private readonly string username;
        private readonly string password;
        private readonly bool ignoreCertificatErrors;

        public GocdServer(string baseuri, string username, string password, bool ignoreCertificatErrors = true)
        {
            this.baseuri = baseuri;
            this.username = username;
            this.password = password;
            this.ignoreCertificatErrors = ignoreCertificatErrors;
        }

        public RestResult<GoDashboard> GetDashboard()
        {
            var webRequestHandler = new WebRequestHandler();
            if (ignoreCertificatErrors)
            {
                webRequestHandler.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }

            var client = new HttpClient(webRequestHandler) { BaseAddress = new Uri(baseuri) };
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

            var request = new HttpRequestMessage(HttpMethod.Get, "/go/api/dashboard");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            request.Headers.Add("Accept", "application/vnd.go.cd.v1+json");

            var response = client.SendAsync(request).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                var data = response.Content.ReadAsStringAsync().FromJsonAsync<GoDashboard>();
                return data.GetAwaiter().GetResult();
            }
            else
            {
                return new RestError((int) response.StatusCode, response.StatusCode.ToString());
            }
        }


    }
}
