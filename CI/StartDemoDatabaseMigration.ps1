# Kicks off the CopyIntegrationDbToDemo job.
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to MPTest02 (optional)
#   -DBUser user                 The SQLServer user to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_USER)
#   -DBPassword password         The SQLServer password to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_PASSWORD)

Param (
  [string]$DBServer = "216.68.184.202", # default to external IP for MPTest02
  [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
  [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value # Default to environment variable
)

$connectionString = "Server=$DBServer;uid=$DBUser;pwd=$DBPassword;Database=master;Integrated Security=False;";

$connection = New-Object System.Data.SqlClient.SqlConnection;
$connection.ConnectionString = $connectionString;
$connection.Open();

$sql = @"
USE [msdb];
EXEC dbo.sp_start_job N'CopyIntegrationDbToDemo';
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$sql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning sp_start_job CopyIntegrationDbToDemo on server $DBServer"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Job failed: " + $_.Exception.Message;
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished sp_start_job CopyIntegrationDbToDemo on server $DBServer"

echo "Status: $exitMessage"
exit $exitCode
