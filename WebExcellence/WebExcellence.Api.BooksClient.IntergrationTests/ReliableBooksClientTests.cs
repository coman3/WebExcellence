using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Api.BooksClient.IntergrationTests
{
    public class ReliableBooksClientTests(ISwaggerBooksClient booksClient, TestHttpMessageHandler requestHandler)
    {
        [Fact]
        public async Task IsValidModelReturned()
        {
            requestHandler.Reset();
            //Arrange: Startup.cs

            //Act
            var result = await booksClient.BookownersAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            requestHandler.Requests.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task BadGatewayResultsInPolicyRetry()
        {
            requestHandler.Reset();
            requestHandler.FailFirstRequests = 2;
            requestHandler.FailCode = HttpStatusCode.BadGateway;
            //Arrange: Startup.cs

            //Act
            var result = await booksClient.BookownersAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            requestHandler.Requests.Should().BeGreaterThan(2);
        }

        [Fact]
        public async Task ToManyRequestsResultsInPolicyRetry()
        {
            requestHandler.Reset();
            requestHandler.FailFirstRequests = 2;
            requestHandler.FailCode = HttpStatusCode.TooManyRequests;
            requestHandler.FailContent = (_) => new StringContent("To many requests.", new MediaTypeHeaderValue("text/plain"));
            //Arrange: Startup.cs

            //Act
            var result = await booksClient.BookownersAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            requestHandler.Requests.Should().BeGreaterThan(2);
        }

        [Fact]
        public async Task NullOkResponseResultsInPolicyRetry()
        {
            requestHandler.Reset();
            requestHandler.FailFirstRequests = 2;
            requestHandler.FailCode = HttpStatusCode.OK;
            requestHandler.FailContent = (_) => new StringContent("", new MediaTypeHeaderValue("application/json"));
            //Arrange: Startup.cs

            //Act
            var result = await booksClient.BookownersAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            requestHandler.Requests.Should().BeGreaterThan(2);
        }
    }
}