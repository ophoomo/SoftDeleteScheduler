using System.Data;
using System.Data.SqlClient;
using Ardalis.GuardClauses;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace SoftDeleteScheduler;

public class Database : IDatabase
{
    private readonly IDbConnection _connection;

    public Database()
    {
        var connectionDb = Environment.GetEnvironmentVariable("DB_URI");
        Guard.Against.NullOrWhiteSpace(connectionDb, nameof(connectionDb), "Environment variable 'DB_URI' not found.");

        var databaseType = Environment.GetEnvironmentVariable("DB_TYPE");
        Guard.Against.NullOrWhiteSpace(databaseType, nameof(databaseType), "Environment variable 'DB_TYPE' not found.");

        _connection = CreateConnection(databaseType, connectionDb);
    }

    public string[] GetTables()
    {
        var sql = _connection switch
        {
            MySqlConnection => "SHOW TABLES",
            NpgsqlConnection => "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'",
            SqlConnection => "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'",
            OracleConnection => "SELECT table_name FROM user_tables",
            FbConnection =>
                "SELECT RDB$RELATION_NAME FROM RDB$RELATIONS WHERE RDB$VIEW_BLR IS NULL AND RDB$SYSTEM_FLAG = 0",
            _ => throw new NotSupportedException($"The connection type '{_connection.GetType()}' is not supported.")
        };

        _connection.Open();
        try
        {
            var result = _connection.Query<string>(sql);
            return result.AsList().ToArray();
        }
        finally
        {
            _connection.Close();
        }
    }

    public bool ColumnExists(string tableName, string columnName)
    {
        var sql = _connection switch
        {
            MySqlConnection =>
                @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName",
            NpgsqlConnection =>
                @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = @TableName AND column_name = @ColumnName",
            SqlConnection =>
                @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName",
            OracleConnection =>
                @"SELECT COUNT(*) FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = UPPER(@TableName) AND COLUMN_NAME = UPPER(@ColumnName)",
            FbConnection =>
                @"SELECT COUNT(*) FROM RDB$RELATION_FIELDS WHERE RDB$RELATION_NAME = @TableName AND RDB$FIELD_NAME = @ColumnName",
            _ => throw new NotSupportedException($"The connection type '{_connection.GetType()}' is not supported.")
        };

        _connection.Open();
        try
        {
            var parameters = new { TableName = tableName, ColumnName = columnName };
            var columnCount = _connection.ExecuteScalar<int>(sql, parameters);
            return columnCount > 0;
        }
        finally
        {
            _connection.Close();
        }
    }

    public void ExecuteQuery(string sql, object? parameters = null)
    {
        _connection.Open();
        try
        {
            _connection.Execute(sql, parameters);
        }
        finally
        {
            _connection.Close();
        }
    }

    public IEnumerable<T> Query<T>(string sql, object? parameters = null)
    {
        _connection.Open();
        try
        {
            return _connection.Query<T>(sql, parameters);
        }
        finally
        {
            _connection.Close();
        }
    }

    private IDbConnection CreateConnection(string databaseType, string connectionDb)
    {
        return databaseType?.ToLower() switch
        {
            "mysql" => new MySqlConnection(connectionDb),
            "sqlserver" => new SqlConnection(connectionDb),
            "postgresql" => new NpgsqlConnection(connectionDb),
            "oracle" => new OracleConnection(connectionDb),
            "firebird" => new FbConnection(connectionDb),
            _ => throw new NotSupportedException($"The database type '{databaseType}' is not supported.")
        };
    }
}
