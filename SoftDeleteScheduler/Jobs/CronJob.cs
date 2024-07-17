using Quartz;
using SoftDeleteScheduler.Application;

namespace SoftDeleteScheduler.Jobs;

public class CronJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var softDelete = new SoftDelete();
        softDelete.Start();
        return Task.CompletedTask;
    }
}