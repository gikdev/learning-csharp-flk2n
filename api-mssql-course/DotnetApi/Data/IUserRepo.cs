namespace DotNetApi.Data;

public interface IUserRepo {
  public bool SaveChanges();
  public void AddEntity<T>(T entityToAdd);
  public void RemoveEntity<T>(T entityToRemove);
}
