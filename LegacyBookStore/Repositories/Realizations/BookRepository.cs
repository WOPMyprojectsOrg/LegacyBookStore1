using LegacyBookStore.Data;
using LegacyBookStore.Models;
using LegacyBookStore.Repositories.Interfaces;

namespace LegacyBookStore.Repositories.Realizations
{
    public class BookRepository(
        AppDbContext _db): IBookRepository
    {
        public Book Create(Book entity)
        {
            var entityentry = _db.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public Book Delete(Book entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
            return entity;   
        }

        public Book DeleteById(int id)
        {
            var book = _db.Books.FirstOrDefault(entity => entity.Id == id);

            if (book == null)
            {
                return null;
            }
            return Delete(book);
        }

        public ICollection<Book> GetAll()
        {
            return _db.Books.ToList();
        }

        public Book GetById(int id)
        {
            return _db.Books.First(entity => entity.Id == id);
        }

        public Book Update(Book entity)
        {
            _db.Books.Update(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
