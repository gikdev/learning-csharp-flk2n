using Api.Mapping;
using App.Models;
using App.Repos;
using App.Services;
using Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase {
  private readonly IMovieService _movieService = movieService;

  [EndpointSummary("Create a movie")]
  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create([FromBody] CreateMoveReq req, CancellationToken token) {
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
  public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token) {
    Movie? movie;

    if (Guid.TryParse(idOrSlug, out var id)) {
      movie = await _movieService.GetByIdAsync(id, token);
    }
    else {
      movie = await _movieService.GetBySlugAsync(slug: idOrSlug, token);
    }

    if (movie is null) return NotFound();

    var res = movie.MapToResponse();

    return Ok(res);
  }

  [EndpointSummary("Get all movies")]
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll(CancellationToken token) {
    var movies = await _movieService.GetAllAsync(token);

    var res = movies.MapToResponse();

    return Ok(res);
  }

  [EndpointSummary("Update a movie")]
  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update(
    [FromRoute] Guid id,
    [FromBody] UpdateMoveReq req,
    CancellationToken token
  ) {
    var movie = req.MapToMovie(id);

    var updatedMovie = await _movieService.UpdateAsync(movie, token);
    if (updatedMovie == null) return NotFound();

    var res = updatedMovie.MapToResponse();

    return Ok(res);
  }

  [EndpointSummary("Delete a movie")]
  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token) {
    var deleted = await _movieService.DeleteByIdAsync(id, token);
    if (deleted == false) return NotFound();
    return Ok();
  }
}
