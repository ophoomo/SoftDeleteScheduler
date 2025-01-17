﻿namespace SoftDeleteScheduler.Infrastructure;

public interface IDatabase
{
    string[] GetTables();
    bool ColumnExists(string tableName, string columnName);
    void CleanSoftDelete(string tableName, string columnName, int daysThreshold);
    void ExecuteQuery(string sql, object? parameters = null);
    IEnumerable<T> Query<T>(string sql, object? parameters = null);
}