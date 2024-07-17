using DotNetEnv;
using Serilog;
using SoftDeleteScheduler;

Env.Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var context = new Database();
var cleaning = new Cleaning(context);
cleaning.Start();