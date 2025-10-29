using Contracts.Responses;
using FluentValidation;

namespace Api.Mapping;

public class ValidationMappingMiddleware(RequestDelegate next) {
  private readonly RequestDelegate _next = next;

  public async Task InvokeAsync(HttpContext ctx) {
    try {
      await _next(ctx);
    }
    catch (ValidationException ex) {
      ctx.Response.StatusCode = StatusCodes.Status400BadRequest;

      var errors = ex.Errors.Select(e => new ValidationRes {
        PropertyName = e.PropertyName,
        Message = e.ErrorMessage,
      });

      var validationFailureRes = new ValidationFailureRes {
        Errors = errors
      };

      await ctx.Response.WriteAsJsonAsync(validationFailureRes);
    }
  }
}
