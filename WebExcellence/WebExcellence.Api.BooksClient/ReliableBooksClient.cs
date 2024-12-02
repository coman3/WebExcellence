using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Registry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Api.BooksClient
{
    /// <summary>
    /// Resilient BooksClient that wraps the base <see cref="SwaggerBooksClient"/> with a retry
    /// policy ensuring safe responses from the unreliable source.
    /// </summary>
    public sealed class ReliableBooksClient : SwaggerBooksClient
    {
        private readonly IPolicyRegistry<string> registry;

        /// <param name="httpClient"></param>
        public ReliableBooksClient(HttpClient httpClient, IPolicyRegistry<string> registry) : base(httpClient)
        {
            this.registry = registry;
            // TODO: Come in here and figure out full property name case insestivity... For now, just ignore it and return broken responses until we can fix it.
        }

        /// <summary>
        /// Wraps the base BookownersAsync method with a retry policy ensuring that the result is not null.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        ///     A collection of BookOwner objects from the API.
        /// </returns>
        public override Task<ICollection<BookOwner>> BookownersAsync(CancellationToken cancellationToken)
        {
            return registry.Get<IAsyncPolicy<ICollection<BookOwner>>>(BooksClientPolicies.BooksOwnerCollection_NotNull_Retry_Policy)
                .ExecuteAsync(() => base.BookownersAsync(cancellationToken));
        }
    }
}
