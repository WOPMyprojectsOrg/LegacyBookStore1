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
    [ApiController]
    public class BooksController(
        IBookRepository _bookRepository) : Controller
    {
        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
                return Ok(new { error = "Book not found" });

            return Ok(book);
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
