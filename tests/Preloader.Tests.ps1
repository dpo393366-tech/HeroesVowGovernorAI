param([Parameter(Mandatory=$true)][string]$GameDir,[Parameter(Mandatory=$true)][string]$PreviousDir,[Parameter(Mandatory=$true)][string]$ExpectedDir,[switch]$Online)
$ErrorActionPreference='Stop'
$repo=Split-Path $PSScriptRoot
. (Join-Path $repo BuildTools.ps1)
$out=Join-Path $PSScriptRoot ('fixture-'+[guid]::NewGuid().ToString('N').Substring(0,8))
$root=Join-Path $out game
New-Item -ItemType Directory -Force $out,"$root/ThreeKingdom_Data","$root/BepInEx/config","$root/BepInEx/patchers/HeroesVowGovernorAI"|Out-Null
Get-ChildItem "$GameDir/BepInEx/core/*.dll"|ForEach-Object {Copy-Item -LiteralPath $_.FullName -Destination $out}
Copy-Item (Join-Path $repo src/Updater/GovernorAutoUpdate.dll) "$root/BepInEx/patchers/HeroesVowGovernorAI/GovernorAutoUpdate.dll"
foreach($n in 'StrategicSupport','TreasuryAwards','HeroesVowSettings'){
 $dest=if($n -eq 'HeroesVowSettings'){"$root/BepInEx/plugins/$n.dll"}else{"$root/BepInEx/plugins/$n/$n.dll"}
 New-Item -ItemType Directory -Force (Split-Path $dest)|Out-Null
 Copy-Item (Join-Path $PreviousDir "$n.dll") $dest
}
$enabled=if($Online){'true'}else{'false'}
"[Update]`nEnabled = $enabled"|Set-Content "$root/BepInEx/config/local.heroesvow.governor.updater.cfg" -Encoding utf8
Invoke-HVCsc @('-target:exe',('-out:'+(Join-Path $out PreloaderTests.dll)),('-r:'+(Join-Path $out BepInEx.Core.dll)),('-r:'+(Join-Path $out BepInEx.Preloader.Core.dll)),('-r:'+(Join-Path $out Mono.Cecil.dll)),(Join-Path $PSScriptRoot PreloaderTests.cs))
Copy-Item (Join-Path $repo src/StrategicSupport/IntegrationTests.runtimeconfig.json) (Join-Path $out PreloaderTests.runtimeconfig.json)
& dotnet (Join-Path $out PreloaderTests.dll) $root $(if($Online){'online'}else{'offline'})
if($LASTEXITCODE){throw 'Actual preloader test failed'}
if($Online){foreach($n in 'StrategicSupport','TreasuryAwards','HeroesVowSettings'){
 $dest=if($n -eq 'HeroesVowSettings'){"$root/BepInEx/plugins/$n.dll"}else{"$root/BepInEx/plugins/$n/$n.dll"}
 if((Get-FileHash (Join-Path $ExpectedDir "$n.dll")).Hash -ne (Get-FileHash $dest).Hash){throw 'Actual preloader downloaded different payload'}
}}
Write-Output ("Verified fixture: "+$root)
