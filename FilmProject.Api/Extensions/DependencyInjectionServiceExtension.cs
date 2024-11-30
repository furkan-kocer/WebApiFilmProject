using FilmProject.DataAccess.CollectionRepositories.FilmCollection;
using FilmProject.DataAccess;
using FilmProject.Services.Businesses.FilmService;

namespace FilmProject.Api.Extensions
{
    public static class DependencyInjectionServiceExtension
    {
        public static IServiceCollection RegisterDependencyService(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddSingleton<MongoDBService>();
            services.AddScoped<IFilmCollectionRepository, FilmCollectionRepository>();
            services.AddScoped<IFilmService, FilmService>();
            return services;
        }
    }
}
