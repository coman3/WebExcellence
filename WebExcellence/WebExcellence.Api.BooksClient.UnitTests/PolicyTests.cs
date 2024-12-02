using Polly.Registry;
using Polly;
using System.Net;
using WebExcellence.External.Api.BooksClient;
using Moq;
using Moq.Protected;
using FluentAssertions;

namespace WebExcellence.Api.BooksClient.UnitTests
{
    public class PolicyTests
    {
        [Fact]
        public async Task Http_Retry_Policy_RetriesOnTransient_Unavaliable_Errors()
        {
            // Arrange
            var retryCount = 0;
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    retryCount++;
                    if (retryCount < 3) // Simulate two transient errors at system level
                    {
                        return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                    }
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });


            var httpClient = new HttpClient(handlerMock.Object);
            var registry = new PolicyRegistry().AddBookClientPolicies();
            var policy = registry.Get<IAsyncPolicy<HttpResponseMessage>>(BooksClientPolicies.Http_Retry_Policy);

            // Act
            var response = await policy.ExecuteAsync(() => httpClient.GetAsync("http://test.com"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            retryCount.Should().Be(3); // Should have retried twice
        }

        [Fact]
        public async Task Http_Retry_Policy_RetriesOnTransient_ToManyRequests_Errors()
        {
            // Arrange
            var retryCount = 0;
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    retryCount++;
                    if (retryCount < 3) // Simulate two transient errors at system level
                    {
                        return new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                        {
                            Content = new StringContent("Too many requests")
                        };
                    }
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });


            var httpClient = new HttpClient(handlerMock.Object);
            var registry = new PolicyRegistry().AddBookClientPolicies();
            var policy = registry.Get<IAsyncPolicy<HttpResponseMessage>>(BooksClientPolicies.Http_Retry_Policy);

            // Act
            var response = await policy.ExecuteAsync(() => httpClient.GetAsync("http://test.com"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            retryCount.Should().Be(3); // Should have retried twice
        }

        [Fact]
        public async Task BooksOwnerCollection_NotNull_Retry_Policy_RetriesOnNullResult()
        {
            // Arrange
            var retryCount = 0;
            var mockClient = new Mock<SwaggerBooksClient>(new HttpClient()); // Mock the underlying client
            mockClient.Setup(c => c.BookownersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    retryCount++;
                    if (retryCount < 3) // Simulate two null results
                    {
                        return null;
                    }
                    return [new BookOwner()]; // Return a non-null result
                });


            var registry = new PolicyRegistry().AddBookClientPolicies();
            var policy = registry.Get<IAsyncPolicy<ICollection<BookOwner>>>(BooksClientPolicies.BooksOwnerCollection_NotNull_Retry_Policy);

            // Act
            var result = await policy.ExecuteAsync(() => mockClient.Object.BookownersAsync(CancellationToken.None));

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            retryCount.Should().Be(3);
        }


        [Fact]
        public void AddBookClientPolicies_AddsPoliciesToRegistry()
        {
            // Arrange
            var registry = new PolicyRegistry();

            // Act
            registry.AddBookClientPolicies();

            // Assert
            registry.Count.Should().Be(2);
            registry.Get<IAsyncPolicy<HttpResponseMessage>>(BooksClientPolicies.Http_Retry_Policy).Should().NotBeNull();
            registry.Get<IAsyncPolicy<ICollection<BookOwner>>>(BooksClientPolicies.BooksOwnerCollection_NotNull_Retry_Policy).Should().NotBeNull();

        }
    }
}