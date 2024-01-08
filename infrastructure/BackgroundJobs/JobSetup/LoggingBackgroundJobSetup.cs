using infrastructure.BackgroundJobs.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace infrastructure.BackgroundJobs.JobSetup;

public class LoggingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(LoggingBackgroundJob));
        options
            .AddJob<LoggingBackgroundJob>(JobBuilder => JobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
            {
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                    {
                        schedule.WithIntervalInSeconds(5).RepeatForever();
                    });
            });
    }
}