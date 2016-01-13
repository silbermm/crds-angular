# Requires Powershell version 4, if this doesn't work you are likely running version 3
# https://www.microsoft.com/en-us/download/confirmation.aspx?id=40855
# Given a directory, parses files and removes meta data from the cr_Build_Scripts
# table.  So that test SQL can be rerun on a database. 

param (
    [string]$DBServer = "mp-int-db.cloudapp.net",
    [string]$SQLcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\sqlcmd.exe",
    [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
    [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value # Default to environment variable
 )

$exitCode = 0
$SQLCommonParams = @("-U", $DBUser, "-P", $DBPassword, "-S", $DBServer, "-b")

#Hardcoded values for the TestSQL directories. 
$fileNames = (Get-ChildItem ..\TestSQL\01.TestUsers\, ..\TestSQL\02.TestData\, ..\TestSQL\03.TestConfigData\, .\ -filter *.sql -recurse) -join "','"

$output = & $SQLcmd @SQLCommonParams -Q "Delete from [MinistryPlatform].[dbo].[cr_Scripts] where name in ('$fileNames')"

write-host $output;