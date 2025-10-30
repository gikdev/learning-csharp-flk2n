using Api.Auth;
using Api.Mapping;
using App.Services;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

[ApiController]
public class RatingsController(IRatingService ratingService) : ControllerBase {
  [Authorize]
  [EndpointSummary("Rate a movie")]
  [HttpPut(ApiEndpoints.V1.Movies.Rate)]
  public async Task<IActionResult> RateMovie(
    [FromRoute] Guid id,
    [FromBody] RateMovieReq req,
    CancellationToken token
  ) {
    var userId = HttpContext.GetUserId();
    if (!userId.HasValue) throw new Exception("UserId is null!");
    var result = await ratingService.RateMovieAsync(id, req.Rating, userId.Value, token);
    return result ? Ok() : NotFound();
  }

  [Authorize]
  [EndpointSummary("Delete a rating")]
  [HttpDelete(ApiEndpoints.V1.Movies.DeleteRating)]
  public async Task<IActionResult> DeleteRating(
    [FromRoute] Guid id,
    CancellationToken token
  ) {
    var userId = HttpContext.GetUserId();
    if (!userId.HasValue) throw new Exception("UserId is null!");

    var result = await ratingService.DeleteRatingAsync(id, userId.Value, token);

    return result ? Ok() : NotFound();
  }

  [Authorize]
  [EndpointSummary("Get user's ratings")]
  [HttpGet(ApiEndpoints.V1.Ratings.GetUserRatings)]
  public async Task<ActionResult<IEnumerable<MovieRatingRes>>> GetUserRatings(CancellationToken token) {
    var userId = HttpContext.GetUserId();
    if (!userId.HasValue) throw new Exception("UserId is null!");

    var ratings = await ratingService.GetRatingsForUserAsync(userId.Value, token);

    var response = ratings.MapToResponse();

    return Ok(response);
  }
}
