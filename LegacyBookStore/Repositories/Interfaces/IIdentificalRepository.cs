namespace LegacyBookStore.Repositories.Interfaces
{
    public interface IIdentificalRepository<T, K> : IRepository<T> where T : class
    {
        T GetById(K id);
        T DeleteById(K id);
    }
}
