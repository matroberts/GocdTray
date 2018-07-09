using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GocdTray.App;
using GocdTray.Rest;
using Newtonsoft.Json;
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

        public class TestObject
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        [Test]
        public void Get_IfTheRequestIsInvalid_TheExceptionIsCaught_AndTheMessagesReturnedInTheRestResult()
        {
            // Arrange
            // Invalid ssl certificate causes and error on the request
            var httpClientHandler = new HttpClientHandlerFakeWithFunc() { SendAsyncFunc = (r, c) => throw new HttpRequestException("An error occurred while sending the request.", new WebException("The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel.")) };
            RestResult<object> result;

            // Act
            using (var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, false, httpClientHandler))
            {
                result = restClient.Get<object>("/go/api/dashboard", "application/vnd.go.cd.v1+json");

            }

            // Assert
            Assert.That(result.HasData, Is.False);
            Assert.That(result.ToString(), Is.EqualTo("An error occurred while sending the request. The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel."));
        }

        [Test]
        public void Get_IfTheResponseIsInvalid_TheMessageIsFormatted_AndReturnedInTheRestResult()
        {
            // Arrange
            // Invalid password causes Unauthorised in response
            var httpClientHandler = new HttpClientHandlerFakeWithFunc { SendAsyncFunc = (r, c) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "" })};
            RestResult<object> result;

            // Act
            using (var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, false, httpClientHandler))
            {
                result = restClient.Get<object>("/go/api/dashboard", "application/vnd.go.cd.v1+json");
            }

            //Assert
            Assert.That(result.HasData, Is.False);
            Assert.That(result.ToString(), Is.EqualTo("Response status code does not indicate success: 401 (Unauthorized)."));
        }

        [Test]
        public void Get_SetsUpTheRequestCorrectly_AndDeserialisesTheResult()
        {
            // Arrange
            string url = string.Empty;
            string method = string.Empty;
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> acceptHeaders = null;
            AuthenticationHeaderValue authentication = null;

            var jsonResult = JsonConvert.SerializeObject(new TestObject() {Name = "to", Value = 2});
            var httpClientHandler = new HttpClientHandlerFakeWithFunc()
            {
                SendAsyncFunc = (r, c) =>
                {
                    url = r.RequestUri.ToString();
                    method = r.Method.ToString();
                    acceptHeaders = r.Headers.Accept;
                    authentication = r.Headers.Authorization;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(jsonResult)});
                }
            };
            RestResult<TestObject> result;

            // Act
            using (var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, false, httpClientHandler))
            {
                result = restClient.Get<TestObject>("/go/api/dashboard", "application/vnd.go.cd.v1+json");
            }

            //Assert
            Assert.That(result.HasData, Is.True);
            Assert.That(url, Is.EqualTo(AppConfig.GocdApiUri + "/go/api/dashboard"));
            Assert.That(method, Is.EqualTo("GET"));
            Assert.That(acceptHeaders.Count, Is.EqualTo(1));
            Assert.That(acceptHeaders.First().ToString(), Is.EqualTo("application/vnd.go.cd.v1+json"));
            Assert.That(authentication.Scheme, Is.EqualTo("Basic"));
            Assert.That(authentication.Parameter, Is.EqualTo(Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppConfig.Username}:{AppConfig.Password}"))));
            Assert.That(result.Data.Name, Is.EqualTo("to"));
            Assert.That(result.Data.Value, Is.EqualTo(2));
        }
    }
}