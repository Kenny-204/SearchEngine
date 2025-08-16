using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace SearchEngine.Services;

public class RedisCacheSettings
{
  public required string Host { get; set; }
  public required int Port { get; set; }
  public required string Password { get; set; }
}

public class RedisCacheService
{
  private readonly IDatabase _cache;
  private readonly RedisCacheSettings _redisSettings;

  public RedisCacheService(IOptions<RedisCacheSettings> options, ILogger<RedisCacheService> logger)
  {
    _redisSettings = options.Value;
    if (
      string.IsNullOrWhiteSpace(_redisSettings.Host)
      || string.IsNullOrWhiteSpace(_redisSettings.Password)
    )
    {
      throw new ArgumentException("Redis settings are missing or invalid.");
    }

    var muxer = ConnectionMultiplexer.Connect(
      new ConfigurationOptions
      {
        EndPoints = { { _redisSettings.Host, _redisSettings.Port } },
        User = "default",
        Password = _redisSettings.Password,
      }
    );
    _cache = muxer.GetDatabase();
    try
    {
      var pong = _cache.Ping();
      logger.LogInformation(
        $"✅ Redis Cloud connection successful! Ping: {pong.TotalMilliseconds} ms"
      );
      // var server = muxer.GetServer("redis-14490.c244.us-east-1-2.ec2.redns.redis-cloud.com:14490");
      // logger.LogInformation(
      //   "Redis server version: " + server.Info("server").SelectMany(g => g).First().Value
      // );
    }
    catch (RedisConnectionException ex)
    {
      logger.LogError($"❌ Redis Cloud connection failed: {ex.Message}");
    }
    catch (Exception e)
    {
      logger.LogError($"Redis Error: {e.Message}");
    }
  }

  public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
  {
    var json = JsonSerializer.Serialize(value);
    await _cache.StringSetAsync(key, json, expiry);
  }

  public async Task<T?> GetAsync<T>(string key)
  {
    var json = await _cache.StringGetAsync(key);
    return json.HasValue ? JsonSerializer.Deserialize<T>(json!) : default;
  }
}
