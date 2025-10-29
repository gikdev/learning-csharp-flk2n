using Api.Mapping;
using App;
using App.Db;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApp();

var connStr = config["Db:ConnectionString"] ?? throw new Exception("DB CONNECTION STRING IS NULL!");
builder.Services.AddDb(connStr);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(o => o.WithClassicLayout());
app.UseHttpsRedirection();
app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
