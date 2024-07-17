namespace SoftDeleteScheduler;

public interface IDatabase
{
    string[] GetTables();
    bool ColumnExists(string tableName, string columnName);
    void ExecuteQuery(string sql, object? parameters = null);
    IEnumerable<T> Query<T>(string sql, object? parameters = null);
}
