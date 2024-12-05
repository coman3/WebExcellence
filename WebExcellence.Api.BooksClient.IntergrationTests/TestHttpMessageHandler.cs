using System.Net;

namespace WebExcellence.Api.BooksClient.IntergrationTests
{
    /// <summary>
    /// A test HttpMessageHandler that can be used to simulate network failures and other aspects of querying a remote API.
    /// </summary>
    /// <remarks>
    /// Ideally this would be in a shared test library across projects with some better structure for production code, but in manner of time, i have slapped this here.
    /// </remarks>
    public class TestHttpMessageHandler : DelegatingHandler
    {
        public int Requests { get; private set; } = 0;

        public int? FailFirstRequests { get; set; } = null;
        public HttpStatusCode FailCode { get; set; } = HttpStatusCode.BadGateway;
        public Func<int, HttpContent>? FailContent { get; set; } = null;

        public void Reset()
        {
            Requests = 0;
            FailFirstRequests = 0;
            FailCode = HttpStatusCode.BadGateway;
            FailContent = null;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests++;
            if (FailFirstRequests == null || FailFirstRequests < Requests)
            {
                return base.SendAsync(request, cancellationToken);
            }

            return Task.FromResult(new HttpResponseMessage(FailCode)
            {
                RequestMessage = request,
                Content = FailContent == null ? null : FailContent(Requests)
            });

        }
    }
}
