namespace LegacyBookStore.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        ICollection<T> GetAll();
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
