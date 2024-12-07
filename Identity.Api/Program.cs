using Identity.Domain.Modal;
using Identity.Services;

var builder = WebApplication.CreateBuilder(args);
var jwtSection = builder.Configuration.GetSection("JwtSettings");

// Add services to the container.
builder.Services.Configure<JWTSettings>(jwtSection);
builder.Services.AddScoped<_IdentityService,IdentityService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
