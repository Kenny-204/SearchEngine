using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SearchEngine.Services;

public interface IBackgroundTaskQueue
{
  void Enqueue(Func<CancellationToken, Task> workItem);
  Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
  private readonly Channel<Func<CancellationToken, Task>> _queue;

  public BackgroundTaskQueue()
  {
    _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
  }

  public void Enqueue(Func<CancellationToken, Task> workItem)
  {
    if (workItem == null)
      throw new ArgumentNullException(nameof(workItem));

    _queue.Writer.TryWrite(workItem);
  }

  public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
  {
    return await _queue.Reader.ReadAsync(cancellationToken);
  }
}

public class BackgroundWorker : BackgroundService
{
  private readonly IBackgroundTaskQueue _taskQueue;
  private readonly ILogger<BackgroundWorker> _logger;

  public BackgroundWorker(IBackgroundTaskQueue taskQueue, ILogger<BackgroundWorker> logger)
  {
    _taskQueue = taskQueue;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Queued Hosted Service is starting.");

    while (!stoppingToken.IsCancellationRequested)
    {
      var workItem = await _taskQueue.DequeueAsync(stoppingToken);

      try
      {
        await workItem(stoppingToken);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred executing work item.");
      }
    }

    _logger.LogInformation("Queued Hosted Service is stopping.");
  }
}
