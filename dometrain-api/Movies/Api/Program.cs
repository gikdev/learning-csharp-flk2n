using System.Text;
using Api;
using Api.Mapping;
using App;
using App.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var jwtKey = config["Jwt:Key"] ?? throw new Exception("JWT KEY IS NULL!");
var jwtIssuer = config["Jwt:Issuer"] ?? throw new Exception("JWT ISSUER IS NULL!");
var jwtAudience = config["Jwt:Audience"] ?? throw new Exception("JWT AUDIENCE IS NULL!");
builder.Services
  .AddAuthentication(o => {
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(o => {
    o.TokenValidationParameters = new TokenValidationParameters {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
      ValidateLifetime = true,
      ValidateIssuer = true,
      ValidIssuer = jwtIssuer,
      ValidateAudience = true,
      ValidAudience = jwtAudience,
    };
  });

builder.Services.AddAuthorizationBuilder()
  .AddPolicy(AuthConstants.AdminUserPolicyName, p => {
    p.RequireClaim(AuthConstants.AdminUserClaimName, "true");
  })
  .AddPolicy(AuthConstants.TrustedMemberPolicyName, p => {
    p.RequireAssertion(c => {
      var hasAdmin = c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" });
      var hasTrustedMember = c.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" });
      return hasAdmin || hasTrustedMember;
    });
  });

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApp();

var connStr = config["Db:ConnectionString"] ?? throw new Exception("DB CONNECTION STRING IS NULL!");
builder.Services.AddDb(connStr);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();
app.MapOpenApi();
app.MapScalarApiReference(o => o.WithClassicLayout());

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
