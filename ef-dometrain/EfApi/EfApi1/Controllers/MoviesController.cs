using EfApi1.Data;
using EfApi1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfApi1.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController(MoviesContext db) : ControllerBase {
  [HttpPost]
  [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
  public async Task<IActionResult> Create([FromBody] Movie movie) {
    await db.Movies.AddAsync(movie);

    await db.SaveChangesAsync();

    return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll() {
    var result = await db.Movies.ToListAsync();

    return Ok(result);
  }

  [HttpGet("by-year/{year:int}")]
  [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAllByYear([FromRoute] int year) {
    var filteredMovies = await db.Movies.Where(m => m.ReleaseDate.Year == year)
      .Select(m => new MovieTitle { Id = m.Id, Title = m.Title }).ToListAsync();

    return Ok(filteredMovies);
  }

  [HttpGet("{id:int}")]
  [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Get(int id) {
    // var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id);
    // var movie = await db.Movies.SingleOrDefaultAsync(m => m.Id == id);
    var movie = await db.Movies.FindAsync(id);

    return movie == null ? NotFound() : Ok(movie);
  }

  [HttpPut("{id:int}")]
  [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie) {
    var existingMovie = await db.Movies.FindAsync(id);
    if (existingMovie == null) return NotFound();

    existingMovie.Title = movie.Title;
    existingMovie.ReleaseDate = movie.ReleaseDate;
    existingMovie.Synopsis = movie.Synopsis;

    await db.SaveChangesAsync();

    return Ok(existingMovie);
  }

  [HttpDelete("{id:int}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete([FromRoute] int id) {
    var existingMovie = await db.Movies.FindAsync(id);
    if (existingMovie == null) return NotFound();

    db.Movies.Remove(existingMovie);

    await db.SaveChangesAsync();

    return NoContent();
  }
}
