using Microsoft.AspNetCore.Identity;

namespace DotNetApi.Data;

public class UserRepo(DataContextEf ef) : IUserRepo {
  private readonly DataContextEf _ef = ef;

  public bool SaveChanges() => _ef.SaveChanges() > 0;

  public void AddEntity<T>(T entityToAdd) {
    if (entityToAdd is null) return;
    _ef.Add(entityToAdd);
  }

  public void RemoveEntity<T>(T entityToRemove) {
    if (entityToRemove is null) return;
    _ef.Remove(entityToRemove);
  }
}
