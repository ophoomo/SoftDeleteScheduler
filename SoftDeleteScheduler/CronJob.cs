using Quartz;

namespace SoftDeleteScheduler;

public class CronJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var dbUri = Environment.GetEnvironmentVariable("DB_URI");
        var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
        var db = new Database(dbUri, dbType);
        var cleaning = new Cleaning(db);
        cleaning.Start();
        return Task.CompletedTask;
    }
}
