using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
