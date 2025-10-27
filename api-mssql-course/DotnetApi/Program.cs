using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// About configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options => {
  options.AddPolicy("DevCors", corsBuilder => corsBuilder
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
  );

  options.AddPolicy("ProdCors", corsBuilder => corsBuilder
    .WithOrigins("https://myproductionsite.com")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
  );
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(o => {
  o.WithClassicLayout();
});

if (app.Environment.IsProduction()) {
  app.UseCors("ProdCors");
  app.UseHttpsRedirection();
}
else {
  app.UseCors("DevCors");
}

app.MapControllers();

app.Run();
