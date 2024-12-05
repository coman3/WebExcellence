using Microsoft.Extensions.DependencyInjection;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Api.BooksClient
{

    public static class ReliableBooksClientExtensions
    {
        public static IHttpClientBuilder AddReliableBooksHttpClient(this IServiceCollection services, string baseUrl)
        {
            services.AddPolicyRegistry()
                .AddBookClientPolicies();

            return services.AddHttpClient<ISwaggerBooksClient, ReliableBooksClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandlerFromRegistry(BooksClientPolicies.Http_Retry_Policy);
        }
    }
}
