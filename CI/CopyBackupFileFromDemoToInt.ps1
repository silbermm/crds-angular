# Executes database restore of the MinistryPlatform database
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to mp-int-db (optional)
#   -DBName databaseName         The database to backup (optional, defaults to MinistryPlatform)
#   -BackupPath path_on_server   The directory on the local server where the backup file resides (required)
#   -DestinationPath path_name   The directory to send the file to on the remote server (required)

Param (
  [string]$DBServer = "mp-int-db.cloudapp.net", # default to mp-int-db
  [string]$DBName = "MinistryPlatform", # default to MinistryPlatform
  [string]$BackupPath = "/cygdrive/E/Program Files/Microsoft SQL Server/MSSQL11.MSSQLSERVER/MSSQL/Backup",
  [string]$DestinationPath = "/cygdrive/D/SQLServer/Backup"
)

$exitCode = 0;
$exitMessage = "Success";

$backupDateStamp = Get-Date -format 'yyyyMMdd';
$backupFileName="${BackupPath}/${DBName}-Backup-${backupDateStamp}.trn"

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning copy of file $backupFileName to server $DBServer"
try {
  $output = & "C:\Program Files\OpenSSH\bin\scp.exe" -i /home/Administrator/.ssh/id_dsa "$backupFileName" "CRAdmin@${DBServer}:${DestinationPath}" 2> $null
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Copy failed: " + $_.Exception.Message;
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished copy of file $backupFileName to server $DBServer"

echo "Status: $exitMessage"
exit $exitCode
