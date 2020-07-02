(Get-Content 'package.json') | 
	Foreach-Object {$_ -replace '"version": ".+"',"""version"": ""$env:GitVersion_SemVer""" } | 
	Out-File -Encoding Default "package.json"
