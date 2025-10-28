using DotNetApi.Data;
using DotNetApi.Dtos;
using DotNetApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers;

[ApiController]
[Route("users")]
public class UserController(DataContextDapper dapper) : ControllerBase {
  private readonly DataContextDapper _dapper = dapper;

  [HttpGet]
  public IEnumerable<User> GetUsers() {
    var sql = @"
      SELECT
        [UserId],
        [FirstName],
        [LastName],
        [Email],
        [Gender],
        [Active]
      FROM TutorialAppSchema.Users
    ;";

    var users = _dapper.LoadData<User>(sql);

    return users;
  }

  [HttpGet("{userId:int}")]
  public User GetSingleUser(int userId) {
    var sql = $@"
      SELECT
        [UserId],
        [FirstName],
        [LastName],
        [Email],
        [Gender],
        [Active]
      FROM TutorialAppSchema.Users
      WHERE UserId = {userId}
    ;";

    var user = _dapper.LoadSingleData<User>(sql);

    return user;
  }

  [HttpPost]
  public IActionResult AddUser(UserToAddDto userDto) {
    string sql = @$"
      INSERT INTO TutorialAppSchema.Users (
        [FirstName],
        [LastName],
        [Email],
        [Gender],
        [Active]
      ) VALUES (
        '{userDto.FirstName}',
        '{userDto.LastName}' ,
        '{userDto.Email}'    ,
        '{userDto.Gender}'   ,
         {(userDto.Active ? 1 : 0)}
      )
    ;";

    var wasSuccessful = _dapper.ExecuteSql(sql);

    if (wasSuccessful) return Ok(new { Created = true });

    return Problem(
      detail: "Failed to create the user",
      statusCode: StatusCodes.Status400BadRequest
    );
  }

  [HttpPut]
  public IActionResult EditUser(User user) {
    string sql = @$"
      UPDATE TutorialAppSchema.Users
      SET
        [FirstName] = '{user.FirstName}',
        [LastName]  = '{user.LastName}',
        [Email]     = '{user.Email}',
        [Gender]    = '{user.Gender}',
        [Active]    =  {(user.Active ? 1 : 0)}
      WHERE UserId  =  {user.UserId}
    ;";

    var wasSuccessful = _dapper.ExecuteSql(sql);

    if (wasSuccessful) return Ok(new { Updated = true });

    return Problem(
      detail: "Failed to update user",
      statusCode: StatusCodes.Status500InternalServerError
    );
  }

  [HttpDelete("{userId:int}")]
  public IActionResult DeleteUser(int userId) {
    string sql = @$"
      DELETE FROM TutorialAppSchema.Users
      WHERE UserId = {userId}
    ;";

    var wasSuccessful = _dapper.ExecuteSql(sql);

    if (wasSuccessful) return Ok(new { Deleted = true });

    return Problem(
      detail: "Failed to delete user",
      statusCode: StatusCodes.Status500InternalServerError
    );
  }
}
