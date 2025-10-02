using LegacyBookStore.Repositories.Interfaces;
using LegacyBookStore.Repositories.Realizations;

namespace LegacyBookStore.Extensions
{
    public static class DependencyInjection
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
        }
    }
}
