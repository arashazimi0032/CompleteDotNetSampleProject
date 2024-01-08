using Microsoft.Extensions.Logging;
using Quartz;

namespace infrastructure.BackgroundJobs.Jobs;

[DisallowConcurrentExecution]
public class LoggingBackgroundJob : IJob
{
    private readonly ILogger<LoggingBackgroundJob> _logger;

    public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("This is LoggingBackgroundJob: Current Time: {CurrentTime}", DateTime.Now);
        return Task.CompletedTask;
    }
}