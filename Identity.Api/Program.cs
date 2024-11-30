using Identity.Api.Modal;

var builder = WebApplication.CreateBuilder(args);
var jwtSection = builder.Configuration.GetSection("JwtSettings");

// Add services to the container.
builder.Services.Configure<JWTSettings>(jwtSection);
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
