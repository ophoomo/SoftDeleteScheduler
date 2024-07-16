using Microsoft.Extensions.Configuration;
using Serilog;

#if DEBUG
dotenv.net.DotEnv.Load();
#endif

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
