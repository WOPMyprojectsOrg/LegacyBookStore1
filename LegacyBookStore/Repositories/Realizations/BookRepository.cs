using LegacyBookStore.Data;
using LegacyBookStore.Models;
using LegacyBookStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LegacyBookStore.Repositories.Realizations
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;

        public BookRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<Book> Create(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return book;
        } 
        public async Task<bool> DeleteById(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) { 
                return false;
            }
            _db.Books.Remove(book);
            return true;
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _db.Books.FindAsync(id);
        }

        public async Task<List<Book>> GetAll()
        {
            return await _db.Books.ToListAsync();
        }
    }
}
