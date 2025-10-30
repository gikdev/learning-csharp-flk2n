using Api.Mapping;
using App.Models;
using Api.Auth;
using App.Services;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase {
  [Authorize(AuthConstants.TrustedMemberPolicyName)]
  [EndpointSummary("Create a movie")]
  [HttpPost(ApiEndpoints.V1.Movies.Create)]
  public async Task<ActionResult<MovieRes>> Create([FromBody] CreateMoveReq req, CancellationToken token) {
    var movie = new Movie {
      Id = Guid.NewGuid(),
      Title = req.Title,
      YearOfRelease = req.YearOfRelease,
      Genres = [.. req.Genres],
    };

    await movieService.CreateAsync(movie, token);

    return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
  }

  [EndpointSummary("Get a movie")]
  [HttpGet(ApiEndpoints.V1.Movies.Get)]
  public async Task<ActionResult<MovieRes>> Get(
    [FromRoute] string idOrSlug,
    [FromServices] LinkGenerator linkGenerator,
    CancellationToken token
  ) {
    var userId = HttpContext.GetUserId();

    Movie? movie;

    if (Guid.TryParse(idOrSlug, out var id)) {
      movie = await movieService.GetByIdAsync(id, userId, token);
    }
    else {
      movie = await movieService.GetBySlugAsync(idOrSlug, userId, token);
    }

    if (movie is null) return NotFound();

    var res = movie.MapToResponse();

    var movieObj = new { id = movie.Id };

    res.Links.Add(new Link {
      Rel = "self",
      Type = "GET",
      Href = linkGenerator.GetPathByAction(HttpContext, nameof(Get), values: new { idOrSlug = movie.Id }) ?? "",
    });

    return Ok(res);
  }

  [EndpointSummary("Get all movies")]
  [HttpGet(ApiEndpoints.V1.Movies.GetAll)]
  public async Task<ActionResult<MoviesRes>> GetAll(
    [FromQuery] GetAllMoviesReq req,
    CancellationToken token
  ) {
    var userId = HttpContext.GetUserId();
    var options = req.MapToOptions().WithUser(userId);
    var movies = await movieService.GetAllAsync(options, token);
    var moviesCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);

    var res = movies.MapToResponse(req.Page, req.PageSize, moviesCount);

    return Ok(res);
  }

  [Authorize(AuthConstants.TrustedMemberPolicyName)]
  [EndpointSummary("Update a movie")]
  [HttpPut(ApiEndpoints.V1.Movies.Update)]
  public async Task<ActionResult<MovieRes>> Update(
    [FromRoute] Guid id,
    [FromBody] UpdateMoveReq req,
    CancellationToken token
  ) {
    var userId = HttpContext.GetUserId();

    var movie = req.MapToMovie(id);

    var updatedMovie = await movieService.UpdateAsync(movie, userId, token);
    if (updatedMovie == null) return NotFound();

    var res = updatedMovie.MapToResponse();

    return Ok(res);
  }

  [Authorize(AuthConstants.AdminUserPolicyName)]
  [EndpointSummary("Delete a movie")]
  [HttpDelete(ApiEndpoints.V1.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token) {
    var deleted = await movieService.DeleteByIdAsync(id, token);
    if (deleted == false) return NotFound();
    return Ok();
  }
}
