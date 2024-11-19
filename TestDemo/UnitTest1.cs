using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using KoiPool_Project.Controllers;
using KoiPool_Project.Models;
using KoiPool_Project.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace TestDemo
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<UserManager<AppUserModel>> _mockUserManager;
        private Mock<SignInManager<AppUserModel>> _mockSignInManager;
        private Mock<DataContext> _mockContext;
        private AccountController _accountController;

        [TestInitialize]
        public void TestInitialize()
        {
            // Mock UserManager
            var userStoreMock = new Mock<IUserStore<AppUserModel>>();
            _mockUserManager = new Mock<UserManager<AppUserModel>>(
                userStoreMock.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<AppUserModel>>(),
                Array.Empty<IUserValidator<AppUserModel>>(),
                Array.Empty<IPasswordValidator<AppUserModel>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<AppUserModel>>>());

            // Mock SignInManager dependencies
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUserModel>>();
            var optionsMock = Mock.Of<IOptions<IdentityOptions>>();
            var loggerMock = Mock.Of<ILogger<SignInManager<AppUserModel>>>();
            var schemeProviderMock = Mock.Of<IAuthenticationSchemeProvider>();
            var userConfirmationMock = Mock.Of<IUserConfirmation<AppUserModel>>();

            // Initialize SignInManager
            _mockSignInManager = new Mock<SignInManager<AppUserModel>>(
                _mockUserManager.Object,
                httpContextAccessorMock.Object,
                userClaimsPrincipalFactoryMock.Object,
                optionsMock,
                loggerMock,
                schemeProviderMock,
                userConfirmationMock);

            // Mock DataContext
            _mockContext = new Mock<DataContext>();

            // Initialize AccountController
            _accountController = new AccountController(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockContext.Object);
        }


        [TestMethod]
        public async Task Login_ValidUser_RedirectsToHome()
        {
            // Arrange
            var loginVM = new LoginViewModel
            {
                Username = "testuser",
                Password = "Password123!"
            };

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false))
                .ReturnsAsync(SignInResult.Success);

            var user = new AppUserModel { UserName = loginVM.Username };
            _mockUserManager.Setup(x => x.FindByNameAsync(loginVM.Username)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            // Act
            var result = await _accountController.Login(loginVM) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }


        [TestMethod]
        public async Task Login_AdminUser_RedirectsToAdminArea()
        {
            // Arrange
            var loginVM = new LoginViewModel
            {
                Username = "admin",
                Password = "Admin123!"
            };

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var adminUser = new AppUserModel { UserName = loginVM.Username };
            _mockUserManager.Setup(x => x.FindByNameAsync(loginVM.Username)).ReturnsAsync(adminUser);
            _mockUserManager.Setup(x => x.IsInRoleAsync(adminUser, "Admin")).ReturnsAsync(true);

            // Act
            var result = await _accountController.Login(loginVM) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.AreEqual("Admin", result.RouteValues["area"]);
        }

        [TestMethod]
        public async Task Login_InvalidUser_ReturnsViewWithError()
        {
            // Arrange
            var loginVM = new LoginViewModel
            {
                Username = "wronguser",
                Password = "WrongPass123!"
            };

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _accountController.Login(loginVM) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_accountController.ModelState.ContainsKey(""));
            Assert.AreEqual("Tên đăng nhập hoặc mật khẩu không chính xác",
                          _accountController.ModelState[""].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task Create_ValidUser_RedirectsToHome()
        {
            // Arrange
            var user = new UserModel
            {
                Username = "newuser",
                Password = "Password123!",
                Email = "newuser@example.com"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync((AppUserModel)null);
            _mockUserManager.Setup(x => x.FindByNameAsync(user.Username)).ReturnsAsync((AppUserModel)null);
            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUserModel>(), user.Password))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUserModel>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountController.Create(user) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);

            // Verify role assignment
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUserModel>(), "User"), Times.Once);
        }

        [TestMethod]
        public async Task Create_ExistingEmail_ReturnsViewWithError()
        {
            // Arrange
            var user = new UserModel
            {
                Username = "newuser",
                Password = "Password123!",
                Email = "existing@example.com"
            };

            var existingUser = new AppUserModel { Email = user.Email };
            _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(existingUser);

            // Act
            var result = await _accountController.Create(user) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_accountController.ModelState.ContainsKey("Email"));
            Assert.AreEqual("Email này đã được sử dụng",
                          _accountController.ModelState["Email"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task Create_ExistingUsername_ReturnsViewWithError()
        {
            // Arrange
            var user = new UserModel
            {
                Username = "existinguser",
                Password = "Password123!",
                Email = "new@example.com"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync((AppUserModel)null);
            _mockUserManager.Setup(x => x.FindByNameAsync(user.Username))
                .ReturnsAsync(new AppUserModel { UserName = user.Username });

            // Act
            var result = await _accountController.Create(user) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_accountController.ModelState.ContainsKey("Username"));
            Assert.AreEqual("Tên đăng nhập này đã được sử dụng",
                          _accountController.ModelState["Username"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task Logout_RedirectsToReturnUrl()
        {
            // Arrange
            var returnUrl = "/";

            // Act
            var result = await _accountController.Logout(returnUrl) as RedirectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(returnUrl, result.Url);
            _mockSignInManager.Verify(x => x.SignOutAsync(), Times.Once);
        }
    }
}