using App.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class AppServiceCollectionExtensions {
  public static IServiceCollection AddApp(this IServiceCollection services) {
    services.AddSingleton<IMovieRepo, MovieRepo>();
    return services;
  }
}
