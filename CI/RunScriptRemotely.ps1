# Executes a local powershell script on a remote host.
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to mp-int-db (optional)
#   -ScriptPath Path On Local System   The directory on the local server where the powershell script resides

Param (
  [string]$DBServer = "216.68.184.202", # default to public IP of MPTEST02
  [string]$ScriptPath = $(throw "-ScriptPath (Path to powershell script to run) is required.")
)

$exitCode = 0;
$exitMessage = "Success";

$Password = $Env:MPTEST02_PASS;
$Pass = ConvertTo-SecureString -AsPlainText $Password -Force

$Cred = New-Object System.Management.Automation.PSCredential -ArgumentList 'Administrator',$Pass;

try
{
	echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Running powershell script $ScriptPath on server $DBServer";
	$output = Invoke-Command -ComputerName $DBServer -FilePath $ScriptPath -Port 5986 -Credential $Cred;
}
catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Copy failed: " + $_.Exception.Message;
}

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished running $ScriptPath on server $DBServer"

echo "Status: $exitMessage"
exit $exitCode