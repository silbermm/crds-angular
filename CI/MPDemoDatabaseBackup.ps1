# Executes a full database backup of the MinistryPlatform database
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to MPTest02 (optional)
#   -DBName databaseName         The database to backup (optional, defaults to MinistryPlatform)
#   -BackupPath path_on_server   The directory on the DB server to write the backup file (required)
#   -DBUser user                 The SQLServer user to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_USER)
#   -DBPassword password         The SQLServer password to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_PASSWORD)

Param (
  [string]$DBServer = "MPTEST02", # default to external IP for MPTest02
  [string]$DBName = "MinistryPlatform", # default to MinistryPlatform
  [string]$BackupPath = "E:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Backup",
  [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
  [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value # Default to environment variable
)

$connectionString = "Server=$DBServer;uid=$DBUser;pwd=$DBPassword;Database=master;Integrated Security=False;";

$connection = New-Object System.Data.SqlClient.SqlConnection;
$connection.ConnectionString = $connectionString;
$connection.Open();

$backupDateStamp = Get-Date -format 'yyyyMMdd';
$backupFileName="$BackupPath\$DBName-Backup-$backupDateStamp.trn"
$backupDescription="$DBName - Full Database Backup $backupDateStamp"

$backupSql = @"
USE [master];
BACKUP DATABASE [$DBName]
TO DISK = N'$backupFileName'
WITH COPY_ONLY, NOFORMAT, INIT, NAME = N'$backupDescription', SKIP, NOREWIND, NOUNLOAD, STATS = 10;
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$backupSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning backup to file $backupFileName on server $DBServer"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Backup failed: " + $_.Exception.Message;
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished backup to file $backupFileName on server $DBServer"

echo "Status: $exitMessage"
exit $exitCode
