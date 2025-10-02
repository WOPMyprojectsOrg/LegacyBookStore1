using LegacyBookStore.Data;
using LegacyBookStore.Models;
using LegacyBookStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace LegacyBookStore.Controllers
{
    [Route("api/[controller]")]
    public class BooksController(
        IBookRepository _bookRepository) : Controller
    {
        [HttpGet]
        public string GetBooks()
        {
            var books = _bookRepository.GetAll();
            return JsonSerializer.Serialize(books);
        }

        [HttpGet("{id}")]
        public string GetBook(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
                return JsonSerializer.Serialize(new { error = "Book not found" });

            return JsonSerializer.Serialize(book);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (string.IsNullOrWhiteSpace(book?.Title))
            {
                return BadRequest("Title is required");
            }

            _bookRepository.Create(book);

            return Content("Book created", "text/plain");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            _bookRepository.DeleteById(id);

            return Ok("Deleted");
        }
    }
}
