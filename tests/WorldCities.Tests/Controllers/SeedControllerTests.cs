using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using WorldCities.API.Controllers;
using WorldCities.Persistence;
using WorldCities.UnitTests.API.Helpers;
using Xunit;

namespace WorldCities.UnitTests.API.Controllers;

public class SeedControllerTests
{
    [Fact]
    public async Task CreateDefaultUsersAsync()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("WorldCities")
            .Options;

        var mockEnv = Mock.Of<IWebHostEnvironment>();

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s ==
            "DefaultPasswords:RegisteredUser")])
            .Returns("M0ckP$$word");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s ==
            "DefaultPasswords:Administrator")])
            .Returns("M0ckP$$word");

        using var context = new ApplicationDbContext(options);

        var roleManager = IdentityHelper.GetRoleManager(
            new RoleStore<IdentityRole>(context));

        var userManager = IdentityHelper.GetUserManager(
            new UserStore<ApplicationUser>(context));

        var controller = new SeedController(
            context,
            roleManager,
            userManager,
            mockEnv,
            mockConfiguration.Object);

        // Act
        await controller.CreateDefaultUsersAsync();

        var user_Admin = await userManager.FindByEmailAsync(
            "admin@email.com");
        var user_User = await userManager.FindByEmailAsync(
            "user@email.com");
        var user_NotExisting = await userManager.FindByEmailAsync(
            "notexisting@email.com");

        // Assert
        using (new AssertionScope())
        {
            user_Admin.Should().NotBeNull();
            user_User.Should().NotBeNull();
            user_NotExisting.Should().BeNull();
        }
    }
}