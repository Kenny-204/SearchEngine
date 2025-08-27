using dotenv.net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SearchEngine.API.Core;
using SearchEngine.Enpoints;
using SearchEngine.Query.Algorithms;
using SearchEngine.Query.Services;
using SearchEngine.Services;

// Only load .env file in development
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
{
    DotEnv.Load();
}

var envVars = Environment.GetEnvironmentVariables()
    .Cast<System.Collections.DictionaryEntry>()
    .ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.ToString());

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
                "https://metaseek-admin.netlify.app",
                "https://metaseek-client.netlify.app",
                "http://localhost:5174",
                "http://localhost:8080",
                "http://localhost:4200"
                // Add your Render URL here when deployed
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Cors - Use more restrictive policy in production
builder.Services.AddCors(options =>
{
  options.AddPolicy(
    "AllowAll",
    builder =>
    {
      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }
  );
});

var app = builder.Build();

app.UseCors("AllowAll");

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


// if (app.Environment.IsDevelopment())


app.UseSwagger();
app.UseSwaggerUI();
app.MapScalarApiReference();
app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to MetaSeek API");

/// <summary>
/// Returns autocomplete suggestions for a given prefix.
/// </summary>
/// <param name="prefix">The text prefix to search for.</param>
/// <param name="autoSuggest">The auto suggestion service.</param>
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
  .WithName("GetAutoSuggest")
  .WithOpenApi();

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