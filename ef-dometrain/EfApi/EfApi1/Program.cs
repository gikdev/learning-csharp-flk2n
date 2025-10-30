using EfApi1.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MoviesContext>();

var app = builder.Build();

// DIRTY HACK! START
if (app.Environment.IsDevelopment()) {
  var scope = app.Services.CreateScope();
  var ctx = scope.ServiceProvider.GetRequiredService<MoviesContext>();
  ctx.Database.EnsureDeleted();
  ctx.Database.EnsureCreated();
}
// DIRTY HACK! END

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();
app.Run();
