﻿using Serilog;

namespace SoftDeleteScheduler;

public class Cleaning
{
    private readonly IDatabase _database;

    public Cleaning(IDatabase database)
    {
        _database = database;
    }

    public void Start()
    {
        Log.Information("starting cleaning database");

        var tables = _database.GetTables();
        Log.Information("found {table} tables in database", tables.Length);

        foreach (var table in tables)
        {
            Log.Information("starting cleaning table {table}", table);
            Progress(table);
        }
    }

    private void Progress(string tableName)
    {
        var columnName = "DeletedAt";
        if (_database.ColumnExists(tableName, columnName))
            _database.ExecuteQuery($"DELETE FROM {tableName} WHERE {columnName} IS NOT NULL");
    }
}