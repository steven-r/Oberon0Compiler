[CmdletBinding()]
param()

##### Config #####
# Path to GitVersion.exe
$gitversion = "GitVersion.exe"
function Create-AdditionalReleaseArtifacts
{
 param( [string]$Version )

 # Put any custom release logic here (like generating release notes?)
}
### END Config ###

$ErrorActionPreference = "Stop"
trap
{
   Pop-Location
   Write-Error "$_"
   Exit 1
}

Push-Location $PSScriptRoot

# Make sure there are no pending changes
$pendingChanges = & git status --porcelain
if ($pendingChanges -ne $null) 
{
  throw 'You have pending changes, aborting release'
}

# Pull latest, fast-forward only so that it git stops if there is an error
& git fetch origin
& git checkout master
& git merge origin/master --ff-only

# Determine version to release 
$output = & $gitversion /output json
$versionInfoJson = $output -join "`n"

$versionInfo = $versionInfoJson | ConvertFrom-Json
$stableVersion = $versionInfo.MajorMinorPatch

# Create release
Create-AdditionalReleaseArtifacts $stableVersion
# Always create a new commit because some CI servers cannot be triggered by just pushing a tag
& git commit -Am "Create release $stableVersion" --allow-empty 
& git tag $stableVersion
if ($LASTEXITCODE -ne 0) {
    & git reset --hard HEAD^
    throw "No changes detected since last release"
}

& git push origin master --tags

Pop-Location
