using System.Data;
using Dapper;
using DotNetApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DotNetApi.Data;

public class DataContextEf(IConfiguration config) : DbContext {
  private readonly IConfiguration _config = config;

  private string? ConnStr => _config.GetConnectionString("DefaultConnection");

  public virtual DbSet<User> Users { get; set; }
  public virtual DbSet<UserSalary> UserSalary { get; set; }
  public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

  protected override void OnModelCreating(ModelBuilder mb) {
    mb.HasDefaultSchema("TutorialAppSchema");

    mb.Entity<UserSalary>().HasKey(u => u.UserId);
    mb.Entity<UserJobInfo>().HasKey(u => u.UserId);
    mb.Entity<User>()
      .ToTable("Users", "TutorialAppSchema")
      .HasKey(u => u.UserId);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder ob) {
    if (ob.IsConfigured) return;

    ob.UseSqlServer(
      ConnStr,
      ob => ob.EnableRetryOnFailure()
    );
  }

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

