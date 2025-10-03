using System.Net;
using LegacyBookStore.Controllers;
using LegacyBookStore.Data;
using LegacyBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace LegacyBookStore.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<AppDbContext> _mockContext;
        private UserController _controller;
        private Mock<DbSet<User>> _mockSet;

        [SetUp]
        public void Setup()
        {
            var users = new List<User>
    {
        new User { Id = 1, Name = "Alice" },
        new User { Id = 2, Name = "Bob" }
    };

            _mockSet = MockDbSetHelper.CreateMockDbSet(users);
            _mockContext = new Mock<AppDbContext>();
            _mockContext.Setup(c => c.Users).Returns(_mockSet.Object);
            _controller = new UserController(_mockContext.Object);
        }

        [TearDown]
        public void Exit()
        {
            _controller.Dispose();
        }

        #region GetUsers

        [Test]
        public void GetUsers_ReturnsOkWithListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "Test User", Email = "test@example.com" }
            }.AsQueryable();

            _mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            // Act
            var result = _controller.GetUsers();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var returnedUsers = okResult?.Value as List<User>;
            Assert.That(returnedUsers, Is.Not.Null);
            Assert.That(returnedUsers.Count, Is.EqualTo(1));
            Assert.That(returnedUsers[0].Name, Is.EqualTo("Test User"));
        }

        #endregion

        #region Welcome

        [Test]
        public void Welcome_WithValidName_ReturnsContentWithEncodedName()
        {
            // Arrange
            string name = "John";

            // Act
            var result = _controller.Welcome(name);

            // Assert
            Assert.That(result, Is.InstanceOf<ContentResult>());
            var contentResult = result as ContentResult;
            Assert.That(contentResult?.Content, Is.EqualTo("<h1>Welcome, John!</h1>"));
            Assert.That(contentResult?.ContentType, Is.EqualTo("text/html"));
        }

        [Test]
        public void Welcome_WithEmptyName_ReturnsContentWithGuest()
        {
            // Act
            var result = _controller.Welcome("");

            // Assert
            Assert.That(result, Is.InstanceOf<ContentResult>());
            var contentResult = result as ContentResult;
            Assert.That(contentResult?.Content, Is.EqualTo("<h1>Welcome, Guest!</h1>"));
            Assert.That(contentResult?.ContentType, Is.EqualTo("text/html"));
        }

        [Test]
        public void Welcome_WithNullName_ReturnsContentWithGuest()
        {
            // Act
            var result = _controller.Welcome(null);

            // Assert
            Assert.That(result, Is.InstanceOf<ContentResult>());
            var contentResult = result as ContentResult;
            Assert.That(contentResult?.Content, Is.EqualTo("<h1>Welcome, Guest!</h1>"));
            Assert.That(contentResult?.ContentType, Is.EqualTo("text/html"));
        }

        [Test]
        public void Welcome_WithHtmlInjection_ReturnsEncodedContent()
        {
            // Arrange
            string name = "<script>alert('xss')</script>";

            // Act
            var result = _controller.Welcome(name);

            // Assert
            Assert.That(result, Is.InstanceOf<ContentResult>());
            var contentResult = result as ContentResult;
            var expected = WebUtility.HtmlDecode("<h1>Welcome, <script>alert('xss')</script>!</h1>");
            var actual = WebUtility.HtmlDecode(contentResult?.Content ?? "");
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(contentResult?.ContentType, Is.EqualTo("text/html"));
        }

        #endregion
    }
    public static class MockDbSetHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.As<IEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return mockSet;
        }
    }
}