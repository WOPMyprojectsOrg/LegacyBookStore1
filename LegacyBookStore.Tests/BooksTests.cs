using LegacyBookStore.Repositories.Interfaces;
using LegacyBookStore.Controllers;
using Moq;
using NUnit.Framework;
using LegacyBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LegacyBookStore.Validators;

namespace LegacyBookStore.Tests
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBookRepository> _mockRepository;
        private BooksController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IBookRepository>();
            _controller = new BooksController(_mockRepository.Object);
        }
        [TearDown]
        public void Exit()
        {
            _controller.Dispose();
        }

        #region GetBooks

        [Test]
        public void GetBooks_ReturnsOkWithListOfBooks()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = 1, Title = "Test Book", Price = 10 } };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(books);

            // Act
            var result = _controller.GetBooks();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(books));
        }

        #endregion

        #region GetBook

        [Test]
        public void GetBook_ExistingId_ReturnsOkWithBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Found Book", Price = 15 };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(book);

            // Act
            var result = _controller.GetBook(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(book));
        }

        [Test]
        public void GetBook_NonExistingId_ReturnsOkWithErrorObject()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(999)).Returns((Book)null);

            // Act
            var result = _controller.GetBook(999);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var value = okResult?.Value;
            var excepted = new { error = "Book not found" };
            var expectedJson = JsonSerializer.Serialize(excepted);
            var actualJson = JsonSerializer.Serialize(value);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }
        #endregion

        #region CreateBook

        [Test]
        public void CreateBook_NullBook_ReturnsBadRequest()
        {
            // Act
            var result = _controller.CreateBook(null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest?.Value, Is.EqualTo("Title is required"));
        }

        [Test]
        public void CreateBook_EmptyTitle_ReturnsBadRequest()
        {
            // Arrange
            var book = new Book { Title = "", Price = 10 };

            // Act
            var result = _controller.CreateBook(book);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest?.Value, Is.EqualTo("Title is required"));
        }

        [Test]
        public void CreateBook_WhitespaceTitle_ReturnsBadRequest()
        {
            // Arrange
            var book = new Book { Title = "   ", Price = 10 };

            // Act
            var result = _controller.CreateBook(book);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest?.Value, Is.EqualTo("Title is required"));
        }

        [Test]
        public void CreateBook_NegativePrice_ReturnsBadRequest()
        {
            // Arrange
            var book = new Book { Title = "Valid Title", Price = -5 };
            // Act
            var result = _controller.CreateBook(book);
            var validator = new BookValidator();
            var validationResult = validator.Validate(book);
            // Assert
            Assert.That(validationResult.IsValid, Is.False);
        }

        [Test]
        public void CreateBook_ValidBook_CallsRepositoryAndReturnsContent()
        {
            // Arrange
            var book = new Book { Title = "Good Book", Price = 20 };

            // Act
            var result = _controller.CreateBook(book);

            // Assert
            _mockRepository.Verify(repo => repo.Create(book), Times.Once);
            Assert.That(result, Is.InstanceOf<ContentResult>());
            var contentResult = result as ContentResult;
            Assert.That(contentResult?.Content, Is.EqualTo("Book created"));
            Assert.That(contentResult?.ContentType, Is.EqualTo("text/plain"));
        }

        #endregion

        #region DeleteBook

        [Test]
        public void DeleteBook_CallsRepositoryAndReturnsOk()
        {
            // Act
            var result = _controller.DeleteBook(5);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteById(5), Times.Once);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo("Deleted"));
        }

        #endregion
    }
}