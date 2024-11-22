using FilmProject.DataAccess;
using FilmProject.DataAccess.CollectionRepositories.FilmCollection;
using Serilog;
using FilmProject.Services.Businesses.FilmService;
using FilmProject.Services.Extensions;
using FluentValidation.AspNetCore;
using FluentValidation;
using FilmProject.Services.Validations.FluentValidation.FilmValidation;
using FilmProject.Services.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure the mongoDBSettings
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
//Logging Configuration
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json", optional: true).Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddScoped<IFilmCollectionRepository,FilmCollectionRepository>();
builder.Services.AddScoped<IFilmService, FilmService>();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<FilmRequestDtoValidator>();
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()).
    ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure proper log flushing on application shutdown
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
