using Api.Mapping;
using App.Models;
using Api.Auth;
using App.Services;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase {
  private readonly IMovieService _movieService = movieService;

  [Authorize(AuthConstants.TrustedMemberPolicyName)]
  [EndpointSummary("Create a movie")]
  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<ActionResult<MovieRes>> Create([FromBody] CreateMoveReq req, CancellationToken token) {
    var movie = new Movie {
      Id = Guid.NewGuid(),
      Title = req.Title,
      YearOfRelease = req.YearOfRelease,
      Genres = [.. req.Genres],
    };

    await _movieService.CreateAsync(movie, token);

    return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
  }

  [EndpointSummary("Get a movie")]
  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<ActionResult<MovieRes>> Get([FromRoute] string idOrSlug, CancellationToken token) {
    var userId = HttpContext.GetUserId();

    Movie? movie;

    if (Guid.TryParse(idOrSlug, out var id)) {
      movie = await _movieService.GetByIdAsync(id, userId, token);
    }
    else {
      movie = await _movieService.GetBySlugAsync(idOrSlug, userId, token);
    }

    if (movie is null) return NotFound();

    var res = movie.MapToResponse();

    return Ok(res);
  }

  [EndpointSummary("Get all movies")]
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<ActionResult<MoviesRes>> GetAll(CancellationToken token) {
    var userId = HttpContext.GetUserId();

    var movies = await _movieService.GetAllAsync(userId, token);

    var res = movies.MapToResponse();

    return Ok(res);
  }

  [Authorize(AuthConstants.TrustedMemberPolicyName)]
  [EndpointSummary("Update a movie")]
  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<ActionResult<MovieRes>> Update(
    [FromRoute] Guid id,
    [FromBody] UpdateMoveReq req,
    CancellationToken token
  ) {
    var userId = HttpContext.GetUserId();

    var movie = req.MapToMovie(id);

    var updatedMovie = await _movieService.UpdateAsync(movie, userId, token);
    if (updatedMovie == null) return NotFound();

    var res = updatedMovie.MapToResponse();

    return Ok(res);
  }

  [Authorize(AuthConstants.AdminUserPolicyName)]
  [EndpointSummary("Delete a movie")]
  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token) {
    var deleted = await _movieService.DeleteByIdAsync(id, token);
    if (deleted == false) return NotFound();
    return Ok();
  }
}
