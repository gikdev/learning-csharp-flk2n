using App;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApp();

var app = builder.Build();

app.MapControllers();
app.MapOpenApi();
app.MapScalarApiReference(o => o.WithClassicLayout());
app.UseHttpsRedirection();
app.Run();
