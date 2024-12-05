using FluentAssertions;
using WebExcellence.Application.Configuration;
using WebExcellence.Application.Models;

namespace WebExcellence.Application.UnitTests
{
    public class AgeCategoryOptionsTests
    {
        [Fact]
        public void ValidateCategories_EmptyCategories_ThrowsArgumentException()
        {
            // Arrange
            var options = new AgeCategoryOptions();
            //Act
            var act = () => options.ValidateCategories();
            //Assert
            act.Should().Throw<ArgumentException>().WithMessage("Age category configuration is missing or empty.");
        }

        [Fact]
        public void ValidateCategories_OverlappingRanges_ThrowsArgumentException()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "Child", MinAge = 0, MaxAge = 12 },
                    new() { Name = "Teen", MinAge = 10, MaxAge = 19 },
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Overlapping age ranges detected: Child and Teen");
        }

        [Fact]
        public void ValidateCategories_GapsBetweenRanges_ThrowsArgumentException()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "Child", MinAge = 0, MaxAge = 12 },
                    new() { Name = "Adult", MinAge = 14, MaxAge = 65 },  // Gap between 12 and 14
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Gap detected between age ranges: Child and Adult");
        }


        [Fact]
        public void ValidateCategories_MinAgeGreaterThanMaxAge_ThrowsArgumentException()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "Invalid", MinAge = 15, MaxAge = 10 },
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid age category configuration: MinAge cannot be greater than MaxAge.");
        }

        [Fact]
        public void ValidateCategories_NegativeAgeBounds_ThrowsArgumentException()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "Invalid", MinAge = -5, MaxAge = 10 },
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid age category configuration: Age bounds cannot be negative");
        }


        [Fact]
        public void ValidateCategories_ValidConfiguration_DoesNotThrow()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "Child", MinAge = 0, MaxAge = 12 },
                    new() { Name = "Teen", MinAge = 13, MaxAge = 19 },
                    new() { Name = "Adult", MinAge = 20, MaxAge = 65 },
                    new() { Name = "Senior", MinAge = 66, MaxAge = 120 }
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ValidateCategories_MaxValue_DoesNotThrow()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "Senior", MinAge = 66, MaxAge = int.MaxValue },
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ValidateCategories_ZeroAgeCategory_DoesNotThrow()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "Newborn", MinAge = 0, MaxAge = 0 }
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ValidateCategories_EmptyCategoryName_DoesNotThrow()
        {
            // Arrange
            var options = new AgeCategoryOptions
            {
                Categories = new List<AgeCategory>
                {
                    new() { Name = "", MinAge = 0, MaxAge = 12 },
                }
            };
            // Act
            var act = () => options.ValidateCategories();
            // Assert
            act.Should().NotThrow();
        }
    }
}
