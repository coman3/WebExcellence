using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using WebExcellence.Application.Configuration;
using WebExcellence.Application.Models;
using WebExcellence.Application.Services;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Application.UnitTests
{
    public class BookServiceTests
    {

        private readonly ICollection<BookOwner> _testBookOwners = GetTestBookOwners();  // Shared test data
        private readonly AgeCategoryOptions _testAgeCategoryOptions = new AgeCategoryOptions
        {
            Categories =
            [
                new AgeCategory { Name = "Children", MinAge = 0, MaxAge = 17 },
                new AgeCategory { Name = "Adults", MinAge = 18, MaxAge = 999 }
            ]
        };

        private static ICollection<BookOwner> GetTestBookOwners() =>
            [
                new BookOwner
                {
                    Name = "Jane",
                    Age = 23,
                    Books =
                    [
                        new Book { Name = "Hamlet", Type = BookType.Hardcover },
                        new Book { Name = "Wuthering Heights", Type = BookType.Paperback }
                    ]
                },
                new BookOwner
                {
                    Name = "Charlotte",
                    Age = 14,
                    Books =
                    [
                        new Book { Name = "Hamlet", Type = BookType.Paperback }
                    ]
                },
                new BookOwner
                {
                    Name = "Max",
                    Age = 25,
                    Books =
                    [
                        new Book { Name = "React: The Ultimate Guide", Type = BookType.Hardcover },
                        new Book { Name = "Gulliver's Travels", Type = BookType.Hardcover },
                        new Book { Name = "Jane Eyre", Type = BookType.Paperback },
                        new Book { Name = "Great Expectations", Type = BookType.Hardcover }
                    ]
                },
                new BookOwner
                {
                    Name = "William",
                    Age = 15,
                    Books =
                    [
                        new Book { Name = "Great Expectations", Type = BookType.Hardcover }
                    ]
                },
                new BookOwner
                {
                    Name = "Charles",
                    Age = 17,
                    Books =
                    [
                        new Book { Name = "Little Red Riding Hood", Type = BookType.Hardcover },
                        new Book { Name = "The Hobbit", Type = BookType.Ebook }
                    ]
                }
            ];


        [Fact]
        public async Task ReturnsBooksCategorizedByAge_UnexpectedNullResult()
        {
            // Arrange
            var mockApiClient = new Mock<ISwaggerBooksClient>();
            mockApiClient.Setup(client => client.BookownersAsync())
                .Returns(Task.FromResult<ICollection<BookOwner>>(null!));


            var bookService = new BookService(mockApiClient.Object, Options.Create(_testAgeCategoryOptions));

            // Act
            var result = await bookService.GetBooksCategorizedByAgeAsync(null);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnsBooksCategorizedByAge_UnexpectedNullResult_Filtered()
        {
            // Arrange
            var mockApiClient = new Mock<ISwaggerBooksClient>();
            mockApiClient.Setup(client => client.BookownersAsync())
                .Returns(Task.FromResult<ICollection<BookOwner>>(null!));


            var bookService = new BookService(mockApiClient.Object, Options.Create(_testAgeCategoryOptions));

            // Act
            var result = await bookService.GetBooksCategorizedByAgeAsync(BookType.Ebook);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnsBooksCategorizedByAge_NoFiltering()
        {
            // Arrange
            var mockApiClient = new Mock<ISwaggerBooksClient>();
            mockApiClient.Setup(client => client.BookownersAsync())
                .ReturnsAsync(_testBookOwners);


            var bookService = new BookService(mockApiClient.Object, Options.Create(_testAgeCategoryOptions));

            // Act
            var result = await bookService.GetBooksCategorizedByAgeAsync(null);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(c => c.Name == "Children");   
            result.Should().ContainSingle(c => c.Name == "Adults");

            result.Should().BeInAscendingOrder(c => c.Name);
            result.Single(x=> x.Name == "Children").Books.Should().HaveCount(4);
            result.Single(x => x.Name == "Adults").Books.Should().HaveCount(6);
            result.Single(x => x.Name == "Children").Books.Should().BeInAscendingOrder(b => b.Name);
            result.Single(x => x.Name == "Adults").Books.Should().BeInAscendingOrder(b => b.Name);

        }

        [Fact]
        public async Task ReturnsBooksCategorizedByAge_FilteringPaperback()
        {
            // Arrange
            var mockApiClient = new Mock<ISwaggerBooksClient>();
            mockApiClient.Setup(client => client.BookownersAsync())
                .ReturnsAsync(_testBookOwners);


            var bookService = new BookService(mockApiClient.Object, Options.Create(_testAgeCategoryOptions));

            // Act
            var result = await bookService.GetBooksCategorizedByAgeAsync(BookType.Paperback);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(c => c.Name == "Children");
            result.Should().ContainSingle(c => c.Name == "Adults");

            result.Should().BeInAscendingOrder(c => c.Name);
            result.Single(x => x.Name == "Children").Books.Should().HaveCount(1);
            result.Single(x => x.Name == "Adults").Books.Should().HaveCount(2);
            result.Should().AllSatisfy(c => c.Books.Should().NotContain(b => b.Type != BookType.Paperback));

        }

        [Fact]
        public async Task ReturnsEmptyList_WhenNoMatchingBooks()
        {
            // Arrange
            var mockApiClient = new Mock<ISwaggerBooksClient>();
            mockApiClient.Setup(client => client.BookownersAsync())
                .ReturnsAsync(_testBookOwners);


            var bookService = new BookService(mockApiClient.Object, Options.Create(_testAgeCategoryOptions));

            // Act
            var result = await bookService.GetBooksCategorizedByAgeAsync((BookType)999); // nonexisting book type (not defined in assumed enum)

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}