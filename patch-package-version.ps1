(Get-Content 'package.json') | 
	Foreach-Object {$_ -replace '"version": "[0-9.]+"',"""version"": ""$env:GitVersion_SemVer""" } | 
	Out-File "package.json"
