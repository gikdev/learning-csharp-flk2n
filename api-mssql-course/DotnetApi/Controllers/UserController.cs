using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase {

  public UserController() {

  }

  [HttpGet("/{testValue:required}", Name = "GetUsers")]
  public string[] GetUsers(string testValue) {
    Console.WriteLine(testValue);

    string[] resArr = [
      "test1",
      "test2",
    ];

    return resArr;
  }
}
