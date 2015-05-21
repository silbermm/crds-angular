# Executes a full database backup of the MinistryPlatform database
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to MPTest02 (optional)
#   -BackupPath path_on_server   The directory on the DB server to write the backup file (required)
#   -SQLcmd full_path_to_sqlcmd  The full path to sqlcmd.exe (optional)
#   -DBUser user                 The SQLServer user to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_USER)
#   -DBPassword password         The SQLServer password to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_PASSWORD)

Param (
  [string]$DBServer = "216.68.184.202", # default to external IP for MPTest02
  [string]$BackupPath = $(throw "-BackupPath (destination on server for backup files) is required."),
  [string]$SQLcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\sqlcmd.exe",
  [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
  [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value # Default to environment variable
)

$backupDateStamp = Get-Date -format 'yyyyMMdd';
$backupFileName="$BackupPath\MinistryPlatformDemoBackup-$backupDateStamp.trn"
$backupDescription="MinistryPlatformDemo - Full Database Backup $backupDateStamp"

$backupSql = @"
BACKUP DATABASE [MinistryPlatform]
TO DISK = N'$backupFileName'
WITH COPY_ONLY, NOFORMAT, INIT, NAME = N'$backupDescription', SKIP, NOREWIND, NOUNLOAD, STATS = 10;
GO
"@;

$exitCode = 0;
$exitMessage = "Success";

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning backup to file $backupFileName on server $DBServer"
$output = & "$SQLcmd" -S $DBServer -b -I -U $DBUser -P $DBPassword -Q "$backupSql"
if($LASTEXITCODE -ne 0) {
  $exitCode = $LASTEXITCODE;
  $exitMessage = "ERROR - SQLcmd failed: $output";
} elseif(!([string]$output).Contains("BACKUP DATABASE successfully processed")) {
  $exitCode = 1;
  $exitMessage = "ERROR - SQLcmd processed, but backup was not successful: $output";
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished backup to file $backupFileName on server $DBServer"

echo "Status: $exitMessage"
exit $exitCode
