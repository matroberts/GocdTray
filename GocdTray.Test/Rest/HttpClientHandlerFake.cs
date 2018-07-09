using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GocdTray.Test.Rest
{
    public class HttpClientHandlerFake : HttpMessageHandler
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public Uri RequestUri { get; set; }
        public HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> AcceptHeaders { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            AcceptHeaders = request.Headers.Accept;

            return Task.FromResult<HttpResponseMessage>(HttpResponseMessage);
        }
    }
}