using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using AstrolPOSAPI.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace AstrolPOSAPI.UnitTests
{
    public class NoSeriesTests
    {
        [Fact]
        public async Task GenerateNextNumberAsync_Should_Return_FormattedNumber()
        {
            // Arrange
            var mockRepo = new Mock<IGenericRepository<NoSeries>>();
            var mockUow = new Mock<IUnitOfWork>();
            
            var noSeries = new NoSeries
            {
                Code = "TEST",
                Prefix = "PRE-",
                Suffix = "-SUF",
                CurrentNo = "5"
            };

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<NoSeries> { noSeries });
            mockUow.Setup(u => u.Repository<NoSeries>()).Returns(mockRepo.Object);

            var service = new NoSeriesService(mockUow.Object);

            // Act
            var result = await service.GenerateNextNumberAsync("TEST");

            // Assert
            result.Should().Be("PRE-006-SUF");
            noSeries.CurrentNo.Should().Be("6");
            mockUow.Verify(u => u.Save(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GenerateNextNumberAsync_Should_ThrowException_When_NotFound()
        {
            // Arrange
            var mockRepo = new Mock<IGenericRepository<NoSeries>>();
            var mockUow = new Mock<IUnitOfWork>();

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<NoSeries>());
            mockUow.Setup(u => u.Repository<NoSeries>()).Returns(mockRepo.Object);

            var service = new NoSeriesService(mockUow.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GenerateNextNumberAsync("NON-EXISTENT"));
        }
    }
}
