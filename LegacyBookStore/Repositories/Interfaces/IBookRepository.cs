using LegacyBookStore.Models;

namespace LegacyBookStore.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> Create(Book book);
        Task<bool> DeleteById(int id);
        Task<Book?> GetBookById(int id);
        Task<List<Book>> GetAll();

    }
}
