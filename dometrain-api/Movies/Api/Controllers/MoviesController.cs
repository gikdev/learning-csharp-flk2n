using Api.Mapping;
using App.Models;
using App.Repos;
using Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class MoviesController(IMovieRepo movieRepo) : ControllerBase {
  private readonly IMovieRepo _movieRepo = movieRepo;

  [EndpointSummary("Create a movie")]
  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create([FromBody] CreateMoveReq req) {
    var movie = new Movie {
      Id = Guid.NewGuid(),
      Title = req.Title,
      YearOfRelease = req.YearOfRelease,
      Genres = [.. req.Genres],
    };

    await _movieRepo.CreateAsync(movie);

    return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
  }

  [EndpointSummary("Get a movie")]
  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get([FromRoute] Guid id) {
    var movie = await _movieRepo.GetByIdAsync(id);

    if (movie is null) return NotFound();

    var res = movie.MapToResponse();

    return Ok(res);
  }

  [EndpointSummary("Get all movies")]
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll() {
    var movies = await _movieRepo.GetAllAsync();

    var res = movies.MapToResponse();

    return Ok(res);
  }

  [EndpointSummary("Update a movie")]
  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update(
    [FromRoute] Guid id,
    [FromBody] UpdateMoveReq req
  ) {
    var movie = req.MapToMovie(id);

    var updated = await _movieRepo.UpdateAsync(movie);
    if (updated == false) return NotFound();

    var res = movie.MapToResponse();

    return Ok(res);
  }

  [EndpointSummary("Delete a movie")]
  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute] Guid id) {
    var deleted = await _movieRepo.DeleteByIdAsync(id);
    if (deleted == false) return NotFound();
    return Ok();
  }
}
