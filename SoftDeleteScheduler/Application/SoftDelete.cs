using SoftDeleteScheduler.Infrastructure;

namespace SoftDeleteScheduler.Application;

public class SoftDelete
{
    public void Start()
    {
        var dbUri = Environment.GetEnvironmentVariable("DB_URI");
        var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
        var db = new Database(dbUri, dbType);
        var cleaning = new Cleaning(db);
        cleaning.Start();
    }
}
