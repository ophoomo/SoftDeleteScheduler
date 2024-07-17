using DotNetEnv;
using Quartz;
using Quartz.Impl;
using Serilog;
using SoftDeleteScheduler;

Env.Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var cronJob = Environment.GetEnvironmentVariable("CRON_JOB");

var dbUri = Environment.GetEnvironmentVariable("DB_URI");
var dbType = Environment.GetEnvironmentVariable("DB_TYPE");

if (!string.IsNullOrEmpty(cronJob))
{
    IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
    await scheduler.Start();

    IJobDetail job = JobBuilder.Create<CronJob>()
        .WithIdentity("CronJob", "group1")
        .Build();

    ITrigger trigger = TriggerBuilder.Create()
        .WithIdentity("CronTrigger", "group1")
        .WithCronSchedule(cronJob)
        .Build();

    await scheduler.ScheduleJob(job, trigger);

    while (true)
    {
        await Task.Delay(1000);
    }
}
else
{
    var context = new Database(dbUri, dbType);
    var cleaning = new Cleaning(context);
    cleaning.Start();
}
