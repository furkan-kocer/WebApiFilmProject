using FilmProject.DataAccess;
using Serilog;
using FilmProject.Services.Extensions;
using FluentValidation.AspNetCore;
using FluentValidation;
using FilmProject.Services.Validations.FluentValidation.FilmValidation;
using FilmProject.Services.Filters;
using FilmProject.Api.JWTConfigure;
using FilmProject.Api.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configure the mongoDBSettings
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
//Configure the JwtSettings
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwtSettings = jwtSection.Get<JWTSettings>();
builder.Services.Configure<JWTSettings>(jwtSection);
//Logging Configuration
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json", optional: true).Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
builder.Host.UseSerilog();
builder.Services.RegisterDependencyService();
//FluentValidation Implementation
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<FilmRequestDtoValidator>();
//JWT Service Registration
builder.Services.RegisterJWTService(Options.Create(jwtSettings));
builder.Services.AddAuthorization();
//Implement ValidationFilter and Api Behavior
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()).
    ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ConfigureExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure proper log flushing on application shutdown
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
