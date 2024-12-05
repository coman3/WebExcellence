using Polly.Extensions.Http;
using Polly;
using WebExcellence.External.Api.BooksClient;
using Polly.Registry;
using System.Net;

namespace WebExcellence.Api.BooksClient
{
    public static class BooksClientPolicies
    {
        public const string Http_Retry_Policy = nameof(BooksClientPolicies) + ":" + nameof(Http_Retry);
        internal static IAsyncPolicy<HttpResponseMessage> Http_Retry()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public const string BooksOwnerCollection_NotNull_Retry_Policy = nameof(BooksClientPolicies) + ":" + nameof(BooksOwnerCollection_NotNull_Retry);
        internal static IAsyncPolicy<ICollection<BookOwner>> BooksOwnerCollection_NotNull_Retry()
        {
            return Policy<ICollection<BookOwner>>
                .HandleResult(result => result == null)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IPolicyRegistry<string> AddBookClientPolicies(this IPolicyRegistry<string> registry)
        {
            registry.Add(Http_Retry_Policy, Http_Retry());
            registry.Add(BooksOwnerCollection_NotNull_Retry_Policy, BooksOwnerCollection_NotNull_Retry());
            return registry;
        }
    }
}
