using System.Data;
using Npgsql;

namespace App.Db;

public interface IDbConnectionFactory {
  Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class NpgsqlConnectionFactory(string connStr) : IDbConnectionFactory {
  private readonly string _connStr = connStr;
  public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default) {
    var conn = new NpgsqlConnection(_connStr);
    await conn.OpenAsync(token);
    return conn;
  }
}
