using System;
using AppProject.Core.Services.General;
using AppProject.Core.Services.Inventory;
using Hangfire;

namespace AppProject.Core.API.Bootstraps;

public static class JobsBootstrap
{
    public static void RegisterRecurringJobs()
    {
        // Register your recurring jobs here
        /*RecurringJob.AddOrUpdate<YourJobName>(
            recurringJobId: nameof(YourJobName),
            job => job.ExecuteAsync(CancellationToken.None),
            cronExpression: Cron.Daily,
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            });*/

#if DEBUG
        RecurringJob.AddOrUpdate<SampleJob>(
            recurringJobId: nameof(SampleJob),
            job => job.ExecuteAsync(CancellationToken.None),
            cronExpression: Cron.Daily,
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            });
#endif

        RecurringJob.AddOrUpdate<InventoryLowStockJob>(
            recurringJobId: nameof(InventoryLowStockJob),
            job => job.ExecuteAsync(CancellationToken.None),
            cronExpression: Cron.Daily,
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            });
    }
}
