using dotenv.net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SearchEngine.Enpoints;
using SearchEngine.Indexing;
using SearchEngine.Parser;
using SearchEngine.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IDocumentIndexedEventHandler, DocumentIndexedEventHandler>(); // Handle Event when document is fished indexing
builder.Services.AddScoped<IIndexService, IndexService>(); // Register IndexService (Add a doc to the indexing queue)
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>(); // Enqueue documents for processing the indexing processes

// builder.Services.AddHostedService<BackgroundService>(); // Register background worker for processing and indexing
// Swagger
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document API", Version = "v1" });
});

// Cloudinary
builder.Services.Configure<CloudinarySettings>(options =>
{
  var envVars = DotEnv.Read();
  options.CloudName = envVars["CLOUDINARY_CLOUD_NAME"];
  options.ApiKey = envVars["CLOUDINARY_API_KEY"];
  options.ApiSecret = envVars["CLOUDINARY_API_SECRET"];
});
builder.Services.AddSingleton<CloudinaryService>();

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

app.MapDocumentEndpoint();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
