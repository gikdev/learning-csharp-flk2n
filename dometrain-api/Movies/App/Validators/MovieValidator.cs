using App.Models;
using App.Repos;
using FluentValidation;

namespace App.Validators;

public class MovieValidator : AbstractValidator<Movie> {
  private readonly IMovieRepo _movieRepo;
  public MovieValidator(IMovieRepo movieRepo) {
    _movieRepo = movieRepo;

    RuleFor(m => m.Id).NotEmpty();
    RuleFor(m => m.Genres).NotEmpty();
    RuleFor(m => m.Title).NotEmpty();
    RuleFor(m => m.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);
    RuleFor(m => m.Slug)
      .MustAsync(ValidateSlug)
      .WithMessage("This movie already exists in the system.");
  }

  private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken token = default) {
    var existingMovie = await _movieRepo.GetBySlugAsync(slug);

    if (existingMovie is null) return true;

    return existingMovie.Id == movie.Id;
  }
}
