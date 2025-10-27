using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// About configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(o => {
  o.WithClassicLayout();
});

if (app.Environment.IsProduction()) {
  app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
