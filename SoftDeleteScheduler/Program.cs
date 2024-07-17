using DotNetEnv;
using Quartz;
using Quartz.Impl;
using Serilog;
using SoftDeleteScheduler.Application;
using SoftDeleteScheduler.Jobs;

Env.Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var cronJob = Environment.GetEnvironmentVariable("CRON_JOB");

if (!string.IsNullOrEmpty(cronJob))
{
    var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
    await scheduler.Start();

    var job = JobBuilder.Create<CronJob>()
        .WithIdentity("CronJob", "group1")
        .Build();

    var trigger = TriggerBuilder.Create()
        .WithIdentity("CronTrigger", "group1")
        .WithCronSchedule(cronJob)
        .Build();

    await scheduler.ScheduleJob(job, trigger);

    while (true) await Task.Delay(1000);
}

var softDelete = new SoftDelete();
softDelete.Start();