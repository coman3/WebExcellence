using FluentAssertions;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Api.BooksClient.IntergrationTests
{
    public class ReliableBooksClientTests(ISwaggerBooksClient booksClient)
    {
        [Fact]
        public async Task IsValidModelReturned()
        {
            //Arrange: Startup.cs

            //Act
            var result = await booksClient.BookownersAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
    }
}