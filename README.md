# Soft Delete Scheduler

Soft Delete Scheduler is designed to manage the permanent deletion of "soft deleted" data that has surpassed its retention period. This program supports simultaneous operation with multiple databases, including SQL Server, PostgreSQL, MySQL, Oracle, Firebird and MariaDB. It automates the process of identifying and removing expired data to maintain the cleanliness and efficiency of your databases.

## Database Support

| Database |
| :-------- |
| `MySQL` |
| `SQLServer` |
| `PostgreSQL` |
| `Oracle` |
| `Firebird` |

## Environment

### CRON_JOB
A CRONJOB is a scheduled task in Unix or Linux systems that runs automatically at specified times using the cron service. It's defined in the crontab file with the following format:

### DB_TYPE
Specifies the type of database being used by the application. Common values might include mysql, postgresql, sqlserver, oracle, firebird.

### DB_URI
Defines the Uniform Resource Identifier (URI) for connecting to the database. This URI typically includes the username, password, host, port, and database name.

### DAY_THRESHOLD
Indicates the number of days used as a threshold for data deletion. If the data is older than this threshold, it will be marked for deletion.

### COLUMN_DELETEDAT
Specifies the column name in the database used to track the deletion date of a record. This is often used in soft deletion, where the record is not actually removed from the database but is marked as deleted with a timestamp.