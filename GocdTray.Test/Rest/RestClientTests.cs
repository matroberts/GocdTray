using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GocdTray.App;
using GocdTray.Rest;
using NUnit.Framework;

namespace GocdTray.Test.Rest
{
    [TestFixture]
    public class RestClientTests
    {
        public class HttpClientHandlerFakeWithFunc : HttpMessageHandler
        {
            public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> SendAsyncFunc { get; set; }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return SendAsyncFunc(request, cancellationToken);
            }
        }

        [Test]
        public void RealConnection()
        {
            RestResult<object> result;
            using (var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, true))
            {
                result = restClient.Get<object>("/go/api/dashboard", "application/vnd.go.cd.v1+json");

            }

            Assert.That(result.HasData, result.ToString());
            Console.WriteLine(result.Data);
        }

        [Test]
        public void Get_IfTheRequestIsInvalid_TheExceptionIsCaught_AndTheMessagesReturnedInTheRestResult()
        {
            // Invalid ssl certificate causes and error on the request
            var httpClientHandler = new HttpClientHandlerFakeWithFunc() { SendAsyncFunc = (r, c) => throw new HttpRequestException("An error occurred while sending the request.", new WebException("The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel.")) };
            RestResult<object> result;

            using (var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, false, httpClientHandler))
            {
                result = restClient.Get<object>("/go/api/dashboard", "application/vnd.go.cd.v1+json");

            }

            Assert.That(result.HasData, Is.False);
            Assert.That(result.ToString(), Is.EqualTo("An error occurred while sending the request. The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel."));
        }

        [Test]
        public void Get_IfTheResponseIsInvalid_TheMessageIsFormatted_AndReturnedInTheRestResult()
        {
            // Invalid password causes Unauthorised in response
            var httpClientHandler = new HttpClientHandlerFakeWithFunc { SendAsyncFunc = (r, c) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "" })};

            RestResult<object> result;

            using (var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, false, httpClientHandler))
            {
                result = restClient.Get<object>("/go/api/dashboard", "application/vnd.go.cd.v1+json");
            }

            Assert.That(result.HasData, Is.False);
            Assert.That(result.ToString(), Is.EqualTo("Response status code does not indicate success: 401 (Unauthorized)."));
        }

        // authentication
        // url
        // accept header


        // invliad urls
        // if it throws exceptions (e.g. cert not vliad)

        // How deal with failure to connect, or interuption?
        // Not doing it now but may want to connect to more than one go.cd
        // Need to process the return data, and test the processing
        // async
        // request errors - catach exceptions
        // response errors - validation object

    }
}