# Executes database restore of the MinistryPlatform database
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to mp-int-db (optional)
#   -DBName databaseName         The database to backup (optional, defaults to MinistryPlatform)
#   -BackupFile path_on_server   The backup file on the DBServer (required)
#   -DBUser user                 The SQLServer user to login to the DBServer (optional, defaults to environment variable MP_TARGET_DB_USER)
#   -DBPassword password         The SQLServer password to login to the DBServer (optional, defaults to environment variable MP_TARGET_DB_PASSWORD)

Param (
  [string]$DBServer = "mp-int-db.cloudapp.net", # default to mp-int-db
  [string]$DBName = "MinistryPlatform", # default to MinistryPlatform
  [string]$BackupFile = $(throw "-BackupFile (backup file on the DBServer) is required."),
  [string]$DBUser = $(Get-ChildItem Env:MP_TARGET_DB_USER).Value, # Default to environment variable
  [string]$DBPassword = $(Get-ChildItem Env:MP_TARGET_DB_PASSWORD).Value # Default to environment variable
)

$connectionString = "Server=$DBServer;uid=$DBUser;pwd=$DBPassword;Database=master;Integrated Security=False;";

$connection = New-Object System.Data.SqlClient.SqlConnection;
$connection.ConnectionString = $connectionString;
$connection.Open();

# Determine the current log and data file locations, so we can relocate from the backup.
# This is needed because the servers are not setup with identical drives and paths.
$sql = @"
SELECT type, name, physical_name
FROM sys.master_files
WHERE [database_id] = DB_ID('$DBName')
ORDER BY type, name;
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$sql";

$reader = $command.ExecuteReader();

$table = New-Object System.Data.DataTable;
$table.Load($reader);

$dataFile = $table | Where-Object {$_.type -eq 0};
$logFile = $table | Where-Object {$_.type -eq 1};

$dataFileName = $dataFile.name;
$dataFilePhysicalName = $dataFile.physical_name;

$logFileName = $logFile.name;
$logFilePhysicalName = $logFile.physical_name;

# Restore the database - need to take it offline, restore, then bring back online
$restoreSql = @"
USE [master];

ALTER DATABASE [$DBName] SET OFFLINE WITH ROLLBACK IMMEDIATE;

RESTORE DATABASE [$DBName]
FROM DISK = N'$backupFile'
WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5,
MOVE N'$logFileName' TO N'$logFilePhysicalName',
MOVE N'$dataFileName' TO N'$dataFilePhysicalName';

ALTER DATABASE [$DBName] SET ONLINE;
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$restoreSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning restore from file $BackupFile on server $DBServer"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Restore failed: " + $_.Exception.Message;
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished restore from file $BackupFile on server $DBServer"

echo "Status: $exitMessage"
exit $exitCode
