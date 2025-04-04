using FilmProject.DataAccess;
using Serilog;
using FluentValidation.AspNetCore;
using FluentValidation;
using FilmProject.Api.JWTConfigure;
using FilmProject.Api.Extensions;
using Microsoft.Extensions.Options;
using FilmProject.Services.Businesses.ExternalApi;
using System.Security.Claims;
using FilmProject.Api.Validations.FluentValidation.FilmValidation;
using FilmProject.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure the mongoDBSettings
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.Configure<ExternalApiSettings>(builder.Configuration.GetSection("ExternalApiSettings"));
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
//TODO
builder.Services.RegisterJWTService(Options.Create<JWTSettings>(jwtSettings));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NoUserRole", policy =>
        policy.RequireAssertion(context =>
            context.User.Identity.IsAuthenticated
            && context.User.FindFirst(ClaimTypes.Role)?.Value != "User"));
});
//Implement ValidationFilter and Api Behavior
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()).
    ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
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
