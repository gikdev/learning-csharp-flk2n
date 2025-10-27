using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IntermediateStuff.Data;

public class DataContextDapper(IConfiguration config) {
  private readonly IConfiguration _config = config;

  private string? ConnStr => _config.GetConnectionString("DefaultConnection");

  public IEnumerable<T> LoadData<T>(string sql) {
    DbConnection dbConn = new SqlConnection(ConnStr);
    var res = dbConn.Query<T>(sql);
    return res;
  }

  public T LoadDataSingle<T>(string sql) {
    DbConnection dbConn = new SqlConnection(ConnStr);
    var res = dbConn.QuerySingle<T>(sql);
    return res;
  }

  public bool ExecuteSql(string sql) {
    DbConnection dbConn = new SqlConnection(ConnStr);
    var res = dbConn.Execute(sql);
    return res > 0;
  }

  public int ExecuteSqlWithRowCount(string sql) {
    DbConnection dbConn = new SqlConnection(ConnStr);
    var res = dbConn.Execute(sql);
    return res;
  }
}
