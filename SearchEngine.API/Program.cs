using dotenv.net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SearchEngine.API.Core;
using SearchEngine.Enpoints;
using SearchEngine.Services;

DotEnv.Load();
var envVars = DotEnv.Read();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(new MongoDbContext(envVars["MONGODB_CONNECTION_STRING"]));
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>(); // Enqueue documents for processing the indexing processes
builder.Services.AddSingleton<Indexer>();
builder.Services.AddHostedService<BackgroundWorker>(); // Register background worker for processing and indexing
builder.Services.AddSingleton<AutoSuggestion>();

// Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document API", Version = "v1" });
});

// Cloudinary
builder.Services.Configure<CloudinarySettings>(options =>
{
  options.CloudName = envVars["CLOUDINARY_CLOUD_NAME"];
  options.ApiKey = envVars["CLOUDINARY_API_KEY"];
  options.ApiSecret = envVars["CLOUDINARY_API_SECRET"];
});
builder.Services.AddSingleton<CloudinaryService>();

// Redis
builder.Services.Configure<RedisCacheSettings>(options =>
{
  options.Host = envVars["REDIS_HOST"];
  options.Port = int.Parse(envVars["REDIS_PORT"]);
  options.Password = envVars["REDIS_PASSWORD"];
});
builder.Services.AddSingleton<RedisCacheService>();

// AntiForgery
builder.Services.AddAntiforgery();

var app = builder.Build();

// middlewares
app.UseAntiforgery();

app.UseExceptionHandler(appError =>
{
  appError.Run(async context =>
  {
    context.Response.StatusCode = 500;
    context.Response.ContentType = "application/json";

    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
    if (contextFeature is not null)
    {
      // Console.WriteLine($"Error: {contextFeature.Error}");

      await context.Response.WriteAsJsonAsync(
        new { StatusCode = context.Response.StatusCode, Message = "Internal Server Error" }
      );
    }
  });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var summaries = new[]
{
  "Freezing",
  "Bracing",
  "Chilly",
  "Cool",
  "Mild",
  "Warm",
  "Balmy",
  "Hot",
  "Sweltering",
  "Scorching",
};

app.MapGet(
    "/weatherforecast",
    () =>
    {
      var forecast = Enumerable
        .Range(1, 5)
        .Select(index => new WeatherForecast(
          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
          Random.Shared.Next(-20, 55),
          summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
      return forecast;
    }
  )
  .WithName("GetWeatherForecast")
  .WithOpenApi();

/// <summary>
/// Returns autocomplete suggestions for a given prefix.
/// </summary>
/// <param name="prefix">The text prefix to search for.</param>
/// <param name="cache">The Redis cache service.</param>
/// <returns>A list of matching terms.</returns>
app.MapGet(
    "/autosuggest",
    async (string prefix, AutoSuggestion autoSuggest) =>
    {
      var results = await autoSuggest.SuggestAsync(prefix);

      return Results.Ok(results ?? []);
    }
  )
  .Produces<List<string>>(StatusCodes.Status200OK)
  .WithName("GetAutoSuggest") // Swagger operationId
  .WithOpenApi(); // Ensures it appears in Swagger

app.MapGet(
    "/next-batch",
    (IBackgroundTaskQueue taskQueue) =>
    {
      return Results.Ok(taskQueue.NextBatchTime);
    }
  )
  .Produces<DateTimeOffset>(StatusCodes.Status200OK)
  .WithName("GetNextIndexBatch")
  .WithOpenApi();

app.MapDocumentEndpoint();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
