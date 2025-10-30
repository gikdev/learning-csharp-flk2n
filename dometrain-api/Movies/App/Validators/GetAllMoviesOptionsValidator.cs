using App.Models;
using FluentValidation;

namespace App.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions> {
  private static readonly string[] AcceptableSortFields = [
    "title",
    "year",
  ];

  public GetAllMoviesOptionsValidator() {
    RuleFor(o => o.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);

    var acceptableSortFields = string.Join(", ", AcceptableSortFields);
    RuleFor(o => o.SortField)
      .Must(f => (
        f is null
          ||
        AcceptableSortFields.Contains(f, StringComparer.OrdinalIgnoreCase)
      ))
      .WithMessage($"You can only sort by these fields {acceptableSortFields}");

    RuleFor(o => o.Page)
      .GreaterThanOrEqualTo(1);

    RuleFor(o => o.PageSize)
      .InclusiveBetween(1, 25)
      .WithMessage("You can get between 1 and 25 movies per page");
  }
}
