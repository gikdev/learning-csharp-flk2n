using AutoMapper;
using DotNetApi.Data;
using DotNetApi.Dtos;
using DotNetApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers;

[ApiController]
[Route("users-ef")]
public class UserEfController(DataContextEf ef, IMapper mapper, IUserRepo userRepo) : ControllerBase {
  private readonly DataContextEf _ef = ef;
  private readonly IMapper _mp = mapper;
  private readonly IUserRepo _userRepo = userRepo;

  [HttpGet]
  public IEnumerable<User> GetUsers() {
    var users = _ef.Users.ToList();

    return users;
  }

  [HttpGet("{userId:int}")]
  public IActionResult GetSingleUser(int userId) {
    var user = _ef.Users.Where(u => u.UserId == userId).FirstOrDefault();

    if (user is not null) return Ok(user);

    return Problem(
      detail: $"User wasn't found #{userId}",
      statusCode: StatusCodes.Status404NotFound
    );
  }

  [HttpPost]
  public IActionResult AddUser(UserToAddDto userDto) {
    var newUser = _mp.Map<User>(userDto);

    _userRepo.AddEntity(newUser);

    var wasSuccessful = _userRepo.SaveChanges();

    if (wasSuccessful == false) {
      return Problem(
        detail: $"Failed to create user",
        statusCode: StatusCodes.Status404NotFound
      );
    }

    return Ok(new { Created = true });
  }

  [HttpPut]
  public IActionResult EditUser(User user) {
    var existingUser = _ef.Users.FirstOrDefault(u => u.UserId == user.UserId);

    if (existingUser is null) {
      return Problem(
        detail: $"User wasn't found #{user.UserId}",
        statusCode: StatusCodes.Status404NotFound
      );
    }

    existingUser.FirstName = user.FirstName;
    existingUser.LastName = user.LastName;
    existingUser.Email = user.Email;
    existingUser.Gender = user.Gender;
    existingUser.Active = user.Active;

    var wasSuccessful = _userRepo.SaveChanges();

    if (wasSuccessful == false) {
      return Problem(
        detail: $"Failed to update user #{user.UserId}",
        statusCode: StatusCodes.Status404NotFound
      );
    }

    return Ok(new { Updated = true });
  }


  [HttpDelete("{userId:int}")]
  public IActionResult DeleteUser(int userId) {
    var existingUser = _ef.Users.FirstOrDefault(u => u.UserId == userId);

    if (existingUser is null) {
      return Problem(
        detail: $"User wasn't found #{userId}",
        statusCode: StatusCodes.Status404NotFound
      );
    }

    _ef.Users.Remove(existingUser);

    var wasSuccessful = _userRepo.SaveChanges();

    if (wasSuccessful == false) {
      return Problem(
        detail: $"Failed to delete user #{userId}",
        statusCode: StatusCodes.Status404NotFound
      );
    }

    return Ok(new { Deleted = true });
  }
}
