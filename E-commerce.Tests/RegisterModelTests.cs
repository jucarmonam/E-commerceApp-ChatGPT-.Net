using E_commerce.Models;
using E_commerce.Pages;
using E_commerce.ViewModels;
using Ecommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace E_commerce.Tests
{
    public class RegisterModelTests
    {
        [Fact]
        public async Task OnPost_ValidModel_RedirectsToPage()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null);

            userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

            // Create a DbContextOptions for an in-memory database
            var dbContextOptions = new DbContextOptionsBuilder<EcommerceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            // Create a mock of SignInManager with the right dependencies
            var signInManagerMock = new Mock<SignInManager<User>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);

            using (var context = new EcommerceContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var model = new RegisterModel(userManagerMock.Object, signInManagerMock.Object)
                {
                    RegisterViewModel = new RegisterViewModel
                    {
                        Email = "test@example.com",
                        Password = "Password123", // A valid password
                    }
                };

                // Act
                var result = await model.OnPost();

                // Assert
                Assert.IsType<RedirectToPageResult>(result);
                var redirectResult = result as RedirectToPageResult;
                Assert.Equal("/Index", redirectResult.PageName); // Ensure it redirects to the correct page
            }
        }
    }
}
