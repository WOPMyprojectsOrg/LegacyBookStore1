using LegacyBookStore.Data;
using LegacyBookStore.Models;
using LegacyBookStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace LegacyBookStore.Controllers
{
    [Route("api/[controller]")]
    [EnableRateLimiting("BooksPolicy")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
                return NotFound(new { error = "Book not found" });

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (string.IsNullOrWhiteSpace(book?.Title))
            {
                return BadRequest("Title is required");
            }

            await _bookRepository.Create(book);

            return Content("Book created", "text/plain");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookRepository.DeleteById(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok("Deleted");
        }
    }
}
