param([Parameter(Mandatory=$true)][string]$PreviousDir,[Parameter(Mandatory=$true)][string]$PayloadDir)
$ErrorActionPreference='Stop'
. (Join-Path (Split-Path $PSScriptRoot) BuildTools.ps1)
$out=Join-Path $PSScriptRoot ('fixture-'+[guid]::NewGuid().ToString('N').Substring(0,8))
New-Item -ItemType Directory -Path $out | Out-Null
Invoke-HVCsc @('-target:exe',('-out:'+(Join-Path $out UpdaterTests.dll)),(Join-Path $PSScriptRoot UpdaterTests.cs),(Join-Path (Split-Path $PSScriptRoot) src/Updater/Engine.cs))
Copy-Item (Join-Path (Split-Path $PSScriptRoot) src/StrategicSupport/IntegrationTests.runtimeconfig.json) (Join-Path $out UpdaterTests.runtimeconfig.json)
& dotnet (Join-Path $out UpdaterTests.dll) ([IO.Path]::GetFullPath($PreviousDir)) ([IO.Path]::GetFullPath($PayloadDir)) (Join-Path $out game)
if($LASTEXITCODE){throw 'Startup updater regression failed'}
