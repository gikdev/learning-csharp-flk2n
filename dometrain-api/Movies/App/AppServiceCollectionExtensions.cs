using App.Db;
using App.Repos;
using App.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class AppServiceCollectionExtensions {
  public static IServiceCollection AddApp(this IServiceCollection services) {
    services.AddSingleton<IRatingRepo, RatingRepo>();
    services.AddSingleton<IRatingService, RatingService>();
    services.AddSingleton<IMovieRepo, MovieRepo>();
    services.AddSingleton<IMovieService, MovieService>();
    services.AddValidatorsFromAssemblyContaining<IAppMarker>(ServiceLifetime.Singleton);
    return services;
  }

  public static IServiceCollection AddDb(this IServiceCollection services, string connStr) {
    services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connStr));
    services.AddSingleton<DbInitializer>();
    return services;
  }
}
