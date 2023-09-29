using E_commerce.Models;
using E_commerce.Pages;
using E_commerce.ViewModels;
using Ecommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Tests
{
    public class LoginModelTests
    {
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly EcommerceContext _context;

        public LoginModelTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null);

            // Create a mock of SignInManager with the right dependencies
            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);

            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<EcommerceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_LoginModel")
                .Options;

            _context = new EcommerceContext(options);
        }

        // Test methods go here
        [Fact]
        public async Task OnPost_ValidLogin_RedirectsToIndex()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Arrange: Set up the mock to return a successful login result
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            using (var context = _context)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Create and add a user to the in-memory database
                var user = new User
                {
                    UserName = "test@example.com",
                    Email = "test@example.com",
                };
                await _userManagerMock.Object.CreateAsync(user, "Password123");

                var model = new LoginModel(_signInManagerMock.Object)
                {
                    LoginViewModel = new LoginViewModel
                    {
                        Email = "test@example.com",
                        Password = "Password123", // A valid password
                    }
                };

                // Act: Call the OnPost method with valid credentials
                var result = await model.OnPost();

                // Assert: Check that it redirects to the Index page
                var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
                Assert.Equal("/Index", redirectToPageResult.PageName);
            }

        }
    }
}
