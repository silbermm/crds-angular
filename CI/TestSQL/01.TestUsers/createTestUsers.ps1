param (
    [string]$Endpoint = 'https://gatewayint.crossroads.net/gateway/api/User'
 )

$userList = import-csv .\userList.csv

foreach($user in $userList)
{

	if(![string]::IsNullOrEmpty($user.first))
	{
		write-host "Adding User" $user.first $user.last "with email" $user.email;
		
		$person = @{
			firstName= $user.first
			lastName= $user.last
			email= $user.email
			password= 'welcome'
		};
		$json = $person | ConvertTo-Json;
		
		try {
			$response = Invoke-RestMethod $Endpoint -Method Post -Body $json -ContentType 'application/json'
			write-host "Successfully added user account" $user.email"!" -foregroundcolor green
		}		
		catch{
			write-host "An error occurred adding "$user.first $user.last "with email" $user.email"!" -foregroundcolor red
		}
	}
}


