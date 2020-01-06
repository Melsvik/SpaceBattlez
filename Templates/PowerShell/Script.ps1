Write-Host "ready!"
$gameState = Read-Host

Write-Host "start loop"
while(-not ([string]::IsNullOrEmpty($gameState)))
{
	$FleetCommand = @(  );
	$state =  $gameState | ConvertFrom-Json;

	$MyPlanets = @( $state.planets | where { $_.ownerID -eq 1 })
	$OtherPlanets = @( $state.planets | where { $_.ownerID -ne 1 } | Sort-Object -Property numberOfShips)

	foreach ($MyPlanet in $MyPlanets) 
	{
		$mX = $MyPlanet.Position.x;
		$mY = $MyPlanet.Position.y;

		$minDist = 1000000000;
		$minPlanet;

		foreach ($EnemyPlanet in $OtherPlanets) 
		{
			$eX = $EnemyPlanet.Position.x;
			$eY = $EnemyPlanet.Position.y;

			$dist = [Math]::SQRT(($mX - $eX) * ($mX - $eX) + ($mY - $eY) * ($mY - $eY));

			if($dist -lt $minDist -and $dist -gt 0)
			{
				$minDist = $dist;
				$minPlanet = $EnemyPlanet;
			}
		}

		if(-not ($minPlanet -eq $null))
		{
			$FleetCommand += @{ SourcePlanetID = $MyPlanet.id; DestinationPlanetID = $minPlanet.id; NumberOfUnits = $MyPlanet.numberOfShips - 1  };
		}
	}
	
	$output = ConvertTo-Json -Compress $FleetCommand;
	Write-Host $output;

	$gameState = Read-Host

	if($gameState -eq 'terminate')
	{
		break;
	}
}


