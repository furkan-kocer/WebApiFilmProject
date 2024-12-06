using FilmProject.DataAccess.CollectionRepositories.FilmCollection;
using FilmProject.DataAccess;
using FilmProject.DataAccess.CollectionRepositories.UserCollection;
using FilmProject.Services.Businesses.ExternalApi;
using FilmProject.Contracts.Abstractions.Businesses.FilmServiceAbs;
using FilmProject.Services.Businesses.FilmService;
using FilmProject.Contracts.Abstractions.Businesses.UserServiceAbs;
using FilmProject.Services.Businesses.UserService;

namespace FilmProject.Api.Extensions
{
    public static class DependencyInjectionServiceExtension
    {
        public static IServiceCollection RegisterDependencyService(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddSingleton<MongoDBService>();
            services.AddHttpClient<ExternalApiService>();
            services.AddScoped<IFilmCollectionRepository, FilmCollectionRepository>();
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<IUserCollectionRepository, UserCollectionRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
