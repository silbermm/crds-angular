# Requires Powershell version 4, if this doesn't work you are likely running version 3
# https://www.microsoft.com/en-us/download/confirmation.aspx?id=40855
# Given a directory, parses files and adds meta data to the cr_Build_Scripts
# table.  Also executes script and marks them as executed in the DB.
# Will not execute scripts that are in the DB, but not executed, if this
# becomes a problem we'll have to rework it a bit.

param (
    [string]$DBServer = "mp-int-db.cloudapp.net",
    [string]$path = $(throw "-path is required."),
    [string]$SQLcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\sqlcmd.exe",
    [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
    [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value # Default to environment variable
 )

$exitCode = 0
$SQLCommonParams = @("-U", $DBUser, "-P", $DBPassword, "-S", $DBServer, "-b")

Get-ChildItem $path -recurse -filter *.sql | Foreach-Object {
  $hashObj = Get-FileHash $_.FullName -Algorithm MD5
  $hash = $hashObj.hash
  #Store new scripts in the DB
  $output = & $SQLcmd @SQLCommonParams -Q "IF NOT EXISTS(SELECT 1 FROM [MinistryPlatform].[dbo].[cr_Scripts] WHERE [MD5] = '$hash') INSERT INTO [MinistryPlatform].[dbo].[cr_Scripts] ([Name] ,[MD5]) VALUES ('$_','$hash')"
  
  if($LASTEXITCODE -ne 0){
		echo "File: $_"
		echo "Error: $output"
		$exitCode = $LASTEXITCODE
	} elseif($output -eq "(1 rows affected)"){#If a new script was stored then run it
		$output = & $SQLcmd @SQLCommonParams -I -i $path\$_
		if($LASTEXITCODE -ne 0){
			echo "File: $_"
			echo "Error: $output"
			$exitCode = $LASTEXITCODE
		} else {#If the new script executed well mark it as executed
			$output = & $SQLcmd @SQLCommonParams -Q "Update [MinistryPlatform].[dbo].[cr_Scripts] set executed=1 where [MD5] = '$hash'"
			if($LASTEXITCODE -ne 0){
				echo "File: $_"
				echo "Error: $output"
				$exitCode = $LASTEXITCODE
			}
		}
	}
}

exit $exitCode
