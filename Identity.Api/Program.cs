using Identity.Api.Extensions;
using Identity.Domain.Modal;
using Identity.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var jwtSection = builder.Configuration.GetSection("JwtSettings");
//builder.Services.Configure<ExternalApiSettings>(builder.Configuration.GetSection("ExternalApiSettings"));
builder.Services.Configure<JWTSettings>(jwtSection);
//Logging Configuration
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json", optional: true).Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddScoped<_IdentityService,IdentityService>();
//builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>();
builder.Services.AddControllers();

var app = builder.Build();
app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure proper log flushing on application shutdown
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
