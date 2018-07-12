using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GocdTray.App.Abstractions;

namespace GocdTray.Rest
{
    public interface IRestClient : IDisposable
    {
        Result<T> Get<T>(string relativeUri, string acceptHeaders = null);
    }

    public class RestClient : IRestClient
    {
        private readonly HttpClient httpClient;

        public RestClient(string baseuri, string username, string password, bool ignoreCertificatErrors = true, HttpMessageHandler httpMessageHandler = null)
        {
            if (httpMessageHandler == null)
            {
                var webRequestHandler = new WebRequestHandler();
                if (ignoreCertificatErrors)
                {
                    webRequestHandler.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                }
                httpMessageHandler = webRequestHandler;
            }

            httpClient = new HttpClient(httpMessageHandler) { BaseAddress = new Uri(baseuri) };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
        }

        public Result<T> Get<T>(string relativeUri, string acceptHeaders = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, relativeUri);
                if (acceptHeaders != null)
                    request.Headers.Add("Accept", acceptHeaders);

                var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    var data = response.Content.ReadAsStringAsync().FromJsonAsync<T>();
                    return Result<T>.Valid(data.GetAwaiter().GetResult());
                }
                else
                {
                    return Result<T>.Invalid($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode.ToString()}).");
                }
            }
            catch (Exception e)
            {
                var errorMessage = e.Message;
                if (e.InnerException != null)
                    errorMessage += " " + e.InnerException.Message;
                return Result<T>.Invalid(errorMessage);
            }
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
