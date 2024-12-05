using Polly;
using Polly.Registry;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Api.BooksClient
{
    /// <summary>
    /// Resilient BooksClient that wraps the base <see cref="SwaggerBooksClient"/> with a retry
    /// policy ensuring safe responses from the unreliable source.
    /// </summary>
    public partial class ReliableBooksClient : SwaggerBooksClient
    {
        private readonly IPolicyRegistry<string> registry;

        /// <param name="httpClient"></param>
        public ReliableBooksClient(HttpClient httpClient, IPolicyRegistry<string> registry) : base(httpClient)
        {
            this.registry = registry;

            // Sometimes this API returns upper case property names, so we need to use a custom
            // contract resolver to convert all property names to title case
            JsonSerializerSettings.ContractResolver = new CaseConverterContractResolver();
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
