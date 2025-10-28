using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotNetApi.Data;

public class DataContextDapper(IConfiguration config) {
  private readonly IConfiguration _config = config;

  private string? ConnStr => _config.GetConnectionString("DefaultConnection");

  private IDbConnection Connect() {
    IDbConnection dbConn = new SqlConnection(ConnStr);
    return dbConn;
  }

  public IEnumerable<T> LoadData<T>(string sql) {
    var dbConn = Connect();
    var result = dbConn.Query<T>(sql);
    return result;
  }

  public T LoadSingleData<T>(string sql) {
    var dbConn = Connect();
    var result = dbConn.QuerySingle<T>(sql);
    return result;
  }

  public bool ExecuteSql(string sql) {
    var dbConn = Connect();
    var affectedCount = dbConn.Execute(sql);
    return affectedCount > 0;
  }

  public int ExecuteSqlWithRowCount(string sql) {
    var dbConn = Connect();
    var affectedCount = dbConn.Execute(sql);
    return affectedCount;
  }
}
