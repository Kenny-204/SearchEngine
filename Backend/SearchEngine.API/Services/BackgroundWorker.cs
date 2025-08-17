using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SearchEngine.API.Core;
using DocTerms = (
  MongoDB.Bson.ObjectId DocId,
  System.Collections.Generic.Dictionary<string, int> TermFrequencies
);

namespace SearchEngine.Services;

public interface IBackgroundTaskQueue
{
  void Enqueue(DocTerms workItem);
  DateTimeOffset? NextBatchTime { get; set; }
  Task<DocTerms> DequeueAsync(CancellationToken cancellationToken);
  bool TryDequeue(out DocTerms item);
}

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
  private readonly Channel<DocTerms> _queue;
  public DateTimeOffset? NextBatchTime { get; set; }

  public BackgroundTaskQueue()
  {
    _queue = Channel.CreateUnbounded<DocTerms>();
  }

  public void Enqueue(DocTerms workItem)
  {
    _queue.Writer.TryWrite(workItem);
  }

  public async Task<DocTerms> DequeueAsync(CancellationToken cancellationToken)
  {
    return await _queue.Reader.ReadAsync(cancellationToken);
  }

  public bool TryDequeue(out DocTerms item)
  {
    return _queue.Reader.TryRead(out item);
  }
}

public class BackgroundWorker : BackgroundService
{
  private readonly IBackgroundTaskQueue _taskQueue;
  private readonly ILogger<BackgroundWorker> _logger;
  private readonly Indexer _indexer;

  public BackgroundWorker(
    IBackgroundTaskQueue taskQueue,
    Indexer indexer,
    ILogger<BackgroundWorker> logger
  )
  {
    _taskQueue = taskQueue;
    _logger = logger;
    _indexer = indexer;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Hourly batch background worker started.");

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        _taskQueue.NextBatchTime = DateTimeOffset.Now.AddHours(1);
        // Wait 1 hour before processing
        await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

        _logger.LogInformation("Processing queued tasks at {Time}.", DateTimeOffset.Now);

        // Process all items currently in the queue
        List<DocTerms> docTermsBatch = [];
        while (_taskQueue.TryDequeue(out var workItem))
        {
          docTermsBatch.Add(workItem);
        }
        if (docTermsBatch.Count > 0)
        {
          try
          {
            await _indexer.BatchUpdateGroupedInvertedIndexAsync(docTermsBatch);
          }
          catch (Exception ex)
          {
            _logger.LogError(ex, "Error occurred executing work item.");
          }
          _logger.LogInformation("Finished processing batch at {Time}.", DateTimeOffset.Now);
        }
      }
      catch (TaskCanceledException)
      {
        // Service stopping, break out
        break;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error in hourly batch loop.");
      }
    }

    _logger.LogInformation("Hourly batch background worker stopping.");
  }
}
