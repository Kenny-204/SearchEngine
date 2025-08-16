using dotenv.net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SearchEngine.API.Core;
using SearchEngine.Enpoints;
using SearchEngine.Query.Algorithms;
using SearchEngine.Query.Services;
using SearchEngine.Services;

DotEnv.Load();
var envVars = DotEnv.Read();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongoConnectionString = envVars.ContainsKey("MONGODB_CONNECTION_STRING") 
    ? envVars["MONGODB_CONNECTION_STRING"] 
    : "mongodb://localhost:27017/searchengine";
builder.Services.AddSingleton(new MongoDbContext(mongoConnectionString));
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>(); // Enqueue documents for processing the indexing processes
builder.Services.AddSingleton<Indexer>();
builder.Services.AddHostedService<BackgroundWorker>(); // Register background worker for processing and indexing
builder.Services.AddSingleton<AutoSuggestion>();
builder.Services.AddSingleton<DocMatcher>();
builder.Services.AddSingleton<QueryParser>(QueryParserFactory.CreateDefault());

// Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document API", Version = "v1" });
});

// Cloudinary
var cloudinaryCloudName = envVars.ContainsKey("CLOUDINARY_CLOUD_NAME") ? envVars["CLOUDINARY_CLOUD_NAME"] : "demo";
var cloudinaryApiKey = envVars.ContainsKey("CLOUDINARY_API_KEY") ? envVars["CLOUDINARY_API_KEY"] : "demo";
var cloudinaryApiSecret = envVars.ContainsKey("CLOUDINARY_API_SECRET") ? envVars["CLOUDINARY_API_SECRET"] : "demo";

builder.Services.Configure<CloudinarySettings>(options =>
{
  options.CloudName = cloudinaryCloudName;
  options.ApiKey = cloudinaryApiKey;
  options.ApiSecret = cloudinaryApiSecret;
});
builder.Services.AddSingleton<CloudinaryService>();

// Redis
var redisHost = envVars.ContainsKey("REDIS_HOST") ? envVars["REDIS_HOST"] : "localhost";
var redisPort = envVars.ContainsKey("REDIS_PORT") ? int.Parse(envVars["REDIS_PORT"]) : 6379;
var redisPassword = envVars.ContainsKey("REDIS_PASSWORD") ? envVars["REDIS_PASSWORD"] : "";

builder.Services.Configure<RedisCacheSettings>(options =>
{
  options.Host = redisHost;
  options.Port = redisPort;
  options.Password = redisPassword;
});
builder.Services.AddSingleton<RedisCacheService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins(
                "http://localhost:3000",
                "http://localhost:3001", 
                "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:8080",
                "http://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


// AntiForgery
// builder.Services.AddAntiforgery();

var app = builder.Build();

// Apply CORS first, before other middleware
app.UseCors("AllowFrontend");

// middlewares
// app.UseAntiforgery(); ///[BLOCKING FRONTEND ACCESS]

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

// var summaries = new[]
// {
//   "Freezing",
//   "Bracing",
//   "Chilly",
//   "Cool",
//   "Mild",
//   "Warm",
//   "Balm" }

// app.MapGet(
//     "/weatherforecast",
//     () =>
//     {
//       var forecast = Enumerable
//         .Range(1, 5)
//         .Select(index => new WeatherForecast(
//           DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//           Random.Shared.Next(-20, 55),
//           summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//       return forecast;
//     }
//   )
//   .WithName("GetWeatherForecast")
//   .WithOpenApi();

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
