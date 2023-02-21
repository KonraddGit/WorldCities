using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WorldCities.Domain.Entities;
using WorldCities.Persistence;
using WorldCitiesAPI.Controllers;
using Xunit;

namespace WorldCities.UnitTests.API.Controllers;

public class CitiesControllerTests
{
    [Fact]
    public async Task GetCity()
    {
        // Arrange
        var logger = new Mock<ILogger<CitiesController>>().Object;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "WorldCities")
            .Options;
        using var context = new ApplicationDbContext(options);

        context.Add(new City()
        {
            Id = 1,
            CountryId = 1,
            Lat = 1,
            Lon = 1,
            Name = "TestCity1"
        });

        await context.SaveChangesAsync();

        var controller = new CitiesController(logger, context);

        // Act
        var city_exitising = (await controller.GetCity(1)).Value;
        var city_notExisting = (await controller.GetCity(2)).Value;

        // Assert
        using (new AssertionScope())
        {
            city_exitising.Should().NotBeNull();
            city_notExisting.Should().BeNull();
        }
    }
}
